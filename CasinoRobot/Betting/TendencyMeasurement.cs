using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.Betting
{
    public class TendencyMeasurement
    {

        public int LossCount { get; set; }
        public int SpinCount { get; set; }

        public void Reset()
        {
            LossCount = 0;
            SpinCount = 0;
        }

    }
}
