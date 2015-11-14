using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasinoRobot.ViewModels
{
    public class StreakViewModel : ViewModel
    {

        private int _FollowingZeroCount;
        private int _SurpassingStreaksCount;
        private int _Count;
        private int _StreakLength;
        public int StreakLength
        {
            get
            {
                return _StreakLength;
            }
            set
            {
                _StreakLength = value;
                FirePropertyChanged("StreakLength");
            }
        }

        public int Count
        {
            get
            {
                return _Count;
            }
            set
            {
                _Count = value;
                FirePropertyChanged("CountToSurpassingCountDifference");
                FirePropertyChanged("Count");
            }
        }

        /// <summary>
        /// Count of zeros after streaks of the length
        /// </summary>
        public int FollowingZeroCount
        {
            get
            {
                return _FollowingZeroCount;
            }
            set
            {
                _FollowingZeroCount = value;
                FirePropertyChanged("FollowingZeroCount");
            }
        }

        /// <summary>
        /// Count of all streaks combined, with a length surpassing this streakLength.
        /// </summary>
        public int SurpassingStreaksCount
        {
            get
            {
                return _SurpassingStreaksCount;
            }
            set
            {
                _SurpassingStreaksCount = value;
                FirePropertyChanged("CountToSurpassingCountDifference");
                FirePropertyChanged("SurpassingStreaksCount");
            }
        }

        public int CountToSurpassingCountDifference
        {
            get { return Count - SurpassingStreaksCount; }
        }

    }
}
