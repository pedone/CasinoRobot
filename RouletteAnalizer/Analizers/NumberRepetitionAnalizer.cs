using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouletteAnalizer.Analizers
{
    public class NumberRepetitionAnalizer : AnalizerBase<int>
    {

        private int _ResultAverageDistance;
        private int _ResultMinDistance;
        private int _ResultMaxDistance;

        public int Distance { get; set; }

        public int ResultAverageDistance
        {
            get
            {
                return _ResultAverageDistance;
            }
            private set
            {
                _ResultAverageDistance = value;
                FirePropertyChanged("ResultAverageDistance");
            }
        }
        public int ResultMinDistance
        {
            get
            {
                return _ResultMinDistance;
            }
            private set
            {
                _ResultMinDistance = value;
                FirePropertyChanged("ResultMinDistance");
            }
        }
        public int ResultMaxDistance
        {
            get
            {
                return _ResultMaxDistance;
            }
            private set
            {
                _ResultMaxDistance = value;
                FirePropertyChanged("ResultMaxDistance");
            }
        }

        public NumberRepetitionAnalizer()
        {
            Distance = 0;
        }

        public override void Analize(IEnumerable<int> numbers)
        {
            List<int> distances = new List<int>();
            int distanceMatches = 0;

            List<int> lastNumbers = new List<int>();
            int curDistance = 0;
            for (int i = 0; i < numbers.Count(); i++)
            {
                int curNumber = numbers.ElementAt(i);
                if (lastNumbers.Count == (Distance + 1) && lastNumbers[0] == curNumber)
                {
                    distanceMatches++;

                    distances.Add(curDistance);
                    curDistance = 0;
                }
                else
                    curDistance++;

                lastNumbers.Add(curNumber);
                if (lastNumbers.Count > Distance + 1)
                    lastNumbers.RemoveAt(0);
            }

            //for (int num = 0; num <= 36; num++)
            //{
            //    int curDistance = 0;
            //    int distanceBetweenMatches = 0;
            //    for (int i = 0; i < numbers.Count(); i++)
            //    {
            //        int curNumber = numbers.ElementAt(i);
            //        if (curNumber == num)
            //        {
            //            if (curDistance == Distance)
            //            {
            //                distances.Add(distanceBetweenMatches);
            //                distanceBetweenMatches = 0;

            //                distanceMatches++;
            //            }

            //            curDistance = 0;
            //        }
            //        else
            //        {
            //            distanceBetweenMatches++;
            //            curDistance++;
            //        }
            //    }

            //}

            ResultAverageDistance = (int)distances.Average();
            ResultMinDistance = distances.Min();
            ResultMaxDistance = distances.Max();

            Result = distanceMatches;
        }
    }
}
