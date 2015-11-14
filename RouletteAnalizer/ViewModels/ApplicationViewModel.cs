using RouletteAnalizer.Analizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouletteAnalizer.ViewModels
{
    public class ApplicationViewModel : ViewModel
    {
        public static ApplicationViewModel Instance = new ApplicationViewModel();

        public NumberAnalizer Analizer { get; private set; }

        private string _LogFile;
        public string DefaultDirectory { get; private set; }

        public string LogFile
        {
            get
            {
                return _LogFile;
            }
            set
            {
                _LogFile = value;
                FirePropertyChanged("LogFile");
            }
        }

        public ApplicationViewModel()
        {
            DefaultDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CasinoRobot");
            Analizer = new NumberAnalizer();
        }

    }
}
