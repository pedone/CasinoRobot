using CasinoRobot.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Xceed.Wpf.Toolkit;
using CasinoRobot.Extensions;
using System.Windows.Input;
using System.Timers;
using CasinoRobot.Enums;
using System.IO;

namespace CasinoRobot.ViewModels
{
    public class SessionViewModel : ViewModel
    {

        private FileStream _NumberLogStream;
        private StreamWriter _NumberLogWriter;

        private bool _IsPauseScheduled;
        private bool _IsStopScheduled;
        private bool _IsRefreshScheduled;

        private const int MaxSpinTries = 20;
        private const int WheelTurnDelayInMsFast = 300;
        private const int WheelTurnDelayInMsNormal = 1000;
        private const int WheelTurnDelayInMsSlow = 2000;
        private const int TimerInterval = 20;

        /// <summary>
        /// 20s
        /// </summary>
        private const int RefreshWaitTimeInMs = 20000;

        /// <summary>
        /// 15s
        /// </summary>
        private const int RefreshWaitTimeInMsFast = 15000;

        private Stopwatch _SessionTime;
        private Stopwatch _RefreshTime;
        private Timer _ElapsedTimeUpdateTimer;

        private List<ComparisonPixel> _NumZeroOriginalComparisonPixels;
        private CasinoNumberViewModel _NumZeroMarker;

        private int _WheelTurnDelay = WheelTurnDelayInMsFast;

        private StatisticsViewModel Statistics
        {
            get { return ApplicationView.Statistics; }
        }
        private SettingsViewModel Settings
        {
            get { return ApplicationView.Settings; }
        }
        public bool IsRefreshScheduled
        {
            get
            {
                return _IsRefreshScheduled;
            }
            set
            {
                _IsRefreshScheduled = value;
                if (_IsRefreshScheduled)
                    ApplicationView.UpdateStatus("Refresh is scheduled...");
            }
        }
        public bool IsStopScheduled
        {
            get
            {
                return _IsStopScheduled;
            }
            set
            {
                _IsStopScheduled = value;
                if (_IsStopScheduled)
                    ApplicationView.UpdateStatus("Stop is scheduled...");
            }
        }
        public bool IsPauseScheduled
        {
            get
            {
                return _IsPauseScheduled;
            }
            set
            {
                _IsPauseScheduled = value;
                if (_IsPauseScheduled)
                    ApplicationView.UpdateStatus("Pause is scheduled...");
            }
        }

        public TimeSpan ElapsedTime
        {
            get
            {
                if (_SessionTime != null)
                    return _SessionTime.Elapsed;

                return TimeSpan.Zero;
            }
        }

        private int _SpinCount;
        public int SpinCount
        {
            get { return _SpinCount; }
            private set
            {
                _SpinCount = value;
                FirePropertyChanged("SpinCount");
            }
        }

        private ApplicationViewModel ApplicationView
        {
            get { return ApplicationViewModel.Instance; }
        }

        private string _RemainingRefreshTime;
        public string RemainingRefreshTime
        {
            get
            {
                if (!string.IsNullOrEmpty(_RemainingRefreshTime))
                    return _RemainingRefreshTime;
                else
                    return "not started";
            }
            private set
            {
                _RemainingRefreshTime = value;
                FirePropertyChanged("RemainingRefreshTime");
            }
        }

        private string _ElapsedTimeAsString;
        public string ElapsedTimeAsString
        {
            get
            {
                if (!string.IsNullOrEmpty(_ElapsedTimeAsString))
                    return _ElapsedTimeAsString;
                else
                    return "not started";
            }
            private set
            {
                _ElapsedTimeAsString = value;
                FirePropertyChanged("ElapsedTimeAsString");
            }
        }

        private bool _IsRunning;
        public bool IsRunning
        {
            get { return _IsRunning; }
            private set
            {
                _IsRunning = value;
                ApplicationView.UpdateStatus(_IsRunning ? "Running" : "Stopped");

                FirePropertyChanged("IsRunning");
            }
        }

        private bool _IsPaused;
        public bool IsPaused
        {
            get { return _IsPaused; }
            private set
            {
                _IsPaused = value;
                ApplicationView.UpdateStatus(_IsPaused ? "Paused" : "Running");

                FirePropertyChanged("IsPaused");
            }
        }

        private void CheckForAbort()
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
                Pause();
            if (Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
                Stop();
        }

        private async void StartSession()
        {
            if (IsRunning)
                return;

            if (!CheckCanStart())
                return;

            InitWheelTurnDelay();
            InitSessionNumberColors();
            Statistics.InitBalanceStats();
            Statistics.ResetNumberHistory();
            ApplicationView.BetManager.ResetBettingStreak();
            Statistics.ResetStreaks();
            InitNumberLog();

            StartTimer();

            IsRunning = true;
            while (IsRunning)
            {
                await VerifyRouletteBoardIsVisible();

                //Place Bets
                ApplicationView.BetManager.PlaceBets();

                using (var curCasinoScreenshot = ScreenHelper.TakeScreenshot(ApplicationView.GetFixedCasinoArea()))
                {
                    //Turn
                    TurnWheel();

                    CasinoNumberViewModel drawnNumber = null;
                    //Check result
                    if (ApplicationView.BetManager.IsNumberBetPlaced)
                        drawnNumber = await CheckRouletteResult(curCasinoScreenshot);
                    else
                        drawnNumber = await CheckRouletteResult(ApplicationView.Settings.CasinoScreenshot);

                    if (drawnNumber != null)
                    {
                        LogNumber(drawnNumber);
                        //add number to statistics
                        Statistics.AddNumber(drawnNumber);

                        if (Settings.ResetStreaksOnZero && drawnNumber.Number == 0)
                        {
                            ApplicationView.BetManager.ResetBettingStreak();
                            Statistics.ResetStreaks();
                        }
                    }
                    else
                    {
                        ApplicationView.BetManager.ResetBettingStreak();
                        Statistics.ResetStreaks();
                    }

                    //Calculate gain/loss
                    ApplicationView.BetManager.CalculateWinnings(drawnNumber);
                }

                ApplyAutoDecreaseRules();
                CheckStopConditions();

                SpinCount++;
                await RefreshBrowser();

                while (IsPaused)
                {
                    await Task.Delay(200);
                }

                CheckForAbort();
                if (IsStopScheduled)
                    Stop();
                else if (IsPauseScheduled)
                    Pause();
            }
        }

        private void LogNumber(CasinoNumberViewModel drawnNumber)
        {
            if (_NumberLogWriter != null)
            {
                _NumberLogWriter.Write(string.Format(";{0}", drawnNumber.Number));
                _NumberLogWriter.Flush();
            }
        }

        private void InitNumberLog()
        {
            _NumberLogStream = new FileStream(Settings.Logfile, FileMode.Create);
            _NumberLogWriter = new StreamWriter(_NumberLogStream);
        }

        private void CleanupNumberLog()
        {
            _NumberLogWriter.Close();
            _NumberLogStream.Close();

            _NumberLogWriter = null;
            _NumberLogStream = null;
        }

        private void ApplyAutoDecreaseRules()
        {
            //TODO

            //if (Settings.AutoBetStreakThresholdDecrease == 0 && Settings.AutoMaxStreakDecrease == 0)
            //    return;

            //var lastBet = Statistics.BetHistory.LastOrDefault();
            //if (lastBet == null)
            //    return;

            //var timeSinceLastBet = ElapsedTime - lastBet.Time;

            //if (timeSinceLastBet.Minutes > Settings.AutoBetStreakThresholdDecrease)
            //    Settings.BetStreakThreshold--;

            //if (timeSinceLastBet.Minutes > Settings.AutoMaxStreakDecrease)
            //{
            //    Statistics.MaxAlternating1818StreakCount--;
            //    Statistics.MaxAlternatingOddEvenStreakCount--;
            //    Statistics.MaxAlternatingRedBlackStreakCount--;
            //    Statistics.MaxBlackStreakCount--;
            //    Statistics.MaxEvenStreakCount--;
            //    Statistics.MaxFrom19StreakCount--;
            //    Statistics.MaxOddStreakCount--;
            //    Statistics.MaxRedStreakCount--;
            //    Statistics.MaxTo18StreakCount--;
            //}
        }

        /// <summary>
        /// Checks if the roulette board is visible in the browser.
        /// If it is not visible, refresh the browser until it is.
        /// </summary>
        private async Task VerifyRouletteBoardIsVisible()
        {
            if (_NumZeroMarker == null)
            {
                _NumZeroMarker = Settings.CasinoNumbers.FirstOrDefault(cur => cur.Number == 0);
                _NumZeroOriginalComparisonPixels = _NumZeroMarker.GetComparisonPixels();
            }

            var oldStatus = ApplicationView.Status.Message;
            ApplicationView.UpdateStatus("Waiting for roulette board...");

            bool isZeroVisible = IsNumberVisible(_NumZeroMarker, _NumZeroOriginalComparisonPixels);
            if (!isZeroVisible)
            {
                ApplicationView.BetManager.ResetBettingStreak();
                Statistics.ResetStreaks();
            }

            while (!isZeroVisible && (!_IsStopScheduled && !_IsPauseScheduled))
            {
                RouletteBoardHelper.ClickButton(RouletteButtonKind.RefreshPosition);
                ApplicationViewModel.InputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.F5);

                //wait for refresh
                await Task.Delay(RefreshWaitTimeInMsFast);

                isZeroVisible = IsNumberVisible(_NumZeroMarker, _NumZeroOriginalComparisonPixels);
            }

            ApplicationView.UpdateStatus(oldStatus);
        }

        private bool IsNumberVisible(CasinoNumberViewModel numberMarker, List<ComparisonPixel> numberMarkerComparisonPixels)
        {
            var absoluteNumZeroBounds = new Rect(Settings.FixedCasinoWindowPosition.X + numberMarker.Bounds.Left,
                                                Settings.FixedCasinoWindowPosition.Y + numberMarker.Bounds.Top,
                                                numberMarker.Bounds.Width,
                                                numberMarker.Bounds.Height);

            int pixelMatchCount = 0;
            using (var numZeroScreenshot = ScreenHelper.TakeScreenshot(absoluteNumZeroBounds))
            {
                var xOffset = numberMarker.Bounds.Left;
                var yOffset = numberMarker.Bounds.Top;

                foreach (var originalPixel in numberMarkerComparisonPixels)
                {
                    var dPosition = new Point(originalPixel.Position.X - xOffset,
                                                originalPixel.Position.Y - yOffset).ToDrawingPoint();

                    var curPixelColor = numZeroScreenshot.GetPixel(dPosition.X, dPosition.Y);
                    if (originalPixel.Color.Equals(curPixelColor))
                        pixelMatchCount++;
                }
            }

            //20% here is arbitrary. the number should be visible if at least that much of the pixels match.
            return pixelMatchCount >= (numberMarkerComparisonPixels.Count * 0.2);
        }

        private void InitWheelTurnDelay()
        {
            if (Settings.SessionSpeed == SpeedKind.Normal)
                _WheelTurnDelay = WheelTurnDelayInMsNormal;
            else if (Settings.SessionSpeed == SpeedKind.Slow)
                _WheelTurnDelay = WheelTurnDelayInMsSlow;
            else
                _WheelTurnDelay = WheelTurnDelayInMsFast;
        }

        private void CheckStopConditions()
        {
            if (Settings.MaxWinCount > 0 && Statistics.WinCount >= Settings.MaxWinCount)
                Stop();
            else if (Settings.MaxLossCount > 0 && Statistics.LossCount >= Settings.MaxLossCount)
                Stop();
            else if (Settings.MaxWonAmount > 0 && Statistics.TotalWinnings >= Settings.MaxWonAmount)
                Stop();
            else if (Settings.MaxLostAmount > 0 && Statistics.TotalWinnings <= -Settings.MaxLostAmount)
                Stop();
            else if (Settings.TimeLimit > 0 && ElapsedTime.Minutes >= Settings.TimeLimit)
                Stop();
        }

        private async Task RefreshBrowser()
        {
            if (GetTimeToRefresh().Ticks <= 0)
            {
                if (ApplicationView.BetManager.IsInBettingStreak)
                    IsRefreshScheduled = true;
                else
                {
                    RouletteBoardHelper.ClickButton(RouletteButtonKind.RefreshPosition);

                    IsRefreshScheduled = false;
                    _RefreshTime.Stop();
                    //wait for refresh
                    ApplicationView.UpdateStatus("Waiting for refresh...");
                    await Task.Delay(RefreshWaitTimeInMs);
                    ApplicationView.UpdateStatus("Running");

                    ApplicationView.BetManager.ResetBettingStreak();
                    ApplicationView.Statistics.ResetStreaks();
                    _RefreshTime.Restart();
                }
            }
        }

        private void StartTimer()
        {
            _SessionTime = new Stopwatch();
            _RefreshTime = new Stopwatch();

            _ElapsedTimeUpdateTimer = new Timer
            {
                Interval = TimerInterval,
                AutoReset = true
            };
            _ElapsedTimeUpdateTimer.Elapsed += (s, e) =>
                {
                    ElapsedTimeAsString = _SessionTime.Elapsed.ToString(@"hh\:mm\:ss");
                    TimeSpan timeToRefresh = GetTimeToRefresh();
                    if (timeToRefresh.Ticks > 0)
                        RemainingRefreshTime = timeToRefresh.ToString(@"mm\:ss");
                    else
                        RemainingRefreshTime = "00:00";
                };

            _SessionTime.Start();
            _RefreshTime.Start();
            _ElapsedTimeUpdateTimer.Start();
        }

        private TimeSpan GetTimeToRefresh()
        {
            var totalRefreshTime = TimeSpan.FromMinutes(ApplicationView.Settings.TimeToRefresh);
            return new TimeSpan(totalRefreshTime.Ticks - _RefreshTime.Elapsed.Ticks);
        }

        private void InitSessionNumberColors()
        {
            foreach (var number in ApplicationView.Settings.CasinoNumbers)
                number.InitComparisonPixels();
        }

        private async Task<CasinoNumberViewModel> CheckRouletteResult(System.Drawing.Bitmap originalCasinoScreenshot)
        {
            for (int tryCount = 0; tryCount < MaxSpinTries; tryCount++)
            {
                CheckForAbort();
                await Task.Delay(_WheelTurnDelay);

                //TODO take screenshot of number board only, not the whole casino
                using (var curCasinoScreenshot = ScreenHelper.TakeScreenshot(ApplicationView.GetFixedCasinoArea()))
                {
                    foreach (var number in ApplicationView.Settings.CasinoNumbers)
                    {
                        if (number.IsDrawn(curCasinoScreenshot, originalCasinoScreenshot))
                            return number;
                    }
                }
            }

            return null;
        }

        private void TurnWheel()
        {
            RouletteBoardHelper.ClickButton(RouletteButtonKind.Turn);
        }

        private bool CheckCanStart()
        {
            string message = string.Empty;
            if (ApplicationView.IsAdjustingCasinoArea)
                message = "Cannot run while adjusting the casino area!";

            if (string.IsNullOrEmpty(message))
                message = Settings.ValidateSettings();

            if (!string.IsNullOrEmpty(message))
                Xceed.Wpf.Toolkit.MessageBox.Show(message, "Cannot Run", System.Windows.MessageBoxButton.OK);

            return string.IsNullOrEmpty(message);
        }

        public void Pause()
        {
            if (!IsRunning)
                return;

            if (ApplicationView.BetManager.IsInBettingStreak)
                IsPauseScheduled = true;
            else
            {
                _SessionTime.Stop();
                _ElapsedTimeUpdateTimer.Stop();

                IsPauseScheduled = false;
                IsPaused = true;
            }
        }

        public void Resume()
        {
            if (IsPaused)
            {
                ApplicationView.BetManager.ResetBettingStreak();
                ApplicationView.Statistics.ResetStreaks();
                InitWheelTurnDelay();

                _SessionTime.Start();
                _ElapsedTimeUpdateTimer.Start();

                IsPaused = false;
            }
        }

        public void Start()
        {
            if (!IsPaused)
                StartSession();
            else
                Resume();
        }

        public async void Stop()
        {
            if (!IsRunning)
                return;

            if (ApplicationView.BetManager.IsInBettingStreak)
                IsStopScheduled = true;
            else
            {
                CleanupNumberLog();
                _SessionTime.Stop();
                _ElapsedTimeUpdateTimer.Stop();
                SpinCount = 0;

                IsStopScheduled = false;
                IsPaused = false;
                IsRunning = false;

                await Task.Delay(3000);
                ElapsedTimeAsString = string.Empty;
                RemainingRefreshTime = string.Empty;
            }
        }

    }
}
