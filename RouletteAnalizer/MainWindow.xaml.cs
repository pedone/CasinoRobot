using Microsoft.Win32;
using RouletteAnalizer.ViewModels;
using System;
using System.Collections.Generic;
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

namespace RouletteAnalizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ApplicationViewModel ApplicationView
        {
            get { return ApplicationViewModel.Instance; }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = ApplicationViewModel.Instance;
        }

        private void AnalizeFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                RestoreDirectory = true,
                InitialDirectory = System.IO.Path.Combine(ApplicationView.DefaultDirectory, "log.txt")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ApplicationView.LogFile = openFileDialog.FileName;
                ApplicationView.Analizer.Analize(openFileDialog.FileName);
            }
        }

        private void AnalizeNumberRepetition_Click(object sender, RoutedEventArgs e)
        {
            ApplicationView.Analizer.Analize(ApplicationView.Analizer.NumberRepetitionAnalizer);
        }

    }
}
