using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CasinoRobot.ViewModels
{
    public class CasinoBetButtonViewModel : MarkerViewModel
    {

        private double _MultiBetStartBalance;
        public double MultiBetStartBalance
        {
            get
            {
                return _MultiBetStartBalance;
            }
            set
            {
                _MultiBetStartBalance = value;
                FirePropertyChanged("MultiBetStartBalance");
            }
        }

        private bool _IsAvailable;
        /// <summary>
        /// True, if the bet is available in the current roulette mode.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                return _IsAvailable;
            }
            set
            {
                _IsAvailable = value;
                FirePropertyChanged("IsAvailable");
            }
        }

        private double? _Amount;
        public double? Amount
        {
            get { return _Amount; }
            set
            {
                _Amount = value;
                FirePropertyChanged("Amount");
            }
        }

        private Point? _Position;
        public Point? Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                HasValue = _Position != null;

                FirePropertyChanged("Position");
            }
        }

    }
}
