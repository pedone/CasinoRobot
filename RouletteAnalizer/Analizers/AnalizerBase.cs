using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouletteAnalizer.Analizers
{
    public abstract class AnalizerBase<TResult> : NotifyPropertyChangedBase, IAnalizer
    {

        protected TResult _Result;
        public TResult Result
        {
            get
            {
                return _Result;
            }
            protected set
            {
                _Result = value;
                FirePropertyChanged("Result");
            }
        }

        public abstract void Analize(IEnumerable<int> numbers);

    }
}
