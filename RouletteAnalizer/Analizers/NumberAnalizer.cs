using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RouletteAnalizer.Analizers
{
    public class NumberAnalizer : NotifyPropertyChangedBase
    {

        private int _NumberCount;
        private List<int> _Numbers;

        public int NumberCount
        {
            get
            {
                return _NumberCount;
            }
            private set
            {
                _NumberCount = value;
                FirePropertyChanged("NumberCount");
            }
        }

        public IAnalizer NumberCountAnalizer { get; private set; }
        public IAnalizer NumberRepetitionAnalizer { get; private set; }

        public NumberAnalizer()
        {
            NumberCountAnalizer = new NumberCountAnalizer();
            NumberRepetitionAnalizer = new NumberRepetitionAnalizer();
        }

        public void Analize(string numberLogFile)
        {
            using (FileStream fs = new FileStream(numberLogFile, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                var logContent = sr.ReadToEnd();
                SetNumbers(logContent);
            }

            NumberCountAnalizer.Analize(_Numbers);
        }

        private void SetNumbers(string numbers)
        {
            const char separator = ';';
            _Numbers = new List<int>();

            var splitNumberStrings = numbers.Split(separator);
            foreach (var numString in splitNumberStrings)
            {
                try
                {
                    var number = Convert.ToInt32(numString);
                    _Numbers.Add(number);
                }
                catch
                { }
            }

            NumberCount = _Numbers.Count;
        }


        internal void Analize(IAnalizer analizer)
        {
            if (analizer != null)
                analizer.Analize(_Numbers);
        }
    }
}
