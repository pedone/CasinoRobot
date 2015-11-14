using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CasinoRobot.ViewModels
{
    public class CasinoButtonViewModel : MarkerViewModel
    {

        public string Name { get; set; }
        public RouletteButtonKind Kind { get; set; }

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


        public bool IsPositionAbsolute { get; set; }
    }
}
