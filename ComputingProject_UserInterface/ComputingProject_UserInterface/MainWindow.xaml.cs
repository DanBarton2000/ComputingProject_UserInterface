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

namespace ComputingProject_UserInterface {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        int milliseconds = 1000 / 60;
        double scale = 250 / Constants.AstronomicalUnit;

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

        public MainWindow() {
            InitializeComponent();

            timeController = new TimeController((double)TimeSteps.Day);
            TimeSpan time = new TimeSpan((long)timeController.currentTimeStep);

            screenWidth = Simulation.MaxWidth;
            screenHeight = Simulation.MaxHeight;

            centre = new Vector2(screenWidth, screenHeight) / 2;

            screen = new QuadTree<IQuadtreeObject>(new AABB(centre, centre));

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
            CircleCollider cc = new CircleCollider(new Vector2(), 40);

            CelestialObject earth = new CelestialObject("Earth", 6E24, new Vector2(1500, 0), new Vector2(2 * Constants.AstronomicalUnit, 0.5 * Constants.AstronomicalUnit), null, cc);
            CelestialObject sun = new CelestialObject("Sun", 6E30, new Vector2(500, 400), new Vector2(Constants.AstronomicalUnit, Constants.AstronomicalUnit), null, cc);
            CelestialObject sun1 = new CelestialObject("Sun1", 6E30, new Vector2(200, 300), new Vector2(3 * Constants.AstronomicalUnit, 2 * Constants.AstronomicalUnit), null, cc);
        }

        void SetDebugTools() {
            DebugTools.DebugMode = true;
            DebugTools.UseCollision = true;
        }

        void Draw(IQuadtreeObject obj, int size) {
            Ellipse circle = new Ellipse();
            circle.Fill = Brushes.White;
            circle.Height = size;
            circle.Width = size;

            Canvas.SetTop(circle, obj.screenPosition.y);
            Canvas.SetLeft(circle, obj.screenPosition.x);

            Simulation.Children.Add(circle);
        }

        void timer_Tick(object sender, EventArgs e) {
            Simulation.Children.Clear();
            foreach (IQuadtreeObject obj in ObjectManager.AllObjects) {
                //Console.WriteLine("Screen Position: " + obj.screenPosition.ToString());
                Draw(obj, 40);
                ObjectsViewVelocityPosition.ItemsSource = null;
                ObjectsViewVelocityPosition.ItemsSource = ObjectManager.AllObjects;
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
            CreateObject co = new CreateObject();
            co.Show();
        }

        void EditObject_Click(object sender, EventArgs e) {
            CreateObject co = new CreateObject();
            co.Show();
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
            CreateObject co = new CreateObject();
            co.Show();
        }
    }
}
