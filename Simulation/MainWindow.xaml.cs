using SpaceSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Navigation;

using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;

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
								MediaPlayer mediaPlayer;

								public MainWindow() {
												InitializeComponent();

												InitSolarSystem();
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
												SpeedCheck.Click += (object s, RoutedEventArgs e) => {
																Doubled = !Doubled;
												};
												displayNames.Click += (object s, RoutedEventArgs e) => {
																DisplayNames = !DisplayNames;
												};

												mediaPlayer = new MediaPlayer();


												mediaPlayer.Open(new Uri("pack://siteoforigin:,,,/CornfieldChase.mp3"));

												mediaPlayer.Volume = 1;

												mediaPlayer.Play();
												mediaPlayer.MediaFailed += (sender, args) =>
												{

																MessageBox.Show(args.ErrorException.Message);
												};
												mediaPlayer.MediaEnded += (sender, args) =>
												{

																RestartSong();
												};

												DispatcherTimer timer = new DispatcherTimer();
												timer.Interval = TimeSpan.FromMilliseconds(10);
												timer.Tick += Timer_Tick;
												timer.Start();
								

				}

								private void RestartSong() {
												if (mediaPlayer != null && mediaPlayer.Source != null) {
																if (mediaPlayer.NaturalDuration.HasTimeSpan &&
																				mediaPlayer.NaturalDuration.TimeSpan <= mediaPlayer.Position) {
																				mediaPlayer.Position = TimeSpan.Zero;
																				mediaPlayer.Play();
																}
												}
								}
								public void Timer_Tick(object sender, EventArgs e) {

												days += Doubled ? speed / 10 : speed/1000;
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
																				Canvas.SetLeft(ellipse, canvas.ActualWidth / 2- ellipse.Width / 2);
																				Canvas.SetTop(ellipse, canvas.ActualHeight / 2- ellipse.Height / 2);
																				canvas.Children.Add(ellipse);
																				TextBlock tb = new TextBlock();
																				tb.Text = obj.ToString();
																				tb.Foreground= new SolidColorBrush(Colors.White);
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
																								ellipse.Width =  trueScale ?  obj.Radius : Math.Max(obj.Radius * 15, 10); ellipse.Height = ellipse.Width;
																				} else {
																								ellipse.Width = trueScale ?  obj.Radius : Math.Max(obj.Radius, 3); ellipse.Height = ellipse.Width;
																				}
																								
																				ellipse.Fill = br;
																				ellipse.Stroke = new SolidColorBrush(Colors.White);
																				ellipse.StrokeThickness = 0.5;
																				point = obj.getPosition(days, false, centerRad);
																				double right;
																				double bottom;
																				right = canvas.ActualWidth / 2- ellipse.Width / 2 + point.Item1;
																				bottom = canvas.ActualHeight / 2- ellipse.Height / 2 + point.Item2;
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
																				//if (DisplayNames) { todo
																								Canvas.SetLeft(orbit, canvas.ActualWidth / 2 - orbit.Width / 2);
																								Canvas.SetTop(orbit, canvas.ActualHeight / 2 - orbit.Height / 2);
																								canvas.Children.Add(orbit);
																				//}
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

								private void InitSolarSystem() {
												SpaceObject Sun = new Star("Sun", 0, 696, 0, 27, Colors.OrangeRed, null);
												solarSystem.Add(Sun);
												// Belts
												SpaceObject mainAst = new AsteroidBelt("The main belt", 418874, 0, 2027.0942, 0, Colors.SaddleBrown, Sun);
												solarSystem.Add(mainAst);
												// Planets
												SpaceObject Mercury = new Planet("Mercury", 57910, 2.439, 87.97, 59, Colors.SlateGray, Sun);
												SpaceObject Venus = new Planet("Venus", 108200, 6.051, 224.7, 243, Colors.White, Sun);
												SpaceObject Earth = new Planet("Earth", 149600, 6.371, 365.26, 1, Colors.DodgerBlue, Sun);
												SpaceObject Mars = new Planet("Mars", 227940, 3.389, 686.98, 1.026, Colors.Firebrick, Sun);
												SpaceObject Jupiter = new Planet("Jupiter", 778330, 69.911, 4332.71, 0.417, Colors.Brown, Sun);
												SpaceObject Saturn = new Planet("Saturn", 1429400, 58.232, 10759.5, 0.446, Colors.SandyBrown, Sun);
												SpaceObject Uranus = new Planet("Uranus", 2870990, 25.362, 30685, 0.708, Colors.Teal, Sun);
												SpaceObject Neptun = new Planet("Neptune", 4504300, 24.622, 60190, 0.671, Colors.Cyan, Sun);
												solarSystem.Add(Mercury); solarSystem.Add(Venus); solarSystem.Add(Earth); solarSystem.Add(Mars);
												solarSystem.Add(Jupiter); solarSystem.Add(Saturn); solarSystem.Add(Uranus); solarSystem.Add(Neptun);

												// Dwarf
												SpaceObject Pluto = new Dwarf("Pluto", 5913520, 1.188, 90550, 6.39, Colors.SandyBrown, Sun);
												solarSystem.Add(Pluto);
												// Moons of 
												// Earth
												SpaceObject Luna = new Moon("Luna", 384, 1.74, 27.32, 27.32, Colors.White, Earth);
												solarSystem.Add(Luna);
												// Mars
												SpaceObject Phobos = new Moon("Phobos", 9.380, 0.011267, 0.3189, 0.31875, Colors.DarkGray, Mars);
												SpaceObject Deimos = new Moon("Deimos", 23.460, 0.006, 1.26, 1.26, Colors.Sienna, Mars);
												solarSystem.Add(Phobos); solarSystem.Add(Deimos);
												// Jupiter
												SpaceObject Metis = new Moon("Metis", 128, 0.0215, 0.29, 0.29, Colors.DarkGray, Jupiter);
												SpaceObject Adrastea = new Moon("Adrastea", 129, 0.008, 0.3, 0.3, Colors.DarkGray, Jupiter);
												SpaceObject Amalthea = new Moon("Amalthea", 181, 0.167, 0.5, 0.5, Colors.Firebrick, Jupiter);
												SpaceObject Thebe = new Moon("Thebe", 222, 0.0049, 0.68, 0.68, Colors.DarkGray, Jupiter);
												SpaceObject Io = new Moon("Io", 422, 1.8216, 1.77, 1.77, Colors.Orange, Jupiter);
												SpaceObject Europa = new Moon("Europa", 671, 1.560, 3.55, 3.55, Colors.White, Jupiter);
												SpaceObject Ganymede = new Moon("Ganymede", 1070, 2.631, 7.16, 7.16, Colors.WhiteSmoke, Jupiter);
												SpaceObject Callisto = new Moon("Callisto", 1883, 2.410, 16.69, 16.69, Colors.MidnightBlue, Jupiter);
												SpaceObject Leda = new Moon("Leda", 11094, 0.010, 238.7, 0, Colors.LightGray, Jupiter);
												SpaceObject Himalia = new Moon("Himalia", 11470, 0.085, 251.4, 0, Colors.Red, Jupiter);
												SpaceObject Lysithea = new Moon("Lysithea", 11717, 0.018, 259, 0, Colors.DarkGray, Jupiter);
												SpaceObject Elara = new Moon("Elara", 11737, 0.043, 259.6, 0, Colors.DarkGray, Jupiter);
												SpaceObject Ananke = new Moon("Ananke", 21280, 0.014, 629, 0, Colors.DarkGray, Jupiter);
												SpaceObject Carme = new Moon("Carme", 22610, 0.023, 692, 0, Colors.DarkGray, Jupiter);
												SpaceObject Pasiphae = new Moon("Pasiphae", 23587, 0.030, 735, 0, Colors.DarkGray, Jupiter);
												SpaceObject Sinope = new Moon("Sinope", 23940, 0.038, 758, 0, Colors.DarkGray, Jupiter);
												solarSystem.Add(Metis); solarSystem.Add(Adrastea); solarSystem.Add(Amalthea); solarSystem.Add(Thebe); solarSystem.Add(Io);
												solarSystem.Add(Europa); solarSystem.Add(Ganymede); solarSystem.Add(Callisto); solarSystem.Add(Leda); solarSystem.Add(Himalia);
												solarSystem.Add(Lysithea); solarSystem.Add(Elara); solarSystem.Add(Ananke); solarSystem.Add(Carme); solarSystem.Add(Pasiphae);
												solarSystem.Add(Sinope);
												// Saturn
												SpaceObject Pan = new Moon("Pan", 133, 0.014, 0.575, 0, Colors.DarkGray, Saturn);
												SpaceObject Atlas = new Moon("Atlas", 137, 0.020, 0.602, 0, Colors.DarkGray, Saturn);
												SpaceObject Prometheus = new Moon("Prometheus", 139, 0.086, 0.613, 0, Colors.DarkGray, Saturn);
												SpaceObject Pandora = new Moon("Pandora", 141, 0.081, 0.629, 0, Colors.DarkGray, Saturn);
												SpaceObject Epimetheus = new Moon("Epimetheus", 151, 0.069, 0.649, 0, Colors.DarkGray, Saturn);
												SpaceObject Janus = new Moon("Janus", 151, 0.179, 0.694, 0, Colors.DarkGray, Saturn);
												SpaceObject Mimas = new Moon("Mimas", 185, 0.198, 0.942, 0.942, Colors.DarkGray, Saturn);
												SpaceObject Enceladus = new Moon("Enceladus", 238, 0.252, 1.37, 1.37, Colors.LightGray, Saturn);
												SpaceObject Tethys = new Moon("Tethys", 294, 0.531, 1.89, 1.89, Colors.DarkGray, Saturn);
												SpaceObject Telesto = new Moon("Telesto", 294, 0.024, 1.89, 0, Colors.DarkGray, Saturn);
												SpaceObject Calypso = new Moon("Calypso", 294, 0.021, 1.89, 0, Colors.DarkGray, Saturn);
												SpaceObject Dione = new Moon("Dione", 377, 0.561, 2.74, 2.74, Colors.DarkGray, Saturn);
												SpaceObject Helene = new Moon("Helene", 377, 0.032, 2.74, 0, Colors.DarkGray, Saturn);
												SpaceObject Rhea = new Moon("Rhea", 527, 0.764, 4.52, 4.52, Colors.DarkGray, Saturn);
												SpaceObject Titan = new Moon("Titan", 1222, 2.57475, 15.95, 16, Colors.Orange, Saturn);
												SpaceObject Hyperion = new Moon("Hyperion", 1481, 0.135, 21.28, 0, Colors.Firebrick, Saturn);
												SpaceObject Lapetus = new Moon("Lapetus", 3560, 0.735, 79.32, 79.32, Colors.MidnightBlue, Saturn);
												SpaceObject Phoebe = new Moon("Phoebe", 12952, 0.106, 550.48, 0, Colors.Sienna, Saturn);
												solarSystem.Add(Pan); solarSystem.Add(Atlas); solarSystem.Add(Prometheus); solarSystem.Add(Pandora); solarSystem.Add(Epimetheus);
												solarSystem.Add(Janus); solarSystem.Add(Mimas); solarSystem.Add(Enceladus); solarSystem.Add(Tethys); solarSystem.Add(Telesto);
												solarSystem.Add(Calypso); solarSystem.Add(Dione); solarSystem.Add(Helene); solarSystem.Add(Rhea); solarSystem.Add(Titan);
												solarSystem.Add(Hyperion); solarSystem.Add(Lapetus); solarSystem.Add(Phoebe);
												// Uranus
												SpaceObject Cordelia = new Moon("Cordelia", 49, 0.020, 0.335, 0, Colors.DarkGray, Uranus);
												SpaceObject Ophelia = new Moon("Ophelia", 53, 0.021, 0.376, 0, Colors.DarkGray, Uranus);
												SpaceObject Bianca = new Moon("Bianca", 59, 0.027, 0.435, 0, Colors.DarkGray, Uranus);
												SpaceObject Cressida = new Moon("Cressida", 61, 0.041, 0.463, 0, Colors.DarkGray, Uranus);
												SpaceObject Desdemona = new Moon("Desdemona", 62, 0.032, 0.474, 0, Colors.DarkGray, Uranus);
												SpaceObject Juliet = new Moon("Juliet", 64, 0.053, 0.493, 0, Colors.DarkGray, Uranus);
												SpaceObject Portia = new Moon("Portia", 66, 0.067, 0.513, 0, Colors.DarkGray, Uranus);
												SpaceObject Rosalind = new Moon("Rosalind", 70, 0.036, 0.558, 0, Colors.DarkGray, Uranus);
												SpaceObject Belinda = new Moon("Belinda", 75, 0.045, 0.623, 0, Colors.DarkGray, Uranus);
												SpaceObject Puck = new Moon("Puck", 86, 0.081, 0.761, 0, Colors.DarkGray, Uranus);
												SpaceObject Miranda = new Moon("Miranda", 129, 0.240, 1.413, 1.413, Colors.DarkGray, Uranus);
												SpaceObject Ariel = new Moon("Ariel", 191, 0.580, 2.52, 2.52, Colors.DarkGray, Uranus);
												SpaceObject Umbriel = new Moon("Umbriel", 266, 0.590, 4.14, 4.14, Colors.DimGray, Uranus);
												SpaceObject Titania = new Moon("Titania", 436, 0.7884, 8.71, 8.71, Colors.LightGray, Uranus);
												SpaceObject Oberon = new Moon("Oberon", 583, 0.760, 13.46, 13.46, Colors.Firebrick, Uranus);
												SpaceObject Caliban = new Moon("Caliban", 7230, 0.072, 579.7, 0, Colors.DimGray, Uranus);
												SpaceObject Stephano = new Moon("Stephano", 8004, 0.016, 677.4, 0, Colors.DimGray, Uranus);
												SpaceObject Sycorax = new Moon("Sycorax", 12180, 0.150, 1287.1, 0, Colors.DimGray, Uranus);
												SpaceObject Prospero = new Moon("Prospero", 16600, 0.025, 1977.3, 0, Colors.DimGray, Uranus);
												SpaceObject Setebos = new Moon("Setebos", 17420, 0.024, 2235.5, 0, Colors.DimGray, Uranus);
												solarSystem.Add(Cordelia); solarSystem.Add(Ophelia); solarSystem.Add(Bianca); solarSystem.Add(Cressida);
												solarSystem.Add(Desdemona); solarSystem.Add(Juliet); solarSystem.Add(Portia); solarSystem.Add(Rosalind);
												solarSystem.Add(Belinda); solarSystem.Add(Puck); solarSystem.Add(Miranda); solarSystem.Add(Ariel); solarSystem.Add(Umbriel);
												solarSystem.Add(Titania); solarSystem.Add(Oberon); solarSystem.Add(Caliban); solarSystem.Add(Stephano); solarSystem.Add(Sycorax);
												solarSystem.Add(Prospero); solarSystem.Add(Setebos);
												// Neptune
												SpaceObject Naiad = new Moon("Naiad", 48, 0.033, 0.295, 0.295, Colors.DarkGray, Neptun);
												SpaceObject Thalassa = new Moon("Thalassa", 50, 0.041, 0.311, 0.311, Colors.DarkGray, Neptun);
												SpaceObject Despina = new Moon("Despina", 52, 0.075, 0.335, 0.335, Colors.DarkGray, Neptun);
												SpaceObject Galatea = new Moon("Galatea", 62, 0.088, 0.429, 0.429, Colors.DarkGray, Neptun);
												SpaceObject Larissa = new Moon("Larissa", 73, 0.097, 0.555, 0.555, Colors.DarkGray, Neptun);
												SpaceObject Proteus = new Moon("Proteus", 117, 0.210, 1.122, 1.122, Colors.DimGray, Neptun);
												SpaceObject Triton = new Moon("Triton", 355, 1.3534, 5.875, 5.877, Colors.Peru, Neptun);
												SpaceObject Nereid = new Moon("Nereid", 5510, 0.170, 360.13, 0, Colors.DarkGray, Neptun);
												solarSystem.Add(Naiad); solarSystem.Add(Thalassa); solarSystem.Add(Despina); solarSystem.Add(Galatea); solarSystem.Add(Larissa);
												solarSystem.Add(Proteus); solarSystem.Add(Triton); solarSystem.Add(Nereid);
												// Pluto 
												SpaceObject Charon = new Moon("Charon", 20, 0.6036, 6.4, 6.4, Colors.RosyBrown, Pluto);
												SpaceObject Styx = new Moon("Styx", 42, 0.010, 20.2, 20.2, Colors.Sienna, Pluto);
												SpaceObject Nix = new Moon("Nix", 49, 0.020, 24.9, 24.9, Colors.Sienna, Pluto);
												SpaceObject Kerberos = new Moon("Kerberos", 59, 0.005, 32.1, 32.1, Colors.Red, Pluto);
												SpaceObject Hydra = new Moon("Hydra", 65, 0.035, 38.2, 38.2, Colors.DarkGray, Pluto);
												solarSystem.Add(Charon); solarSystem.Add(Styx); solarSystem.Add(Nix); solarSystem.Add(Kerberos); solarSystem.Add(Hydra);
												// Asteroids
												SpaceObject Vesta = new Asteroid("Vesta", 353000, 0.2625, 1325.829, 0.223, Colors.DimGray, Sun);
												solarSystem.Add(Vesta);
												// Comets
												SpaceObject Halley = new Comet("Comet Halley", 2670000, 0.0055, 27794.95425, 2.2, Colors.DimGray, Sun);
												solarSystem.Add(Halley);

								}
				}
}
