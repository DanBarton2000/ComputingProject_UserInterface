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
using System.Resources;
using System.Reflection;
using Microsoft.Win32;

namespace ComputingProject_UserInterface {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        int milliseconds = 1000 / 60;
        public static double scale = 30 / Constants.AstronomicalUnit;
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

        public int objectSize = 5;

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

            // Set the visuals for each object up
            ObjectVisuals earthVis = new ObjectVisuals(Brushes.Blue, objectSize);
            ObjectVisuals venusVis = new ObjectVisuals(Brushes.Beige, objectSize);
            ObjectVisuals mercuryVis = new ObjectVisuals(Brushes.Gray, objectSize);
            ObjectVisuals marsVis = new ObjectVisuals(Brushes.Red, objectSize);
            ObjectVisuals saturnVis = new ObjectVisuals(Brushes.Yellow, objectSize);
            ObjectVisuals jupiterVis = new ObjectVisuals(Brushes.LightGray, objectSize);
            ObjectVisuals uranusVis = new ObjectVisuals(Brushes.LightBlue, objectSize);
            ObjectVisuals neptuneVis = new ObjectVisuals(Brushes.DarkBlue, objectSize);
            ObjectVisuals sunVis = new ObjectVisuals(Brushes.Yellow, 9);

            // Create the objects needed for the solar system
            Star sun = new Star("Sun", 1.989E+30, new Vector2(0, 0), new Vector2(2 * Constants.AstronomicalUnit, 1 * Constants.AstronomicalUnit), cc, sunVis);
            CelestialObject mercury = new CelestialObject("Mercury", 0.3829 * 6E24, new Vector2(45E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 1.466 * Constants.AstronomicalUnit), cc, mercuryVis);
            CelestialObject venus = new CelestialObject("Venus", 6E24 * 0.815, new Vector2(35E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 1.72 * Constants.AstronomicalUnit), cc, venusVis);
            CelestialObject earth = new CelestialObject("Earth", 6E24, new Vector2(30E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 2 * Constants.AstronomicalUnit), cc, earthVis);
            CelestialObject mars = new CelestialObject("Mars", 0.107 * 6E24, new Vector2(24E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 2.666 * Constants.AstronomicalUnit), cc, marsVis);
            CelestialObject jupiter = new CelestialObject("Jupiter", 317.8 * 6E24, new Vector2(13E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 7 * Constants.AstronomicalUnit), cc, jupiterVis);
            CelestialObject saturn = new CelestialObject("Saturn", 8.55 * 6E24, new Vector2(9.68E+3, 0), new Vector2(2 * Constants.AstronomicalUnit, 11 * Constants.AstronomicalUnit), cc, saturnVis);
            CelestialObject urnanus = new CelestialObject("Uranus", 14.5 * 6E24, new Vector2(0, -6.8E+3), new Vector2(21 * Constants.AstronomicalUnit, 2 * Constants.AstronomicalUnit), cc, uranusVis);
            CelestialObject neptune = new CelestialObject("Neptune", 17.147 * 6E24, new Vector2(5.43, 0), new Vector2(0 * Constants.AstronomicalUnit, 32 * Constants.AstronomicalUnit), cc, neptuneVis);
        }

        void SetDebugTools() {
            DebugTools.DebugMode = false;
            DebugTools.UseCollision = true;
            DebugTools.PrintCollisionVelocities = false;
            DebugTools.PrintForces = false;
        }

        void Draw(IQuadtreeObject obj, int size) {
            Ellipse circle = new Ellipse();
            circle.Fill = obj.visuals.colour;
            circle.Height = obj.visuals.size;
            circle.Width = obj.visuals.size;

            if (obj.screenPosition.x + obj.visuals.size < centre.x && obj.screenPosition.y + obj.visuals.size < centre.y) {
                Canvas.SetTop(circle, obj.screenPosition.y + (size / 2) + 200);
                Canvas.SetLeft(circle, obj.screenPosition.x + (size / 2) + 150);
                Simulation.Children.Add(circle);
            }
        }

        void timer_Tick(object sender, EventArgs e) {
            Simulation.Children.Clear();
            foreach (IQuadtreeObject obj in ObjectManager.AllObjects) {
                /*Console.WriteLine("Object: " + obj.Name);
                Console.WriteLine("Collider position: " + ((CircleCollider)obj.collider).centre.ToString());
                Console.WriteLine("Object position: " + obj.position.ToString()); */
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
            if (!(ObjectsView.SelectedItem == null)) {
                if (ObjectsView.SelectedItem.GetType() == typeof(Star)) {
                    Star obj = ObjectsView.SelectedItem as Star;

                    advanced.SolarMassOfStarTextBox.Text = obj.SolarMassOfStar.ToString();
                    advanced.LuminosityOfStarTextBox.Text = obj.Luminosity.ToString();
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            //Save.WriteXML("test");
            SaveFileDialog fileDialogue = new SaveFileDialog();
            fileDialogue.DefaultExt = ".xml";
 
            if (fileDialogue.ShowDialog() == true) {
                Save.WriteXML(fileDialogue.FileName);
            }
            
        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.DefaultExt = ".xml";

            if (fileBrowser.ShowDialog() == true) {
                List<CelestialObject> xmlObjects = Load.ReadXMLFromPath(fileBrowser.FileName);
                if (xmlObjects == null) {
                    MessageBox.Show("Invalid file.");
                }
                else {
                    ObjectManager.ClearObjects();
                    xmlObjects.ForEach(x => ObjectManager.AddObject(x as CelestialObject));

                    ObjectsView.ItemsSource = null;
                    ObjectsView.ItemsSource = ObjectManager.AllObjects;
                }
            }
        }

        private void Pause_Click(object sender, RoutedEventArgs e) {
            timeController.Pause();
        }

        private void Play_Click(object sender, RoutedEventArgs e) {
            timeController.UnPause();
            timeController.DefaultSpeed();
        }

        private void SolarSystem_Click(object sender, RoutedEventArgs e) {
            string filePath = ComputingProject_UserInterface.Resources.Preset.SolarSystem;

            List<CelestialObject> xmlObjects = new List<CelestialObject>();
            xmlObjects = Load.ReadXML(filePath);

            if (xmlObjects == null) {
                MessageBox.Show("Invalid file.");
            }
            else {
                ObjectManager.ClearObjects();
                xmlObjects.ForEach(x => ObjectManager.AddObject(x as CelestialObject));

                ObjectsView.ItemsSource = null;
                ObjectsView.ItemsSource = ObjectManager.AllObjects;

                ObjectsViewVelocityPosition.ItemsSource = null;
                ObjectsViewVelocityPosition.ItemsSource = ObjectManager.AllObjects;
            } 
        }
    }
}
