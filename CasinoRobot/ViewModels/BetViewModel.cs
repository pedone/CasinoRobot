using CasinoRobot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class BetViewModel : ViewModel
    {
        public int GroupId { get; set; }

        public TimeSpan Time { get; private set; }
        public int LastNumber { get; private set; }
        public BettingKind BetKind { get; private set; }
        public int StreakCount { get; private set; }
        public double Amount { get; private set; }

        private BetResultKind _Result;
        public BetResultKind Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value;
                FirePropertyChanged("Result");
            }
        }

        public BetViewModel(BettingKind betKind, int streakCount, int lastNumber, double amount, TimeSpan time)
        {
            BetKind = betKind;
            StreakCount = streakCount;
            LastNumber = lastNumber;
            Amount = amount;
            Time = time;

            Result = BetResultKind.None;
        }

    }
}
