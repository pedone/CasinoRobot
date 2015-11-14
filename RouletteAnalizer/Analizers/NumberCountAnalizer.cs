using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace RouletteAnalizer.Analizers
{
    public class NumberCountAnalizer : AnalizerBase<ReadOnlyObservableCollection<NumberCountInfo>>
    {

        private ObservableCollection<NumberCountInfo> _ResultInternal;

        public NumberCountAnalizer()
        {
            _ResultInternal = new ObservableCollection<NumberCountInfo>();
            Result = new ReadOnlyObservableCollection<NumberCountInfo>(_ResultInternal);
        }

        public override void Analize(IEnumerable<int> numbers)
        {
            _ResultInternal.Clear();

            for (int num = 0; num <= 36; num++)
            {
                int numCount = 0;
                List<int> distances = new List<int>();

                int curDistance = 0;
                for (int i = 0; i < numbers.Count(); i++)
                {
                    int curNumber = numbers.ElementAt(i);
                    if (curNumber == num)
                    {
                        numCount++;
                        distances.Add(curDistance);

                        curDistance = 0;
                    }
                    else
                    {
                        curDistance++;
                    }
                }

                int averageDistance = (int)distances.Average();
                int minDistance = distances.Min();
                int maxDistance = distances.Max();
                
                _ResultInternal.Add(new NumberCountInfo
                {
                    Number = num,
                    Count = numCount,
                    MinDistance = minDistance,
                    MaxDistance = maxDistance,
                    AverageDistance = averageDistance
                });
            }
        }
    }
}
