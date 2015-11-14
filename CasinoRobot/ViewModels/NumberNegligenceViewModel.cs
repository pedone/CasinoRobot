using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class NumberNegligenceViewModel : ViewModel
    {

        public int Number
        {
            get
            {
                return _Number;
            }
            private set
            {
                _Number = value;
                FirePropertyChanged("Name");
            }
        }

        private int _Number;
        private int _MaxNegligenceCount;
        private int _NegligenceCount;
        public int NegligenceCount
        {
            get
            {
                return _NegligenceCount;
            }
            set
            {
                _NegligenceCount = value;
                if (_NegligenceCount > MaxNegligenceCount)
                    MaxNegligenceCount = _NegligenceCount;

                FirePropertyChanged("NegligenceCount");
            }
        }

        public int MaxNegligenceCount
        {
            get
            {
                return _MaxNegligenceCount;
            }
            private set
            {
                _MaxNegligenceCount = value;
                FirePropertyChanged("MaxNegligenceCount");
            }
        }

        public NumberNegligenceViewModel(int number)
        {
            Number = number;
        }

    }
}
