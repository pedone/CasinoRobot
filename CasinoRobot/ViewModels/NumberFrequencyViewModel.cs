using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class NumberFrequencyViewModel : ViewModel
    {
        public int Number { get; private set; }
        public int MaxCount { get; set; }

        public NumberFrequencyViewModel(int number)
        {
            Number = number;
        }
    }
}
