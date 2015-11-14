using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class NumberKindBalanceViewModel : ViewModel
    {

        private int _KindAMaxAdvantageTotal;
        private int _KindBMaxAdvantageTotal;

        public NumberKind KindA { get; set; }
        public NumberKind KindB { get; set; }

        private double _Center;
        private int _MaxAdvantageKindB;
        private int _MaxAdvantageKindA;

        private int _KindBCount;
        private int _KindACount;
        public int KindACount
        {
            get
            {
                return _KindACount;
            }
            set
            {
                _KindACount = value;

                UpdateAdvantageTotalKindA();
                UpdateMaxAdvantages();
                FirePropertyChanged("CurrentAdvantageKindA");
                FirePropertyChanged("CurrentAdvantageKindB");

                FirePropertyChanged("KindACount");
            }
        }

        public int KindBCount
        {
            get
            {
                return _KindBCount;
            }
            set
            {
                _KindBCount = value;

                UpdateAdvantageTotalKindB();
                UpdateMaxAdvantages();
                FirePropertyChanged("CurrentAdvantageKindA");
                FirePropertyChanged("CurrentAdvantageKindB");

                FirePropertyChanged("KindBCount");
            }
        }

        /// <summary>
        /// The center between all the advantages of KindA against KindB
        /// </summary>
        public double Center
        {
            get
            {
                return _Center;
            }
            private set
            {
                _Center = value;
                FirePropertyChanged("Center");
            }
        }

        /// <summary>
        /// Max Count Kind A was ahead yet.
        /// </summary>
        public int MaxAdvantageKindA
        {
            get
            {
                return _MaxAdvantageKindA;
            }
            private set
            {
                _MaxAdvantageKindA = value;
                FirePropertyChanged("MaxAdvantageKindA");
            }
        }

        /// <summary>
        /// Max Count Kind B was ahead yet.
        /// </summary>
        public int MaxAdvantageKindB
        {
            get
            {
                return _MaxAdvantageKindB;
            }
            private set
            {
                _MaxAdvantageKindB = value;
                FirePropertyChanged("MaxAdvantageKindB");
            }
        }

        public int CurrentAdvantageKindA
        {
            get
            {
                return KindACount - KindBCount;
            }
        }

        public int CurrentAdvantageKindB
        {
            get
            {
                return KindBCount - KindACount;
            }
        }

        private void UpdateAdvantageTotalKindA()
        {
            _KindAMaxAdvantageTotal += CurrentAdvantageKindA;
            UpdateCenter();
        }
        private void UpdateAdvantageTotalKindB()
        {
            _KindBMaxAdvantageTotal += CurrentAdvantageKindB;
            UpdateCenter();
        }

        private void UpdateCenter()
        {
            Center = (_KindAMaxAdvantageTotal - _KindBMaxAdvantageTotal) / (_KindACount + _KindBCount);
        }

        private void UpdateMaxAdvantages()
        {
            MaxAdvantageKindA = Math.Max(CurrentAdvantageKindA, MaxAdvantageKindA);
            MaxAdvantageKindB = Math.Max(CurrentAdvantageKindB, MaxAdvantageKindB);
        }
    }
}
