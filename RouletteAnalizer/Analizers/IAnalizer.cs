using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouletteAnalizer.Analizers
{
    public interface IAnalizer
    {

        void Analize(IEnumerable<int> numbers);

    }
}
