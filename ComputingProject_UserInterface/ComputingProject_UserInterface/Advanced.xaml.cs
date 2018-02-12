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
using System.Windows.Shapes;
using ComputingProject;

namespace ComputingProject_UserInterface
{
    /// <summary>
    /// Interaction logic for Advanced.xaml
    /// </summary>
    public partial class Advanced : Window
    {
        public Advanced() {
            InitializeComponent();
            ScaleSlider.Value = MainWindow.scale * Constants.AstronomicalUnit;
        }

        /// <summary>
        /// Calculates the time and updates the label
        /// </summary>
        public void CalculateTime() {
            uint totalTime = MainWindow.totalTime;
            uint days = totalTime / (24 * 3600);

            totalTime %= (24 * 3600);

            uint hour = totalTime / 3600;

            totalTime %= 3600;

            uint minutes = totalTime / 60;

            totalTime %= 60;

            uint seconds = totalTime;

            Time.Content = "Total Time - D: " + days + " H: " + hour + " M: " + minutes + " S: " + seconds;
        }

        /// <summary>
        /// Method that is called when the slider is updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            MainWindow.scale = ScaleSlider.Value / Constants.AstronomicalUnit;
        }
    }
}