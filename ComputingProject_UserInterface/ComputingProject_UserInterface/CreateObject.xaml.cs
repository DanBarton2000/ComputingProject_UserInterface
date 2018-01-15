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
using ComputingProject;
using ComputingProject.Collision;

namespace ComputingProject_UserInterface {
    /// <summary>
    /// Interaction logic for CreateObject.xaml
    /// </summary>
    public partial class CreateObject : Window {
        public CreateObject() {
            InitializeComponent();
        }

        private void CreateObjectButton_Click(object sender, RoutedEventArgs e) {
            if (TestInputForEmpty()) {
                MessageBox.Show("Invalid input.");
                return;
            }

            string name = NameTextBox.Text;
            double mass = double.Parse(MassTextBox.Text);
            // Screen position
            Vector2 position = new Vector2(double.Parse(PositionXTextBox.Text), double.Parse(PositionYTextBox.Text));
            Vector2 velocity = new Vector2(double.Parse(VelocityXTextBox.Text), double.Parse(VelocityYTextBox.Text));

            string objectTypeString = ObjectTypeCombo.Text;

            // Turn the screen position into world coordinates
            position /= MainWindow.scale;

            switch (objectTypeString) {
                case "Planet":
                    // Create a new planet object
                    CelestialObject obj = new CelestialObject(name, mass, velocity, position, new CircleCollider(position, 40), new ObjectVisuals());
                    break;
                case "Star":
                    // Create a new star object
                    Star star = new Star(name, mass, velocity, position, new CircleCollider(position, 40), new ObjectVisuals());
                    break;
                default:
                    // Should never reach here
                    break;
            }

            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    // Update the UI
                    ((MainWindow)window).ObjectsView.ItemsSource = null;
                    ((MainWindow)window).ObjectsView.ItemsSource = ObjectManager.AllObjects;
                }
            }

            Close();
        }

        bool TestInputForEmpty() {
            if (NameTextBox.Text == "" || MassTextBox.Text == "" || PositionXTextBox.Text == "" || PositionYTextBox.Text == ""
                || VelocityXTextBox.Text == "" || VelocityYTextBox.Text == "") {
                return true;
            }

            return false;
        }
    }
}
