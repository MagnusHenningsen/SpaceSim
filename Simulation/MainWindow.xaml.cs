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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Simulation {
				/// <summary>
				/// Interaction logic for MainWindow.xaml
				/// </summary>
				public partial class MainWindow : Window {
								List<SpaceSim.SpaceObject> FocusedObjects = new List<SpaceObject>();
								List<SpaceSim.SpaceObject> solarSystem = new List<SpaceObject>();
								double days = 0;
								private Point _start;
								private double _zoom = 1.0;
								private Point _origin = new Point(0, 0);
								public MainWindow() {
												InitializeComponent();

												InitSolarSystem();
												solarSystem.ForEach(obj => {
																if (obj is Star || obj is Planet || obj is Dwarf || obj is Asteroid) {
																				FocusedObjects.Add(obj);
																}
												});
												List<SpaceObject> GreaterObjects = FocusedObjects;
												Dropdown.ItemsSource = FocusedObjects;
												Dropdown.SelectedIndex = 0;
												Dropdown.DisplayMemberPath = "Name";
												Dropdown.SelectionChanged += dropdownChange;
												DispatcherTimer timer = new DispatcherTimer();
												timer.Interval = TimeSpan.FromMilliseconds(1); 
												timer.Tick += Timer_Tick;
												timer.Start(); 
												Paint();
								}
								public void Timer_Tick(object sender, EventArgs e) {
												days += 0.5;
												InvalidateVisual();
									
												Paint();
								}

								private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e) {
												var currentScale = (canvas.LayoutTransform as ScaleTransform)?.ScaleX ?? 1.0;
												var newScale = currentScale + e.Delta / 1000.0;
												newScale = Math.Min(Math.Max(newScale, 0.5), 2.0);
												canvas.LayoutTransform = new ScaleTransform(newScale, newScale);
								}
								public double centerRad = 0;
								private void Paint() {
												int counter = 0;
												canvas.Children.Clear();
												foreach (SpaceObject obj in FocusedObjects) {
																if (counter == 0) {
																				Ellipse ellipse = new Ellipse();
																				SolidColorBrush br = new SolidColorBrush(Colors.Yellow);
																				ellipse.Width = Math.Max(obj.Radius / 10, 50); ellipse.Height = Math.Max(50, obj.Radius / 10);
																				ellipse.Fill = br;
																				ellipse.Stroke = new SolidColorBrush(Colors.White);
																				ellipse.StrokeThickness = 0.5;
																				centerRad = obj.Radius  * 150;
																				Tuple<double, double> point = obj.getPosition(days, true, 0);
																				Canvas.SetLeft(ellipse, this.ActualWidth / 2 - ellipse.Width / 2);
																				Canvas.SetTop(ellipse, this.ActualHeight / 2 - ellipse.Height / 2);
																				canvas.Children.Add(ellipse);

																				
																} else {
																				Ellipse ellipse = new Ellipse();
																				SolidColorBrush br = new SolidColorBrush(Colors.Red);
																				Tuple<double, double> point;

																								ellipse.Width = Math.Max(5, obj.Radius); ellipse.Height = Math.Max(5, obj.Radius);
																								ellipse.Fill = br;
																								ellipse.Stroke = new SolidColorBrush(Colors.White);
																								ellipse.StrokeThickness = 0.5;
																								point = obj.getPosition(days, false, centerRad);
																				
																				double right = this.ActualWidth / 2 - ellipse.Width / 2 + point.Item1 / 4000;
																				double bottom = this.ActualHeight / 2 - ellipse.Height / 2 + point.Item2 / 4000;
																				Canvas.SetLeft(ellipse, right);
																				Canvas.SetTop(ellipse, bottom);
																				canvas.Children.Add(ellipse);
																				if (obj is Planet || obj is Dwarf ) {
																								Ellipse orbit = new Ellipse();
																								orbit.Width = (obj.OrbitalRadius + centerRad) / 2000;
																								orbit.Height = orbit.Width;
																								orbit.Stroke = new SolidColorBrush(Colors.White);
																								orbit.StrokeThickness = 0.5;
																								Canvas.SetLeft(orbit, this.ActualWidth / 2 - orbit.Width / 2);
																								Canvas.SetTop(orbit, this.ActualHeight / 2 - orbit.Height / 2);
																								canvas.Children.Add(orbit);
																				}
																				TextBlock textBlock = new TextBlock();
																				textBlock.Text = obj.Name;
																				textBlock.Foreground = Brushes.White;
																				textBlock.TextAlignment = TextAlignment.Left;
																				Canvas.SetLeft(textBlock, Canvas.GetLeft(ellipse) - (textBlock.ActualWidth / 2) + (ellipse.ActualWidth / 2));
																				Canvas.SetTop(textBlock, Canvas.GetTop(ellipse) - textBlock.ActualHeight - 25);
																				canvas.Children.Add(textBlock);
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
												SpaceObject Sun = new Star("Sun", 0, 696, 0, 27, "White", null);
												solarSystem.Add(Sun);
												// Planets
												SpaceObject Mercury = new Planet("Mercury", 57910, 2.439, 87.97, 59, "Slate gray", Sun);
												SpaceObject Venus = new Planet("Venus", 108200, 6.051, 224.7, 243, "White", Sun);
												SpaceObject Earth = new Planet("Earth", 149600, 6.371, 365.26, 1, "Blue", Sun);
												SpaceObject Mars = new Planet("Mars", 227940, 3.389, 686.98, 1.026, "Red", Sun);
												SpaceObject Jupiter = new Planet("Jupiter", 778330, 69.911, 4332.71, 0.417, "Brown/Red", Sun);
												SpaceObject Saturn = new Planet("Saturn", 1429400, 58.232, 10759.5, 0.446, "Yellow/Brown", Sun);
												SpaceObject Uranus = new Planet("Uranus", 2870990, 25.362, 30685, 0.708, "Blue/Green", Sun);
												SpaceObject Neptun = new Planet("Neptune", 4504300, 24.622, 60190, 0.671, "Blue", Sun);
												solarSystem.Add(Mercury); solarSystem.Add(Venus); solarSystem.Add(Earth); solarSystem.Add(Mars);
												solarSystem.Add(Jupiter); solarSystem.Add(Saturn); solarSystem.Add(Uranus); solarSystem.Add(Neptun);

												// Dwarf
												SpaceObject Pluto = new Dwarf("Pluto", 5913520, 1.188, 90550, 6.39, "Red/Brown", Sun);
												solarSystem.Add(Pluto);
												// Moons of 
												// Earth
												SpaceObject Luna = new Moon("Luna", 384, 1.74, 1737, 29.53, "White", Earth);
												solarSystem.Add(Luna);
												// Mars
												SpaceObject Phobos = new Moon("Phobos", 9, 0.011267, 0.3189, 0.31875, "Gray", Mars);
												SpaceObject Deimos = new Moon("Deimos", 23, 0.006, 1.26, 1.26, "Reddish", Mars);
												solarSystem.Add(Phobos); solarSystem.Add(Deimos);
												// Jupiter
												SpaceObject Metis = new Moon("Metis", 128, 0.0215, 0.29, 0.29, "Grey", Jupiter);
												SpaceObject Adrastea = new Moon("Adrastea", 129, 0.008, 0.3, 0.3, "Grey", Jupiter);
												SpaceObject Amalthea = new Moon("Amalthea", 181, 0.167, 0.5, 0.5, "Red/Brown", Jupiter);
												SpaceObject Thebe = new Moon("Thebe", 222, 0.0049, 0.68, 0.68, "Grey", Jupiter);
												SpaceObject Io = new Moon("Io", 422, 1.8216, 1.77, 1.77, "Yellow/Red", Jupiter);
												SpaceObject Europa = new Moon("Europa", 671, 1.560, 3.55, 3.55, "White", Jupiter);
												SpaceObject Ganymede = new Moon("Ganymede", 1070, 2.631, 7.16, 7.16, "White", Jupiter);
												SpaceObject Callisto = new Moon("Callisto", 1883, 2.410, 16.69, 16.69, "Dark", Jupiter);
												SpaceObject Leda = new Moon("Leda", 11094, 0.010, 238.7, 0, "Light Grey", Jupiter);
												SpaceObject Himalia = new Moon("Himalia", 11470, 0.085, 251.4, 0, "Red", Jupiter);
												SpaceObject Lysithea = new Moon("Lysithea", 11717, 0.018, 259, 0, "Grey", Jupiter);
												SpaceObject Elara = new Moon("Elara", 11737, 0.043, 259.6, 0, "Grey", Jupiter);
												SpaceObject Ananke = new Moon("Ananke", 21280, 0.014, 629, 0, "Grey", Jupiter);
												SpaceObject Carme = new Moon("Carme", 22610, 0.023, 692, 0, "Grey", Jupiter);
												SpaceObject Pasiphae = new Moon("Pasiphae", 23587, 0.030, 735, 0, "Grey", Jupiter);
												SpaceObject Sinope = new Moon("Sinope", 23940, 0.038, 758, 0, "Grey", Jupiter);
												solarSystem.Add(Metis); solarSystem.Add(Adrastea); solarSystem.Add(Amalthea); solarSystem.Add(Thebe); solarSystem.Add(Io);
												solarSystem.Add(Europa); solarSystem.Add(Ganymede); solarSystem.Add(Callisto); solarSystem.Add(Leda); solarSystem.Add(Himalia);
												solarSystem.Add(Lysithea); solarSystem.Add(Elara); solarSystem.Add(Ananke); solarSystem.Add(Carme); solarSystem.Add(Pasiphae);
												solarSystem.Add(Sinope);
												// Saturn
												SpaceObject Pan = new Moon("Pan", 133, 0.014, 0.575, 0, "Grey", Saturn);
												SpaceObject Atlas = new Moon("Atlas", 137, 0.020, 0.602, 0, "Grey", Saturn);
												SpaceObject Prometheus = new Moon("Prometheus", 139, 0.086, 0.613, 0, "Grey", Saturn);
												SpaceObject Pandora = new Moon("Pandora", 141, 0.081, 0.629, 0, "Grey", Saturn);
												SpaceObject Epimetheus = new Moon("Epimetheus", 151, 0.069, 0.649, 0, "Grey", Saturn);
												SpaceObject Janus = new Moon("Janus", 151, 0.179, 0.694, 0, "Grey", Saturn);
												SpaceObject Mimas = new Moon("Mimas", 185, 0.198, 0.942, 0.942, "Grey", Saturn);
												SpaceObject Enceladus = new Moon("Enceladus", 238, 0.252, 1.37, 1.37, "White", Saturn);
												SpaceObject Tethys = new Moon("Tethys", 294, 0.531, 1.89, 1.89, "Grey", Saturn);
												SpaceObject Telesto = new Moon("Telesto", 294, 0.024, 1.89, 0, "Grey", Saturn);
												SpaceObject Calypso = new Moon("Calypso", 294, 0.021, 1.89, 0, "Grey", Saturn);
												SpaceObject Dione = new Moon("Dione", 377, 0.561, 2.74, 2.74, "Grey", Saturn);
												SpaceObject Helene = new Moon("Helene", 377, 0.032, 2.74, 0, "Grey", Saturn);
												SpaceObject Rhea = new Moon("Rhea", 527, 0.764, 4.52, 4.52, "Grey", Saturn);
												SpaceObject Titan = new Moon("Titan", 1222, 2.57475, 15.95, 16, "Orange", Saturn);
												SpaceObject Hyperion = new Moon("Hyperion", 1481, 0.135, 21.28, 0, "Red/Brown", Saturn);
												SpaceObject Lapetus = new Moon("Lapetus", 3560, 0.735, 79.32, 79.32, "Dark", Saturn);
												SpaceObject Phoebe = new Moon("Phoebe", 12952, 0.106, 550.48, 0, "Red/Brown", Saturn);
												solarSystem.Add(Pan); solarSystem.Add(Atlas); solarSystem.Add(Prometheus); solarSystem.Add(Pandora); solarSystem.Add(Epimetheus);
												solarSystem.Add(Janus); solarSystem.Add(Mimas); solarSystem.Add(Enceladus); solarSystem.Add(Tethys); solarSystem.Add(Telesto);
												solarSystem.Add(Calypso); solarSystem.Add(Dione); solarSystem.Add(Helene); solarSystem.Add(Rhea); solarSystem.Add(Titan);
												solarSystem.Add(Hyperion); solarSystem.Add(Lapetus); solarSystem.Add(Phoebe);
												// Uranus
												SpaceObject Cordelia = new Moon("Cordelia", 49, 0.020, 0.335, 0, "Grey", Uranus);
												SpaceObject Ophelia = new Moon("Ophelia", 53, 0.021, 0.376, 0, "Grey", Uranus);
												SpaceObject Bianca = new Moon("Bianca", 59, 0.027, 0.435, 0, "Grey", Uranus);
												SpaceObject Cressida = new Moon("Cressida", 61, 0.041, 0.463, 0, "Grey", Uranus);
												SpaceObject Desdemona = new Moon("Desdemona", 62, 0.032, 0.474, 0, "Grey", Uranus);
												SpaceObject Juliet = new Moon("Juliet", 64, 0.053, 0.493, 0, "Grey", Uranus);
												SpaceObject Portia = new Moon("Portia", 66, 0.067, 0.513, 0, "Grey", Uranus);
												SpaceObject Rosalind = new Moon("Rosalind", 70, 0.036, 0.558, 0, "Grey", Uranus);
												SpaceObject Belinda = new Moon("Belinda", 75, 0.045, 0.623, 0, "Grey", Uranus);
												SpaceObject Puck = new Moon("Puck", 86, 0.081, 0.761, 0, "Grey", Uranus);
												SpaceObject Miranda = new Moon("Miranda", 129, 0.240, 1.413, 1.413, "Grey", Uranus);
												SpaceObject Ariel = new Moon("Ariel", 191, 0.580, 2.52, 2.52, "Grey", Uranus);
												SpaceObject Umbriel = new Moon("Umbriel", 266, 0.590, 4.14, 4.14, "Dark Grey", Uranus);
												SpaceObject Titania = new Moon("Titania", 436, 0.7884, 8.71, 8.71, "Neutral Grey", Uranus);
												SpaceObject Oberon = new Moon("Oberon", 583, 0.760, 13.46, 13.46, "Red/Brown", Uranus);
												SpaceObject Caliban = new Moon("Caliban", 7230, 0.072, 579.7, 0, "Dark Grey", Uranus);
												SpaceObject Stephano = new Moon("Stephano", 8004, 0.016, 677.4, 0, "Dark Grey", Uranus);
												SpaceObject Sycorax = new Moon("Sycorax", 12180, 0.150, 1287.1, 0, "Dark Grey", Uranus);
												SpaceObject Prospero = new Moon("Prospero", 166000, 0.025, 1977.3, 0, "Dark Grey", Uranus);
												SpaceObject Setebos = new Moon("Setebos", 17420, 0.024, 2235.5, 0, "Dark Grey", Uranus);
												solarSystem.Add(Cordelia); solarSystem.Add(Ophelia); solarSystem.Add(Bianca); solarSystem.Add(Cressida);
												solarSystem.Add(Desdemona); solarSystem.Add(Juliet); solarSystem.Add(Portia); solarSystem.Add(Rosalind);
												solarSystem.Add(Belinda); solarSystem.Add(Puck); solarSystem.Add(Miranda); solarSystem.Add(Ariel); solarSystem.Add(Umbriel);
												solarSystem.Add(Titania); solarSystem.Add(Oberon); solarSystem.Add(Caliban); solarSystem.Add(Stephano); solarSystem.Add(Sycorax);
												solarSystem.Add(Prospero); solarSystem.Add(Setebos);
												// Neptune
												SpaceObject Naiad = new Moon("Naiad", 48, 0.033, 0.295, 0.295, "Grey", Neptun);
												SpaceObject Thalassa = new Moon("Thalassa", 50, 0.041, 0.311, 0.311, "Grey", Neptun);
												SpaceObject Despina = new Moon("Despina", 52, 0.075, 0.335, 0.335, "Grey", Neptun);
												SpaceObject Galatea = new Moon("Galatea", 62, 0.088, 0.429, 0.429, "Grey", Neptun);
												SpaceObject Larissa = new Moon("Larissa", 73, 0.097, 0.555, 0.555, "Grey", Neptun);
												SpaceObject Proteus = new Moon("Proteus", 117, 0.210, 1.122, 1.122, "Dark Grey", Neptun);
												SpaceObject Triton = new Moon("Triton", 355, 1.3534, 5.875, 5.877, "Reddish", Neptun);
												SpaceObject Nereid = new Moon("Nereid", 5510, 0.170, 360.13, 0, "Grey", Neptun);
												solarSystem.Add(Naiad); solarSystem.Add(Thalassa); solarSystem.Add(Despina); solarSystem.Add(Galatea); solarSystem.Add(Larissa);
												solarSystem.Add(Proteus); solarSystem.Add(Triton); solarSystem.Add(Nereid);
												// Pluto 
												SpaceObject Charon = new Moon("Charon", 20, 0.6036, 6.4, 6.4, "Grey/Brown", Pluto);
												SpaceObject Styx = new Moon("Styx", 42, 0.010, 20.2, 20.2, "Red", Pluto);
												SpaceObject Nix = new Moon("Nix", 49, 0.020, 24.9, 24.9, "Red", Pluto);
												SpaceObject Kerberos = new Moon("Kerberos", 59, 0.005, 32.1, 32.1, "Red", Pluto);
												SpaceObject Hydra = new Moon("Hydra", 65, 0.035, 38.2, 38.2, "Grey", Pluto);
												solarSystem.Add(Charon); solarSystem.Add(Styx); solarSystem.Add(Nix); solarSystem.Add(Kerberos); solarSystem.Add(Hydra);
												// Asteroids
												SpaceObject Vesta = new Asteroid("Vesta", 353000, 0.2625, 1325.829, 0.223, "Dark gray", Sun);
												solarSystem.Add(Vesta);
												// Comets
												SpaceObject Halley = new Comet("Comet Halley", 2670000, 0.0055, 27794.95425, 2.2, "Dark Grey", Sun);
												solarSystem.Add(Halley);
												// Belts
												SpaceObject mainAst = new AsteroidBelt("The main belt", 418874, 0, 2027.0942, 0, "Red/Brown", Sun);
												solarSystem.Add(mainAst);
								}
				}
}
