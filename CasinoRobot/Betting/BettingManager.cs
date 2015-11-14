using CasinoRobot.Enums;
using CasinoRobot.Helpers;
using CasinoRobot.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.Betting
{
    public class BettingManager
    {

        private StatisticsViewModel Statistics
        {
            get
            {
                return ApplicationViewModel.Instance.Statistics;
            }
        }

        private SettingsViewModel Settings
        {
            get
            {
                return ApplicationViewModel.Instance.Settings;
            }
        }

        private BettingModeBase _CurrentBettingModeInstance;
        private BettingModeBase CurrentBettingModeInstance
        {
            get
            {
                if (_CurrentBettingModeInstance == null)
                    UpdateBettingModeInstance(Settings.BettingMode);

                return _CurrentBettingModeInstance;
            }
            set
            {
                _CurrentBettingModeInstance = value;
            }
        }

        public bool IsInBettingStreak
        {
            get
            {
                return CurrentBettingModeInstance.IsInBettingStreak;
            }
        }

        public void PlaceBets()
        {
            CurrentBettingModeInstance.PlaceBets();
        }

        public void ResetBettingStreak()
        {
            CurrentBettingModeInstance.ResetBettingStreak();
        }

        public void UpdateBettingModeInstance(BettingSystem bettingMode)
        {
            if (bettingMode == BettingSystem.Martingale)
                _CurrentBettingModeInstance = new MartingaleBetting();
            else if (bettingMode == BettingSystem.LastNumberBetting)
                _CurrentBettingModeInstance = new LastNumberBetting();
            else if (bettingMode == BettingSystem.SingleStreakBetting)
                _CurrentBettingModeInstance = new SingleStreakBetting();
            else if (bettingMode == BettingSystem.TendencyBetting)
                _CurrentBettingModeInstance = new TendencyBetting();
            else if (bettingMode == BettingSystem.NumberNegligence)
                _CurrentBettingModeInstance = new NumberNegligenceBetting();
            else if (bettingMode == BettingSystem.JustLastNumber)
                _CurrentBettingModeInstance = new JustLastNumberBetting();
        }

        internal void CalculateWinnings(CasinoNumberViewModel drawnNumber)
        {
            CurrentBettingModeInstance.CalculateWinnings(drawnNumber);
        }

        public bool IsNumberBetPlaced
        {
            get
            {
                return _CurrentBettingModeInstance.IsNumberBetPlaced;
            }
        }
    }
}
