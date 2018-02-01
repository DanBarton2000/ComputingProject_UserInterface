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
                MessageBox.Show("Text boxes cannot be empty!");
                return;
            }

            string name = NameTextBox.Text;
            double mass = double.Parse(MassTextBox.Text);
            // Screen position
            Vector2 position = new Vector2(double.Parse(PositionXTextBox.Text), double.Parse(PositionYTextBox.Text));
            Vector2 velocity = new Vector2(double.Parse(VelocityXTextBox.Text), double.Parse(VelocityYTextBox.Text));

            string objectTypeString = ObjectTypeCombo.Text;

            // Make sure that the user input is in a format that can be used
            if (!IsValidInput(name, mass, position, velocity)) {
                return;
            }

            // Turn the screen position into world coordinates
            position /= MainWindow.scale;

            foreach (IQuadtreeObject obj in ObjectManager.AllObjects) {
                if (obj.position == position) {
                    MessageBox.Show("Object already at that positon!");
                    return;
                }
            }

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

        bool IsValidInput(string name, double mass, Vector2 position, Vector2 velocity) {
            if (mass < 1) {
                MessageBox.Show("Mass is too low!");
                return false;
            }
            else if (position == null) {
                MessageBox.Show("Position is invalid!");
                return false;
            }
            else if (velocity == null) {
                MessageBox.Show("Velocity is invalid!");
                return false;
            }
            else if (position.x < 0) {
                MessageBox.Show("Position X is negative!");
                return false;
            }
            else if (position.y < 0) {
                MessageBox.Show("Position Y is negative!");
                return false;
            }

            return true;
        }
    }
}
