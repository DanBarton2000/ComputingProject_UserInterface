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
            if (!InputIsValid()) {
                return;
            }

            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(MainWindow)) {
                    CelestialObject obj = (CelestialObject)((MainWindow)window).ObjectsView.SelectedItem;

                    foreach (IQuadtreeObject quadObject in ObjectManager.AllObjects) {
                        if (quadObject == obj) {
                            quadObject.Name = NameTextBox.Text;

                            double velocityX;
                            double velocityY;

                            double positionX;
                            double positionY;

                            #region Validation
                            try {
                                velocityX = double.Parse(VelocityXTextBox.Text);
                            }
                            catch {
                                MessageBox.Show("Velocity X is invalid.");
                                return;
                            }

                            try {
                                velocityY = double.Parse(VelocityYTextBox.Text);
                            }
                            catch {
                                MessageBox.Show("Velocity Y is invalid.");
                                return;
                            }

                            try {
                                positionX = double.Parse(PositionXTextBox.Text);
                            }
                            catch {
                                MessageBox.Show("Position X is invalid.");
                                return;
                            }

                            try {
                                positionY = double.Parse(PositionYTextBox.Text);
                            }
                            catch {
                                MessageBox.Show("Position Y is invalid.");
                                return;
                            }

                            try {
                                quadObject.Mass = double.Parse(MassTextBox.Text);
                            }
                            catch {
                                MessageBox.Show("Mass is invalid.");
                                return;
                            }
                            #endregion

                            quadObject.velocity = new Vector2(velocityX, velocityY);
                            
                            if (positionX > MainWindow.MaxPositionX) {
                                MessageBox.Show("Position X too big (Position is measured in AU).");
                                return;
                            }

                            if (positionY > MainWindow.MaxPositionY) {
                                MessageBox.Show("Position Y too big (Position is measured in AU).");
                                return;
                            }

                            quadObject.position = new Vector2(positionX * Constants.AstronomicalUnit, positionY * Constants.AstronomicalUnit);


                            try {
                                obj.visuals.colour = (SolidColorBrush)new BrushConverter().ConvertFromString(ColourTextBox.Text.ToUpper());
                            }
                            catch (Exception) {
                                MessageBox.Show("Invalid hex");
                                return;
                            }
                        }
                    }

                    ((MainWindow)window).ObjectsView.ItemsSource = null;
                    ((MainWindow)window).ObjectsView.ItemsSource = ObjectManager.AllObjects;
                }
            }

            // Close the window down
            Close();
        }

        bool InputIsValid() {
            if (NameTextBox.Text == "") {
                MessageBox.Show("Name cannot be empty!");
                return false;
            }
            else if (VelocityXTextBox.Text == "") {
                MessageBox.Show("Velocity X cannot be empty!");
                return false;
            }
            else if (VelocityYTextBox.Text == "") {
                MessageBox.Show("Velocity Y cannot be empty!");
                return false;
            }
            else if (MassTextBox.Text == "") {
                MessageBox.Show("Mass cannot be empty");
                return false;
            }
            else if (PositionXTextBox.Text == "") {
                MessageBox.Show("Position X cannot be empty!");
                return false;
            }
            else if (PositionYTextBox.Text == "") {
                MessageBox.Show("Position Y cannot be empty!");
                return false;
            }

            return true;
        }
    }
}