using CasinoRobot.Enums;
using CasinoRobot.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CasinoRobot.ViewModels
{
    public class StatisticsViewModel : ViewModel
    {
        private double _MaxBalance;
        private double _MinBalance;
        private int _ZeroCount;
        private const int MaxNumberHistoryCount = 50;

        public StatisticsLinesManager StatisticsLines { get; private set; }

        private List<NumberFrequencyViewModel> _NumberCounts;
        public ReadOnlyCollection<NumberFrequencyViewModel> NumberFrequency { get; private set; }

        private ObservableCollection<NumberNegligenceViewModel> _NumberNegligenceStreaks;
        public ReadOnlyObservableCollection<NumberNegligenceViewModel> NumberNegligenceStreaks { get; private set; }

        private ObservableCollection<BetViewModel> _BetHistory;
        public ReadOnlyObservableCollection<BetViewModel> BetHistory { get; private set; }

        private ObservableCollection<NumberBetViewModel> _NumberBetHistory;
        public ReadOnlyObservableCollection<NumberBetViewModel> NumberBetHistory { get; private set; }

        private ObservableCollection<StreakViewModel> _StreakHistory;
        public ReadOnlyObservableCollection<StreakViewModel> StreakHistory { get; private set; }

        private ObservableCollection<StreakViewModel> _ThirdsStreakHistory;
        public ReadOnlyObservableCollection<StreakViewModel> ThirdsStreakHistory { get; private set; }

        private ObservableCollection<int> _NumberHistory = new ObservableCollection<int>();
        public ReadOnlyObservableCollection<int> NumberHistory { get; private set; }

        public NumberKindBalanceViewModel RedBlackBalance { get; private set; }
        public NumberKindBalanceViewModel EvenOddBalance { get; private set; }
        public NumberKindBalanceViewModel To18From19Balance { get; private set; }

        public int? LastNumber { get; private set; }

        public double CurrentBalance
        {
            get { return ApplicationViewModel.Instance.Settings.StartBalance + TotalWinnings; }
        }

        private int _RepetingNumbersCount;
        public int RepetingNumbersCount
        {
            get { return _RepetingNumbersCount; }
            set
            {
                var oldValue = _RepetingNumbersCount;
                _RepetingNumbersCount = value;

                FirePropertyChanged("RepetingNumbersCount");
            }
        }

        /// <summary>
        /// Repeting numbers with one number in between
        /// </summary>
        private int _RepetingNumbersOffset1Count;
        public int RepetingNumbersOffset1Count
        {
            get { return _RepetingNumbersOffset1Count; }
            set
            {
                var oldValue = _RepetingNumbersOffset1Count;
                _RepetingNumbersOffset1Count = value;

                FirePropertyChanged("RepetingNumbersOffset1Count");
            }
        }

        private int _Dozen1StreakCount;
        public int Dozen1StreakCount
        {
            get { return _Dozen1StreakCount; }
            set
            {
                _Dozen1StreakCount = value;
                if (_Dozen1StreakCount > MaxDozen1StreakCount)
                    MaxDozen1StreakCount = _Dozen1StreakCount;

                FirePropertyChanged("Dozen1StreakCount");
            }
        }

        private int _MaxDozen1StreakCount;
        public int MaxDozen1StreakCount
        {
            get { return _MaxDozen1StreakCount; }
            set
            {
                _MaxDozen1StreakCount = value;
                FirePropertyChanged("MaxDozen1StreakCount");
            }
        }

        private int _Dozen2StreakCount;
        public int Dozen2StreakCount
        {
            get { return _Dozen2StreakCount; }
            set
            {
                _Dozen2StreakCount = value;
                if (_Dozen2StreakCount > MaxDozen2StreakCount)
                    MaxDozen2StreakCount = _Dozen2StreakCount;

                FirePropertyChanged("Dozen2StreakCount");
            }
        }

        private int _MaxDozen2StreakCount;
        public int MaxDozen2StreakCount
        {
            get { return _MaxDozen2StreakCount; }
            set
            {
                _MaxDozen2StreakCount = value;
                FirePropertyChanged("MaxDozen2StreakCount");
            }
        }

        private int _Dozen3StreakCount;
        public int Dozen3StreakCount
        {
            get { return _Dozen3StreakCount; }
            set
            {
                _Dozen3StreakCount = value;
                if (_Dozen3StreakCount > MaxDozen3StreakCount)
                    MaxDozen3StreakCount = _Dozen3StreakCount;

                FirePropertyChanged("Dozen3StreakCount");
            }
        }

        private int _MaxDozen3StreakCount;
        public int MaxDozen3StreakCount
        {
            get { return _MaxDozen3StreakCount; }
            set
            {
                _MaxDozen3StreakCount = value;
                FirePropertyChanged("MaxDozen3StreakCount");
            }
        }

        private int _Third1StreakCount;
        public int Third1StreakCount
        {
            get { return _Third1StreakCount; }
            set
            {
                _Third1StreakCount = value;
                if (_Third1StreakCount > MaxThird1StreakCount)
                    MaxThird1StreakCount = _Third1StreakCount;

                FirePropertyChanged("Third1StreakCount");
            }
        }

        private int _MaxThird1StreakCount;
        public int MaxThird1StreakCount
        {
            get { return _MaxThird1StreakCount; }
            set
            {
                _MaxThird1StreakCount = value;
                FirePropertyChanged("MaxThird1StreakCount");
            }
        }

        private int _Third2StreakCount;
        public int Third2StreakCount
        {
            get { return _Third2StreakCount; }
            set
            {
                _Third2StreakCount = value;
                if (_Third2StreakCount > MaxThird2StreakCount)
                    MaxThird2StreakCount = _Third2StreakCount;

                FirePropertyChanged("Third2StreakCount");
            }
        }

        private int _MaxThird2StreakCount;
        public int MaxThird2StreakCount
        {
            get { return _MaxThird2StreakCount; }
            set
            {
                _MaxThird2StreakCount = value;
                FirePropertyChanged("MaxThird2StreakCount");
            }
        }

        private int _Third3StreakCount;
        public int Third3StreakCount
        {
            get { return _Third3StreakCount; }
            set
            {
                _Third3StreakCount = value;
                if (_Third3StreakCount > MaxThird3StreakCount)
                    MaxThird3StreakCount = _Third3StreakCount;

                FirePropertyChanged("Third3StreakCount");
            }
        }

        private int _MaxThird3StreakCount;
        public int MaxThird3StreakCount
        {
            get { return _MaxThird3StreakCount; }
            set
            {
                _MaxThird3StreakCount = value;
                FirePropertyChanged("MaxThird3StreakCount");
            }
        }

        public int ZeroCount
        {
            get
            {
                return _ZeroCount;
            }
            private set
            {
                _ZeroCount = value;
                FirePropertyChanged("ZeroCount");
            }
        }
        public double MaxBalance
        {
            get
            {
                return _MaxBalance;
            }
            private set
            {
                _MaxBalance = value;
                FirePropertyChanged("MaxBalance");
            }
        }
        public double MinBalance
        {
            get
            {
                return _MinBalance;
            }
            private set
            {
                _MinBalance = value;
                FirePropertyChanged("MinBalance");
            }
        }

        public double TotalWinnings
        {
            get { return Math.Round(WinningsAmount - LossesAmount, 2); }
        }

        private int _WinCount;
        public int WinCount
        {
            get
            {
                return _WinCount;
            }
            set
            {
                _WinCount = value;
                FirePropertyChanged("WinCount");
            }
        }

        private int _LossCount;
        public int LossCount
        {
            get
            {
                return _LossCount;
            }
            set
            {
                _LossCount = value;
                FirePropertyChanged("LossCount");
            }
        }

        private double _WinningsAmount;
        public double WinningsAmount
        {
            get
            {
                return _WinningsAmount;
            }
            set
            {
                _WinningsAmount = value;
                UpdateBalance();

                FirePropertyChanged("WinningsAmount");
            }
        }

        private double _LossesAmount;
        public double LossesAmount
        {
            get
            {
                return _LossesAmount;
            }
            set
            {
                _LossesAmount = value;
                UpdateBalance();

                FirePropertyChanged("LossesAmount");
            }
        }

        private IEnumerable<CasinoNumberViewModel> _LastNumberAsCollection;
        public IEnumerable<CasinoNumberViewModel> LastNumberAsCollection
        {
            get
            {
                return _LastNumberAsCollection;
            }
            private set
            {
                _LastNumberAsCollection = value;
                FirePropertyChanged("LastNumberAsCollection");
            }
        }

        private int _AlternatingRedBlackStreakCount;
        public int AlternatingRedBlackStreakCount
        {
            get { return _AlternatingRedBlackStreakCount; }
            set
            {
                _AlternatingRedBlackStreakCount = value;
                if (_AlternatingRedBlackStreakCount > MaxAlternatingRedBlackStreakCount)
                    MaxAlternatingRedBlackStreakCount = _AlternatingRedBlackStreakCount;

                FirePropertyChanged("AlternatingRedBlackStreakCount");
            }
        }

        private int _AlternatingOddEvenStreakCount;
        public int AlternatingOddEvenStreakCount
        {
            get { return _AlternatingOddEvenStreakCount; }
            set
            {
                _AlternatingOddEvenStreakCount = value;
                if (_AlternatingOddEvenStreakCount > MaxAlternatingOddEvenStreakCount)
                    MaxAlternatingOddEvenStreakCount = _AlternatingOddEvenStreakCount;

                FirePropertyChanged("AlternatingOddEvenStreakCount");
            }
        }

        private int _Alternating1818StreakCount;
        public int Alternating1818StreakCount
        {
            get { return _Alternating1818StreakCount; }
            set
            {
                _Alternating1818StreakCount = value;
                if (_Alternating1818StreakCount > MaxAlternating1818StreakCount)
                    MaxAlternating1818StreakCount = _Alternating1818StreakCount;

                FirePropertyChanged("Alternating1818StreakCount");
            }
        }

        private int _RedStreakCount;
        public int RedStreakCount
        {
            get { return _RedStreakCount; }
            set
            {
                _RedStreakCount = value;
                if (_RedStreakCount > MaxRedStreakCount)
                    MaxRedStreakCount = _RedStreakCount;

                FirePropertyChanged("RedStreakCount");
            }
        }

        private int _BlackStreakCount;
        public int BlackStreakCount
        {
            get { return _BlackStreakCount; }
            set
            {
                _BlackStreakCount = value;
                if (_BlackStreakCount > MaxBlackStreakCount)
                    MaxBlackStreakCount = _BlackStreakCount;
                FirePropertyChanged("BlackStreakCount");
            }
        }

        private int _OddStreakCount;
        public int OddStreakCount
        {
            get { return _OddStreakCount; }
            set
            {
                _OddStreakCount = value;
                if (_OddStreakCount > MaxOddStreakCount)
                    MaxOddStreakCount = _OddStreakCount;
                FirePropertyChanged("OddStreakCount");
            }
        }

        private int _EvenStreakCount;
        public int EvenStreakCount
        {
            get { return _EvenStreakCount; }
            set
            {
                _EvenStreakCount = value;
                if (_EvenStreakCount > MaxEvenStreakCount)
                    MaxEvenStreakCount = _EvenStreakCount;
                FirePropertyChanged("EvenStreakCount");
            }
        }

        private int _To18StreakCount;
        public int To18StreakCount
        {
            get { return _To18StreakCount; }
            set
            {
                _To18StreakCount = value;
                if (_To18StreakCount > MaxTo18StreakCount)
                    MaxTo18StreakCount = _To18StreakCount;
                FirePropertyChanged("To18StreakCount");
            }
        }

        private int _From19StreakCount;
        public int From19StreakCount
        {
            get { return _From19StreakCount; }
            set
            {
                _From19StreakCount = value;
                if (_From19StreakCount > MaxFrom19StreakCount)
                    MaxFrom19StreakCount = _From19StreakCount;
                FirePropertyChanged("From19StreakCount");
            }
        }

        private int _MaxAlternatingRedBlackStreakCount;
        public int MaxAlternatingRedBlackStreakCount
        {
            get { return _MaxAlternatingRedBlackStreakCount; }
            set
            {
                _MaxAlternatingRedBlackStreakCount = value;
                FirePropertyChanged("MaxAlternatingRedBlackStreakCount");
            }
        }

        private int _MaxAlternatingOddEvenStreakCount;
        public int MaxAlternatingOddEvenStreakCount
        {
            get { return _MaxAlternatingOddEvenStreakCount; }
            set
            {
                _MaxAlternatingOddEvenStreakCount = value;
                FirePropertyChanged("MaxAlternatingOddEvenStreakCount");
            }
        }

        private int _MaxAlternating1818StreakCount;
        public int MaxAlternating1818StreakCount
        {
            get { return _MaxAlternating1818StreakCount; }
            set
            {
                _MaxAlternating1818StreakCount = value;
                FirePropertyChanged("MaxAlternating1818StreakCount");
            }
        }

        private int _MaxRedStreakCount;
        public int MaxRedStreakCount
        {
            get { return _MaxRedStreakCount; }
            set
            {
                _MaxRedStreakCount = value;
                FirePropertyChanged("MaxRedStreakCount");
            }
        }

        private int _MaxBlackStreakCount;
        public int MaxBlackStreakCount
        {
            get { return _MaxBlackStreakCount; }
            set
            {
                _MaxBlackStreakCount = value;
                FirePropertyChanged("MaxBlackStreakCount");
            }
        }

        private int _MaxOddStreakCount;
        public int MaxOddStreakCount
        {
            get { return _MaxOddStreakCount; }
            set
            {
                _MaxOddStreakCount = value;
                FirePropertyChanged("MaxOddStreakCount");
            }
        }

        private int _MaxEvenStreakCount;
        public int MaxEvenStreakCount
        {
            get { return _MaxEvenStreakCount; }
            set
            {
                _MaxEvenStreakCount = value;
                FirePropertyChanged("MaxEvenStreakCount");
            }
        }

        private int _MaxTo18StreakCount;
        public int MaxTo18StreakCount
        {
            get { return _MaxTo18StreakCount; }
            set
            {
                _MaxTo18StreakCount = value;
                FirePropertyChanged("MaxTo18StreakCount");
            }
        }

        private int _MaxFrom19StreakCount;
        public int MaxFrom19StreakCount
        {
            get { return _MaxFrom19StreakCount; }
            set
            {
                _MaxFrom19StreakCount = value;
                FirePropertyChanged("MaxFrom19StreakCount");
            }
        }

        public IEnumerable<NumberKindBalanceViewModel> NumberKindBalances { get; private set; }

        public StatisticsViewModel()
        {
            StatisticsLines = new StatisticsLinesManager();

            _NumberHistory = new ObservableCollection<int>();
            NumberHistory = new ReadOnlyObservableCollection<int>(_NumberHistory);

            _BetHistory = new ObservableCollection<BetViewModel>();
            BetHistory = new ReadOnlyObservableCollection<BetViewModel>(_BetHistory);

            _NumberBetHistory = new ObservableCollection<NumberBetViewModel>();
            NumberBetHistory = new ReadOnlyObservableCollection<NumberBetViewModel>(_NumberBetHistory);

            _StreakHistory = new ObservableCollection<StreakViewModel>();
            StreakHistory = new ReadOnlyObservableCollection<StreakViewModel>(_StreakHistory);

            _ThirdsStreakHistory = new ObservableCollection<StreakViewModel>();
            ThirdsStreakHistory = new ReadOnlyObservableCollection<StreakViewModel>(_ThirdsStreakHistory);

            _ThirdsStreakHistory.Add(new StreakViewModel { StreakLength = 1 });
            _ThirdsStreakHistory.Add(new StreakViewModel { StreakLength = 2 });
            _ThirdsStreakHistory.Add(new StreakViewModel { StreakLength = 3 });

            _NumberCounts = new List<NumberFrequencyViewModel>();
            NumberFrequency = new ReadOnlyCollection<NumberFrequencyViewModel>(_NumberCounts);

            _NumberNegligenceStreaks = new ObservableCollection<NumberNegligenceViewModel>();
            NumberNegligenceStreaks = new ReadOnlyObservableCollection<NumberNegligenceViewModel>(_NumberNegligenceStreaks);

            for (int i = 0; i <= 36; i++)
                _NumberNegligenceStreaks.Add(new NumberNegligenceViewModel(i));

            RedBlackBalance = new NumberKindBalanceViewModel
            {
                KindA = NumberKind.Red,
                KindB = NumberKind.Black
            };
            EvenOddBalance = new NumberKindBalanceViewModel
            {
                KindA = NumberKind.Even,
                KindB = NumberKind.Odd
            };
            To18From19Balance = new NumberKindBalanceViewModel
            {
                KindA = NumberKind.To18,
                KindB = NumberKind.From19
            };

            NumberKindBalances = new List<NumberKindBalanceViewModel> { RedBlackBalance, EvenOddBalance, To18From19Balance };
        }

        private void UpdateBalance()
        {
            if (MinBalance > CurrentBalance)
                MinBalance = CurrentBalance;
            if (MaxBalance < CurrentBalance)
                MaxBalance = CurrentBalance;

            FirePropertyChanged("TotalWinnings");
            FirePropertyChanged("CurrentBalance");
        }

        public void InitBalanceStats()
        {
            MinBalance = CurrentBalance;
            MaxBalance = CurrentBalance;
            FirePropertyChanged("CurrentBalance");
        }

        public void AddNumber(CasinoNumberViewModel number)
        {
            _NumberHistory.Insert(0, number.Number);
            LastNumberAsCollection = new List<CasinoNumberViewModel> { number };

            AddToNumberCounts(number);
            CountStreaks(number);
            UpdateNumberKindBalances(number);
            StatisticsLines.Update();

            if (_NumberHistory.Count > MaxNumberHistoryCount)
                _NumberHistory.RemoveAt(_NumberHistory.Count - 1);

            LastNumber = number.Number;
        }

        private void AddToNumberCounts(CasinoNumberViewModel number)
        {
            if (number == null)
                return;

            var entry = NumberFrequency.FirstOrDefault(cur => cur.Number == number.Number);
            if (entry == null)
            {
                entry = new NumberFrequencyViewModel(number.Number);
                _NumberCounts.Add(entry);
            }

            entry.MaxCount++;
        }

        private void UpdateNumberKindBalances(CasinoNumberViewModel number)
        {
            bool isRed = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Red);
            bool isBlack = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Black);
            bool isOdd = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Odd);
            bool isEven = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Even);
            bool isTo18 = RouletteHelper.IsNumberOfType(number.Number, NumberKind.To18);
            bool isFrom19 = RouletteHelper.IsNumberOfType(number.Number, NumberKind.From19);

            if (isRed)
                RedBlackBalance.KindACount++;
            else if (isBlack)
                RedBlackBalance.KindBCount++;

            if (isEven)
                EvenOddBalance.KindACount++;
            else if (isOdd)
                EvenOddBalance.KindBCount++;

            if (isTo18)
                To18From19Balance.KindACount++;
            else if (isFrom19)
                To18From19Balance.KindBCount++;

            ApplicationViewModel.Instance.AddNumberKindBalancePoint(NumberKind.Red, RedBlackBalance.CurrentAdvantageKindA);
            ApplicationViewModel.Instance.AddNumberKindBalancePoint(NumberKind.Even, EvenOddBalance.CurrentAdvantageKindA);
            ApplicationViewModel.Instance.AddNumberKindBalancePoint(NumberKind.To18, To18From19Balance.CurrentAdvantageKindA);
        }

        private void CountStreaks(CasinoNumberViewModel number)
        {
            CountNumberNegligenceStreaks(number);
            CountRepetingNumbers();

            bool isZero = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Zero);
            bool isRed = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Red);
            bool isBlack = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Black);
            bool isOdd = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Odd);
            bool isEven = RouletteHelper.IsNumberOfType(number.Number, NumberKind.Even);
            bool isTo18 = RouletteHelper.IsNumberOfType(number.Number, NumberKind.To18);
            bool isFrom19 = RouletteHelper.IsNumberOfType(number.Number, NumberKind.From19);

            DozenKind dozenKind = RouletteHelper.GetDozenKind(number.Number);
            ThirdKind thirdKind = RouletteHelper.GetThirdKind(number.Number);

            if (isZero)
            {
                ZeroCount++;

                if (ApplicationViewModel.Instance.Settings.ResetStreaksOnZero)
                {
                    //commit streaks
                    AddToSimpleStreakHistory(RedStreakCount, true);
                    AddToSimpleStreakHistory(BlackStreakCount, true);
                    AddToSimpleStreakHistory(OddStreakCount, true);
                    AddToSimpleStreakHistory(EvenStreakCount, true);
                    AddToSimpleStreakHistory(To18StreakCount, true);
                    AddToSimpleStreakHistory(From19StreakCount, true);
                    AddToSimpleStreakHistory(Alternating1818StreakCount, true);
                    AddToSimpleStreakHistory(AlternatingOddEvenStreakCount, true);
                    AddToSimpleStreakHistory(AlternatingRedBlackStreakCount, true);

                    AddToThirdsStreakHistory(Dozen1StreakCount, true);
                    AddToThirdsStreakHistory(Dozen2StreakCount, true);
                    AddToThirdsStreakHistory(Dozen3StreakCount, true);
                    AddToThirdsStreakHistory(Third1StreakCount, true);
                    AddToThirdsStreakHistory(Third2StreakCount, true);
                    AddToThirdsStreakHistory(Third3StreakCount, true);

                    //reset
                    ResetStreaks();
                }
                else
                {
                    AddZeroToThirdsStreakHistory(Dozen1StreakCount);
                    AddZeroToThirdsStreakHistory(Dozen2StreakCount);
                    AddZeroToThirdsStreakHistory(Dozen3StreakCount);
                    AddZeroToThirdsStreakHistory(Third1StreakCount);
                    AddZeroToThirdsStreakHistory(Third2StreakCount);
                    AddZeroToThirdsStreakHistory(Third3StreakCount);
                }

                return;
            }

            if (dozenKind == DozenKind.First)
                Dozen1StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Dozen1StreakCount);
                Dozen1StreakCount = 0;
            }

            if (dozenKind == DozenKind.Second)
                Dozen2StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Dozen2StreakCount);
                Dozen2StreakCount = 0;
            }

            if (dozenKind == DozenKind.Third)
                Dozen3StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Dozen3StreakCount);
                Dozen3StreakCount = 0;
            }

            if (thirdKind == ThirdKind.First)
                Third1StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Third1StreakCount);
                Third1StreakCount = 0;
            }

            if (thirdKind == ThirdKind.Second)
                Third2StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Third2StreakCount);
                Third2StreakCount = 0;
            }

            if (thirdKind == ThirdKind.Third)
                Third3StreakCount++;
            else
            {
                AddToThirdsStreakHistory(Third3StreakCount);
                Third3StreakCount = 0;
            }

            if (isRed)
                RedStreakCount++;
            else
            {
                AddToSimpleStreakHistory(RedStreakCount);
                RedStreakCount = 0;
            }

            if (isBlack)
                BlackStreakCount++;
            else
            {
                AddToSimpleStreakHistory(BlackStreakCount);
                BlackStreakCount = 0;
            }

            if (isOdd)
                OddStreakCount++;
            else
            {
                AddToSimpleStreakHistory(OddStreakCount);
                OddStreakCount = 0;
            }

            if (isEven)
                EvenStreakCount++;
            else
            {
                AddToSimpleStreakHistory(EvenStreakCount);
                EvenStreakCount = 0;
            }

            if (isTo18)
                To18StreakCount++;
            else
            {
                AddToSimpleStreakHistory(To18StreakCount);
                To18StreakCount = 0;
            }

            if (isFrom19)
                From19StreakCount++;
            else
            {
                AddToSimpleStreakHistory(From19StreakCount);
                From19StreakCount = 0;
            }

            //Count alternating streaks
            if (LastNumber.HasValue)
            {
                bool isLastNumberZero = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.Zero);
                bool isLastNumberRed = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.Red);
                bool isLastNumberBlack = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.Black);
                bool isLastNumberOdd = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.Odd);
                bool isLastNumberEven = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.Even);
                bool isLastNumberTo18 = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.To18);
                bool isLastNumberFrom19 = RouletteHelper.IsNumberOfType(LastNumber.Value, NumberKind.From19);

                if (isLastNumberRed && isBlack ||
                    isLastNumberBlack && isRed)
                    AlternatingRedBlackStreakCount++;
                else
                {
                    AddToSimpleStreakHistory(AlternatingRedBlackStreakCount);
                    AlternatingRedBlackStreakCount = isZero ? 0 : 1;
                }

                if (isLastNumberOdd && isEven ||
                    isLastNumberEven && isOdd)
                    AlternatingOddEvenStreakCount++;
                else
                {
                    AddToSimpleStreakHistory(AlternatingOddEvenStreakCount);
                    AlternatingOddEvenStreakCount = isZero ? 0 : 1;
                }

                if (isLastNumberTo18 && isFrom19 ||
                    isLastNumberFrom19 && isTo18)
                    Alternating1818StreakCount++;
                else
                {
                    AddToSimpleStreakHistory(Alternating1818StreakCount);
                    Alternating1818StreakCount = isZero ? 0 : 1;
                }
            }
            else
            {
                if (isBlack || isRed)
                    AlternatingRedBlackStreakCount++;

                if (isEven || isOdd)
                    AlternatingOddEvenStreakCount++;

                if (isFrom19 || isTo18)
                    Alternating1818StreakCount++;
            }

            UpdateSimpleStreakCounts();
            UpdateThirdsStreakCounts();
        }

        private void CountRepetingNumbers()
        {
            if (NumberHistory.Count >= 2 && NumberHistory[0] == NumberHistory[1])
                RepetingNumbersCount++;
            if (NumberHistory.Count >= 3 && NumberHistory[0] == NumberHistory[2])
                RepetingNumbersOffset1Count++;
        }

        private void CountNumberNegligenceStreaks(CasinoNumberViewModel number)
        {
            if (number == null)
                return;

            var curNumberEntry = _NumberNegligenceStreaks.FirstOrDefault(cur => cur.Number == number.Number);
            if (curNumberEntry != null)
                curNumberEntry.NegligenceCount = 0;

            foreach (var entry in _NumberNegligenceStreaks.Where(cur => cur.Number != number.Number))
                entry.NegligenceCount++;
        }

        private void UpdateSimpleStreakCounts()
        {
            //update largerStreakCount
            foreach (var entry in StreakHistory)
            {
                int curSurpassingStreakCount = 0;
                foreach (var surpassingStreak in StreakHistory.OrderByDescending(cur => cur.StreakLength).Where(cur => cur.StreakLength > entry.StreakLength))
                    curSurpassingStreakCount += surpassingStreak.Count;

                entry.SurpassingStreaksCount = curSurpassingStreakCount;
            }
        }

        private void UpdateThirdsStreakCounts()
        {
            //update largerStreakCount
            foreach (var entry in ThirdsStreakHistory)
            {
                int curSurpassingStreakCount = 0;
                foreach (var surpassingStreak in ThirdsStreakHistory.OrderByDescending(cur => cur.StreakLength).Where(cur => cur.StreakLength > entry.StreakLength))
                    curSurpassingStreakCount += surpassingStreak.Count;

                entry.SurpassingStreaksCount = curSurpassingStreakCount;
            }
        }

        private void AddToThirdsStreakHistory(int streakLength, bool followedByZero = false)
        {
            if (streakLength < 1)
                return;

            var historyEntry = ThirdsStreakHistory.FirstOrDefault(cur => cur.StreakLength == streakLength);
            if (historyEntry == null)
            {
                historyEntry = new StreakViewModel { StreakLength = streakLength };
                _ThirdsStreakHistory.Add(historyEntry);
            }

            historyEntry.Count++;
            if (followedByZero)
                historyEntry.FollowingZeroCount++;
        }

        private void AddZeroToThirdsStreakHistory(int streakLength)
        {
            if (streakLength < 1)
                return;

            var historyEntry = ThirdsStreakHistory.FirstOrDefault(cur => cur.StreakLength == streakLength);
            if (historyEntry == null)
            {
                historyEntry = new StreakViewModel { StreakLength = streakLength };
                _ThirdsStreakHistory.Add(historyEntry);
            }

            historyEntry.FollowingZeroCount++;
        }

        private void AddToSimpleStreakHistory(int streakLength, bool followedByZero = false)
        {
            if (streakLength < 1)
                return;

            var historyEntry = StreakHistory.FirstOrDefault(cur => cur.StreakLength == streakLength);
            if (historyEntry == null)
            {
                historyEntry = new StreakViewModel { StreakLength = streakLength };
                _StreakHistory.Add(historyEntry);
            }

            historyEntry.Count++;
            if (followedByZero)
                historyEntry.FollowingZeroCount++;
        }

        public void AddBetToHistory(BetViewModel bet)
        {
            if (!_BetHistory.Contains(bet))
                _BetHistory.Add(bet);
        }

        public void AddBetToHistory(NumberBetViewModel bet)
        {
            if (!_NumberBetHistory.Contains(bet))
                _NumberBetHistory.Add(bet);
        }

        internal void ResetNumberHistory()
        {
            _NumberHistory.Clear();
            LastNumberAsCollection = null;
            LastNumber = null;
            RepetingNumbersCount = 0;
            RepetingNumbersOffset1Count = 0;
        }

        public void ResetBetHistory()
        {
            _BetHistory.Clear();
        }

        internal void ResetStreaks()
        {
            RedStreakCount = 0;
            BlackStreakCount = 0;
            OddStreakCount = 0;
            EvenStreakCount = 0;
            To18StreakCount = 0;
            From19StreakCount = 0;
            Alternating1818StreakCount = 0;
            AlternatingOddEvenStreakCount = 0;
            AlternatingRedBlackStreakCount = 0;

            Dozen1StreakCount = 0;
            Dozen2StreakCount = 0;
            Dozen3StreakCount = 0;

            Third1StreakCount = 0;
            Third2StreakCount = 0;
            Third3StreakCount = 0;
        }

        public void ResetMaxStreakCounts()
        {
            MaxAlternating1818StreakCount = 0;
            MaxAlternatingOddEvenStreakCount = 0;
            MaxAlternatingRedBlackStreakCount = 0;
            MaxBlackStreakCount = 0;
            MaxEvenStreakCount = 0;
            MaxFrom19StreakCount = 0;
            MaxOddStreakCount = 0;
            MaxRedStreakCount = 0;
            MaxTo18StreakCount = 0;

            MaxDozen1StreakCount = 0;
            MaxDozen2StreakCount = 0;
            MaxDozen3StreakCount = 0;

            MaxThird1StreakCount = 0;
            MaxThird2StreakCount = 0;
            MaxThird3StreakCount = 0;
        }

        internal void ResetStreaksWinningsAndLosses()
        {
            WinCount = 0;
            LossCount = 0;
            WinningsAmount = 0;
            LossesAmount = 0;
        }

        public void ResetBalance()
        {
            MinBalance = ApplicationViewModel.Instance.Settings.StartBalance;
            MaxBalance = ApplicationViewModel.Instance.Settings.StartBalance;
        }
    }
}
