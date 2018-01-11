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

namespace ComputingProject_UserInterface {
    /// <summary>
    /// Interaction logic for EditObject.xaml
    /// </summary>
    public partial class EditObject : Window {
        public EditObject() {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    CelestialObject obj = (CelestialObject)((MainWindow)window).ObjectsView.SelectedItem;

                    foreach (IQuadtreeObject quadObject in ObjectManager.AllObjects) {
                        if (quadObject == obj) {
                            quadObject.Name = NameTextBox.Text;
                            Vector2 velocity = new Vector2(double.Parse(VelocityXTextBox.Text), double.Parse(VelocityYTextBox.Text));
                            quadObject.velocity = velocity;
                            Vector2 screenPosition = new Vector2(double.Parse(PositionXTextBox.Text), double.Parse(PositionYTextBox.Text));
                            quadObject.position = screenPosition / MainWindow.scale;
                            quadObject.Mass = double.Parse(MassTextBox.Text);
                        }
                    }
                }
            }
            // Close the window down
            Close();
        }
    }
}