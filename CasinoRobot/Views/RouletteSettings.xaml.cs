using CasinoRobot.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CasinoRobot.Views
{
    /// <summary>
    /// Interaction logic for RouletteSettings.xaml
    /// </summary>
    public partial class RouletteSettings : UserControl
    {
        private string _DefaultDirectory;

        public ApplicationViewModel ApplicationView
        {
            get { return ApplicationViewModel.Instance; }
        }

        public RouletteSettings()
        {

            InitializeComponent();
        }

        private void SelectLogFile_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(ApplicationView.Settings.DefaultDirectory))
                Directory.CreateDirectory(ApplicationView.Settings.DefaultDirectory);

            string logfile = ApplicationViewModel.Instance.Settings.Logfile;
            SaveFileDialog dialog = new SaveFileDialog
            {
                RestoreDirectory = true,
                FileName = logfile,
                InitialDirectory = ApplicationView.Settings.DefaultDirectory,
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                ApplicationViewModel.Instance.Settings.Logfile = dialog.FileName;
            }
        }

        protected void CasinoMarker_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ApplicationView.CurrentlyAssigningMarker = ((FrameworkElement)sender).DataContext as ViewModel;
        }

    }
}
