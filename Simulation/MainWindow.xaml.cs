using SpaceSim;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Path = System.IO.Path;

namespace Simulation {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<SpaceSim.SpaceObject> FocusedObjects = new List<SpaceObject>();
        List<SpaceSim.SpaceObject> solarSystem = new List<SpaceObject>();
        double days = 0;
        Boolean trueScale = false;
        double speed = 0.001;
        Boolean Doubled = false;
        Boolean DisplayNames = true;
        SoundPlayer mediaPlayer;
        Boolean DisplayOrbit = true;
        public MainWindow() {
            InitializeComponent();

            SolarSystem sys = new SolarSystem();
            solarSystem = sys.solarSystem;
            SetFocus(solarSystem[0]);

            List<SpaceObject> GreaterObjects = new List<SpaceObject>();
            FocusedObjects.ForEach(obj => {
                if (obj is Planet || obj is Star || obj is Dwarf) {
                    GreaterObjects.Add(obj);
                }
            });

            Dropdown.ItemsSource = GreaterObjects;
            Dropdown.SelectedIndex = 0;
            Dropdown.DisplayMemberPath = "Name";
            Dropdown.SelectionChanged += dropdownChange;
            Check.Click += (object sender, RoutedEventArgs e) => {
                trueScale = !trueScale;
            };
            reset.Click += (object s, RoutedEventArgs e) => {
                days = 0;
            };
            SpeedCheck.Checked += (object s, RoutedEventArgs e) => {
                Doubled = true;
            };
            SpeedCheck.Unchecked += (object s, RoutedEventArgs e) => {
                Doubled = false;
            };
            displayNames.Click += (object s, RoutedEventArgs e) => {
                DisplayNames = !DisplayNames;
                int x = 0;
                _ = DisplayNames ? ((Button)s).Content = "Hide Names" : ((Button)s).Content = "Show Names";
            };
            displayOrbits.Click += (object s, RoutedEventArgs e) => {
                DisplayOrbit = !DisplayOrbit;
                _ = DisplayOrbit ? ((Button)s).Content = "Hide Orbits" : ((Button)s).Content = "Show Orbits";
            };
            using (FileStream stream = File.Open(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\CornfieldChase.wav"), FileMode.Open)) {
                mediaPlayer = new SoundPlayer(stream);
                mediaPlayer.Load();
                mediaPlayer.Play();
            }
            MuteButton.Click += (object s, RoutedEventArgs e) => {
                SoundplayerChange(s, e);
            };


            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();


        }
        Boolean playing = true;
        public void SoundplayerChange(object s, RoutedEventArgs e) {
            if (playing) {
                mediaPlayer.Stop();
                playing = false;
                ((Button)s).Content = "Start Music";
            } else {
                mediaPlayer.Play();
                playing = true;
                ((Button)s).Content = "Stop Music";
            }
        }
        public void Timer_Tick(object sender, EventArgs e) {

            days += Doubled ? speed / 10 : speed / 1000;
            speedLabel.Content = Doubled ? Math.Round(speed / 10 * 100, 2) : Math.Round(speed / 1000 * 100, 2);
            speedLabel.Content += " Day(s)/s";
            DaysOrbited.Content = $"Years: {Math.Round(days / 365.26, 2).ToString("F2")} Days: {Math.Round(days % 365.26, 2).ToString("F2")}";
            InvalidateVisual();
            Paint();
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

            var value = e.NewValue;
            speed = value;

        }
        private double fontsize = 10;
        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e) {
            var currentScale = (canvas.LayoutTransform as ScaleTransform)?.ScaleX ?? 1.0;
            var newScale = currentScale + e.Delta / 1000.0;
            newScale = Math.Min(Math.Max(newScale, 0.2), 3.0);
            var transform = new ScaleTransform(newScale, newScale);
            canvas.LayoutTransform = transform;

            fontsize = 12 / newScale;


        }
        public double centerRad = 0;
        private void Paint() {
            int counter = 0;
            canvas.Children.Clear();
            foreach (SpaceObject obj in FocusedObjects) {
                if (counter == 0) {
                    Ellipse ellipse = new Ellipse();
                    SolidColorBrush br = new SolidColorBrush(obj.Color);
                    ellipse.Width = trueScale ? obj.Radius : Math.Max(obj.Radius / 10, 50); ellipse.Height = ellipse.Width;
                    ellipse.Fill = br;
                    ellipse.Stroke = new SolidColorBrush(Colors.White);
                    ellipse.StrokeThickness = 0.5;
                    centerRad = ellipse.Width;
                    Tuple<double, double, double> point = obj.getPosition(days, true, 0);
                    Canvas.SetLeft(ellipse, canvas.ActualWidth / 2 - ellipse.Width / 2);
                    Canvas.SetTop(ellipse, canvas.ActualHeight / 2 - ellipse.Height / 2);
                    canvas.Children.Add(ellipse);
                    TextBlock tb = new TextBlock();
                    tb.Text = obj.ToString();
                    tb.Foreground = new SolidColorBrush(Colors.White);
                    tb.FontSize = fontsize;
                    Canvas.SetRight(tb, 20);
                    Canvas.SetBottom(tb, 30);
                    canvas.Children.Add(tb);
                    if (DisplayNames) {
                        TextBlock textBlock = new TextBlock();
                        textBlock.FontSize = fontsize;
                        textBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                        textBlock.Text = obj.Name;


                        Canvas.SetLeft(textBlock, Canvas.GetLeft(ellipse));
                        Canvas.SetTop(textBlock, Canvas.GetTop(ellipse) - textBlock.ActualHeight - 25);
                        canvas.Children.Add(textBlock);
                    }
                } else {
                    Ellipse ellipse = new Ellipse();
                    SolidColorBrush br = new SolidColorBrush(obj.Color);
                    Tuple<double, double, double> point;
                    if (obj is Moon) {
                        ellipse.Width = trueScale ? obj.Radius : Math.Max(obj.Radius * 15, 10); ellipse.Height = ellipse.Width;
                    } else {
                        ellipse.Width = trueScale ? obj.Radius : Math.Max(obj.Radius, 3); ellipse.Height = ellipse.Width;
                    }

                    ellipse.Fill = br;
                    ellipse.Stroke = new SolidColorBrush(Colors.White);
                    ellipse.StrokeThickness = 0.5;
                    point = obj.getPosition(days, false, centerRad);
                    double right;
                    double bottom;
                    right = canvas.ActualWidth / 2 - ellipse.Width / 2 + point.Item1;
                    bottom = canvas.ActualHeight / 2 - ellipse.Height / 2 + point.Item2;
                    Ellipse textPlace = ellipse;

                    Canvas.SetLeft(ellipse, right);
                    Canvas.SetTop(ellipse, bottom);

                    Ellipse orbit = new Ellipse();
                    if (obj is Moon) {
                        orbit.Width = point.Item3;
                        orbit.Height = orbit.Width;
                    } else {
                        orbit.Width = ((obj.OrbitalRadius) / 2000) + centerRad;
                        orbit.Height = orbit.Width;
                    }
                    if (obj is AsteroidBelt) {
                        orbit.Stroke = new LinearGradientBrush(Colors.Firebrick, Colors.Sienna, new Point(0, 0), new Point(1, 1));
                        orbit.StrokeThickness = 5;
                        textPlace = orbit;
                    } else {
                        orbit.Stroke = new SolidColorBrush(Colors.White);
                        orbit.StrokeThickness = 0.5;
                    }
                    if (DisplayOrbit || obj is AsteroidBelt) {
                        Canvas.SetLeft(orbit, canvas.ActualWidth / 2 - orbit.Width / 2);
                        Canvas.SetTop(orbit, canvas.ActualHeight / 2 - orbit.Height / 2);
                        canvas.Children.Add(orbit);
                    }
                    if (!(obj is AsteroidBelt)) {
                        canvas.Children.Add(ellipse);
                    }
                    if (DisplayNames) {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = obj.Name;
                        textBlock.FontSize = fontsize;
                        textBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                        Canvas.SetLeft(textBlock, Canvas.GetLeft(textPlace));
                        Canvas.SetTop(textBlock, Canvas.GetTop(textPlace) - textBlock.ActualHeight - 25);
                        canvas.Children.Add(textBlock);
                    }
                }

                counter++;

            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Left) {
                slider.Value -= 1;
                e.Handled = true;
            } else if (e.Key == Key.Right) {
                slider.Value += 1;
                e.Handled = true;
            } else if (e.Key == Key.Up) {
                Dropdown.SelectedIndex = Math.Max(0, Dropdown.SelectedIndex - 1);
                e.Handled = true;
            } else if (e.Key == Key.Down) {
                Dropdown.SelectedIndex = Math.Min(Dropdown.Items.Count - 1, Dropdown.SelectedIndex + 1);
                e.Handled = true;
            } else if (e.Key == Key.Enter) {
                SpeedCheck.IsChecked = !SpeedCheck.IsChecked;
                e.Handled = true;
            }
        }

        private void SetFocus(SpaceObject obj) {
            FocusedObjects.Clear();
            FocusedObjects.Add(obj);
            solarSystem.ForEach((x) => {
                if (x.Orbits != null && obj.Name.Equals(x.Orbits.Name)) {
                    FocusedObjects.Add(x);
                }
            });

        }

        private void dropdownChange(object sender, SelectionChangedEventArgs e) {
            SpaceObject selectedSpaceObject = Dropdown.SelectedItem as SpaceObject;
            if (selectedSpaceObject != null) {
                SetFocus(selectedSpaceObject);
            }
        }


    }
}
