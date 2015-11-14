using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasinoRobot.ViewModels
{
    public class StatusViewModel : ViewModel
    {

        private string _Message;
        public string Message
        {
            get
            {
                return _Message;
            }
            set
            {
                _Message = value;
                FirePropertyChanged("Message");
            }
        }

    }
}
