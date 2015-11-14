using CasinoRobot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class NumberBetViewModel : ViewModel
    {
        public int GroupId { get; set; }
        public int Number { get; private set; }
        public double Amount { get; private set; }
        public TimeSpan Time { get; private set; }

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

        public NumberBetViewModel(int number, double amount, TimeSpan time)
        {
            Number = number;
            Amount = amount;
            Time = time;
        }
    }
}
