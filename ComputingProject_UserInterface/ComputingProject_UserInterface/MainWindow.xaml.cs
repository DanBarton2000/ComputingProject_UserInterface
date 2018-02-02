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
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Win32;

namespace ComputingProject_UserInterface {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        int milliseconds = 1000 / 60;
        public static double scale = 250 / Constants.AstronomicalUnit;
        public static uint totalTime = 0;

        enum TimeSteps {
            Second = 1,
            Minute = 60,
            Hour = 3600,
            Day = 24 * 3600
        }

        // Quadtree might need to be celetial object only
        QuadTree<IQuadtreeObject> screen;

        double screenWidth;
        double screenHeight;

        BackgroundWorker worker; // New thread

        TimeController timeController;

        Vector2 centre;

        public int objectSize = 20;

        public MainWindow() {
            InitializeComponent();

            timeController = new TimeController((double)TimeSteps.Day);
            TimeSpan time = new TimeSpan((long)timeController.currentTimeStep);

            screenWidth = Simulation.MaxWidth;
            screenHeight = Simulation.MaxHeight;

            centre = new Vector2(screenWidth, screenHeight);

            screen = new QuadTree<IQuadtreeObject>(new AABB(centre/2, centre/2));

            Console.WriteLine(screen.Boundary.ToString());

            AddObjects();

            ObjectsView.ItemsSource = ObjectManager.AllObjects;
            ObjectsViewVelocityPosition.ItemsSource = ObjectManager.AllObjects;

            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.WorkerSupportsCancellation = true;
            worker.RunWorkerAsync(); 

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = time;
            timer.Start();

            SetDebugTools();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        void AddObjects() {
            CircleCollider cc = new CircleCollider(new Vector2(), objectSize);

            ObjectVisuals earthVis = new ObjectVisuals(Brushes.Blue, 30);
            ObjectVisuals sunVis = new ObjectVisuals(Brushes.Yellow, 80);

            CelestialObject earth = new CelestialObject("Earth", 6E24, new Vector2(30000, 0), new Vector2(2 * Constants.AstronomicalUnit, 0 * Constants.AstronomicalUnit), cc, earthVis);
            Star sun = new Star("Sun", 2E+30, new Vector2(0, 0), new Vector2(2 * Constants.AstronomicalUnit, Constants.AstronomicalUnit), cc, sunVis);
        }

        void SetDebugTools() {
            DebugTools.DebugMode = false;
            DebugTools.UseCollision = true;
            DebugTools.PrintCollisionVelocities = true;
        }

        void Draw(IQuadtreeObject obj, int size) {
            Ellipse circle = new Ellipse();
            circle.Fill = obj.visuals.colour;
            circle.Height = obj.visuals.size;
            circle.Width = obj.visuals.size;

            if (obj.screenPosition.x + obj.visuals.size < centre.x && obj.screenPosition.y + obj.visuals.size < centre.y) {
                Canvas.SetTop(circle, obj.screenPosition.y + (size / 2));
                Canvas.SetLeft(circle, obj.screenPosition.x + (size / 2));
                Simulation.Children.Add(circle);
            }
        }

        void timer_Tick(object sender, EventArgs e) {
            Simulation.Children.Clear();
            foreach (IQuadtreeObject obj in ObjectManager.AllObjects) {
                //Console.WriteLine("Screen Position: " + obj.screenPosition.ToString());
                Draw(obj, obj.visuals.size);
                ObjectsViewVelocityPosition.ItemsSource = null;
                ObjectsViewVelocityPosition.ItemsSource = ObjectManager.AllObjects;

                totalTime += (uint)Math.Abs(timeController.currentTimeStep);

                // Update the advanced windows
                foreach (Window window in Application.Current.Windows) {
                    if (window.GetType() == typeof(Advanced)) {
                        Advanced advanced = (Advanced)window;
                        advanced.CalculateTime();
                    }
                }
            }
        }

        void Worker_DoWork(object sender, EventArgs e) {
            while (!worker.CancellationPending) {
                Stopwatch sw = new Stopwatch();
                ObjectManager.Update(timeController.currentTimeStep, scale, screen);
                sw.Stop();

                int msec = milliseconds - (int)sw.ElapsedMilliseconds;
                if (msec < 1) {
                    msec = 1;
                }

                System.Threading.Thread.Sleep(msec);
            }
        }

        void CreateObject_Click(object sender, EventArgs e) {
            CreateObject createObject = new CreateObject();
            createObject.Show();
        }

        void EditObject_Click(object sender, EventArgs e) {
            object obj = ObjectsView.SelectedItem;

            if (obj != null) {

                CelestialObject celObj = (CelestialObject)obj;

                EditObject editObject = new EditObject();
                editObject.Show();
                editObject.NameTextBox.Text = celObj.Name;
                editObject.MassTextBox.Text = celObj.Mass.ToString();
                editObject.PositionXTextBox.Text = celObj.screenPosition.x.ToString("G4");
                editObject.PositionYTextBox.Text = celObj.screenPosition.y.ToString("G4");
                editObject.VelocityXTextBox.Text = celObj.velocity.x.ToString("G3");
                editObject.VelocityYTextBox.Text = celObj.velocity.y.ToString("G3");
                editObject.ColourTextBox.Text = celObj.visuals.colourName;

                if (obj.GetType() == typeof(Star)) {
                    editObject.ObjectTypeCombo.SelectedIndex = 1;
                }

            }
            else {
                MessageBox.Show("Please select an object.");
            }
        }

        void DeleteObject_Click(object sender, EventArgs e) {
            CelestialObject co = (CelestialObject)ObjectsView.SelectedItem;
            if (co == null) {
                return;
            }
            ObjectManager.AllObjects.Remove(co);

            foreach (IQuadtreeObject obj in ObjectManager.AllObjects) {
                Console.WriteLine(obj.ToString());
            }

            ObjectsView.ItemsSource = null;
            ObjectsView.ItemsSource = ObjectManager.AllObjects;

            ObjectsViewVelocityPosition.ItemsSource = null;
            ObjectsViewVelocityPosition.ItemsSource = ObjectManager.AllObjects;
        }

        void Advanced_Click(object sender, EventArgs e) {
            Advanced advanced = new Advanced();
            advanced.Show();
            if (ObjectsView.SelectedItem.GetType() == typeof(Star)) {
                Star obj = ObjectsView.SelectedItem as Star;

                advanced.SolarMassOfStarTextBox.Text = obj.SolarMassOfStar.ToString();
                advanced.LuminosityOfStarTextBox.Text = obj.Luminosity.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            //Save.WriteXML("test");
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.DefaultExt = ".xml";
            fileBrowser.ShowDialog();
        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            List<CelestialObject> objects = Load.ReadXML("test");
            ObjectManager.ClearObjects();

            objects.ForEach(x => ObjectManager.AddObject(x));

            ObjectsView.ItemsSource = null;
            ObjectsView.ItemsSource = ObjectManager.AllObjects;
        }

        private void Pause_Click(object sender, RoutedEventArgs e) {
            timeController.Pause();
        }

        private void Play_Click(object sender, RoutedEventArgs e) {
            timeController.UnPause();
            timeController.DefaultSpeed();
        }
    }
}
