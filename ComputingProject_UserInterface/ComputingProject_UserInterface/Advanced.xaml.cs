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

namespace ComputingProject_UserInterface
{
    /// <summary>
    /// Interaction logic for Advanced.xaml
    /// </summary>
    public partial class Advanced : Window
    {
        public Advanced()
        {
            InitializeComponent();
        }

        public void CalculateTime() {
            int totalTime = (int)MainWindow.totalTime;
            int days = totalTime / (24 * 3600);

            totalTime %= (24 * 3600);

            int hour = totalTime / 3600;

            totalTime %= 3600;

            int minutes = totalTime / 60;

            totalTime %= 60;

            int seconds = totalTime;

            Time.Content = "Total Time - D: " + days + " H: " + " M: " + minutes + " S: " + seconds;
        }
    }
}