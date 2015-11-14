using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CasinoRobot.ViewModels
{
    public class MarkerViewModel : ViewModel
    {

        private bool _HasValue;
        public bool HasValue
        {
            get { return _HasValue; }
            set
            {
                _HasValue = value;
                FirePropertyChanged("HasValue");
            }
        }

    }
}
