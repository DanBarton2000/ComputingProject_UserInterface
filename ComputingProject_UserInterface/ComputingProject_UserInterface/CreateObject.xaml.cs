﻿using System;
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
            string name = NameTextBox.Text;
            double mass = double.Parse(MassTextBox.Text);
            // Screen position
            Vector2 position = new Vector2(double.Parse(PositionXTextBox.Text), double.Parse(PositionYTextBox.Text));
            Vector2 velocity = new Vector2(double.Parse(VelocityXTextBox.Text), double.Parse(VelocityYTextBox.Text));

            string objectTypeString = ObjectTypeCombo.Text;

            position /= MainWindow.scale;

            switch (objectTypeString) {
                case "Planet":
                    new CelestialObject(name, mass, velocity, position, null, new CircleCollider(position, 40));
                    break;
                case "Star":
                    new Star();
                    break;
                default:
                    break;
            }
        }
    }
}
