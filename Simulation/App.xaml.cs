﻿using SpaceSim;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace Simulation {
				/// <summary>
				/// Interaction logic for App.xaml
				/// </summary>
				public partial class App : Application {
								public List<SpaceSim.SpaceObject> solarSystem = new List<SpaceObject>();
								public App() {
												
												SpaceObject Sun = new Star("Sun", 0, 6963500, 0, 27, "White", null);
												solarSystem.Add(Sun);
												// Planets
												SpaceObject Mercury = new Planet("Mercury", 57910, 2439.7, 87.97, 59, "Slate gray", Sun);
												SpaceObject Venus = new Planet("Venus", 108200, 6051.8, 224.7, 243, "White", Sun);
												SpaceObject Earth = new Planet("Earth", 149600, 6378, 365.26, 1, "Blue", Sun);
												SpaceObject Mars = new Planet("Mars", 227940, 3389.5, 686.98, 1.026, "Red", Sun);
												SpaceObject Jupiter = new Planet("Jupiter", 778330, 69910, 4332.71, 0.417, "Brown/Red", Sun);
												SpaceObject Saturn = new Planet("Saturn", 1429400, 58230, 10759.5, 0.446, "Yellow/Brown", Sun);
												SpaceObject Uranus = new Planet("Uranus", 2870990, 25362, 30685, 0.708, "Blue/Green", Sun);
												SpaceObject Neptun = new Planet("Neptun", 4504300, 24622, 60190, 0.671, "Blue", Sun);
												solarSystem.Add(Mercury); solarSystem.Add(Venus); solarSystem.Add(Earth); solarSystem.Add(Mars);
												solarSystem.Add(Jupiter); solarSystem.Add(Saturn); solarSystem.Add(Uranus); solarSystem.Add(Neptun);

												// Dwarf
												SpaceObject Pluto = new Dwarf("Pluto", 5913520, 1188, 90550, 6.39, "Red/Brown", Sun);
												solarSystem.Add(Pluto);
												// Moons of 
												// Earth
												SpaceObject Luna = new Moon("Luna", 384, 27.32, 1737, 29.53, "White", Earth);
												solarSystem.Add(Luna);
												// Mars
												SpaceObject Phobos = new Moon("Phobos", 9, 11.1, 0.3189, 0.31875, "Gray", Mars);
												SpaceObject Deimos = new Moon("Deimos", 23, 6.2, 1.26, 1.26, "Reddish", Mars);
												solarSystem.Add(Phobos); solarSystem.Add(Deimos);
												// Jupiter
												SpaceObject Metis = new Moon("Metis", 128, 21.5, 0.29, 0.29, "Grey", Jupiter);
												SpaceObject Adrastea = new Moon("Adrastea", 129, 8, 0.3, 0.3, "Grey", Jupiter);
												SpaceObject Amalthea = new Moon("Amalthea", 181, 167, 0.5, 0.5, "Red/Brown", Jupiter);
												SpaceObject Thebe = new Moon("Thebe", 222, 49, 0.68, 0.68, "Grey", Jupiter);
												SpaceObject Io = new Moon("Io", 422, 1821.6, 1.77, 1.77, "Yellow/Red", Jupiter);
												SpaceObject Europa = new Moon("Europa", 671, 1560, 3.55, 3.55, "White", Jupiter);
												SpaceObject Ganymede = new Moon("Ganymede", 1070, 2631, 7.16, 7.16, "White", Jupiter);
												SpaceObject Callisto = new Moon("Callisto", 1883, 2410, 16.69, 16.69, "Dark", Jupiter);
												SpaceObject Leda = new Moon("Leda", 11094, 10, 238.7, 0, "Light Grey", Jupiter);
												SpaceObject Himalia = new Moon("Himalia", 11470, 85, 251.4, 0, "Red", Jupiter);
												SpaceObject Lysithea = new Moon("Lysithea", 11717, 18, 259, 0, "Grey", Jupiter);
												SpaceObject Elara = new Moon("Elara", 11737, 43, 259.6, 0, "Grey", Jupiter);
												SpaceObject Ananke = new Moon("Ananke", 21280, 14, 629, 0, "Grey", Jupiter);
												SpaceObject Carme = new Moon("Carme", 22610, 23, 692, 0, "Grey", Jupiter);
												SpaceObject Pasiphae = new Moon("Pasiphae", 23587, 30, 735, 0, "Grey", Jupiter);
												SpaceObject Sinope = new Moon("Sinope", 23940, 38, 758, 0, "Grey", Jupiter);
												solarSystem.Add(Metis); solarSystem.Add(Adrastea); solarSystem.Add(Amalthea); solarSystem.Add(Thebe); solarSystem.Add(Io);
												solarSystem.Add(Europa); solarSystem.Add(Ganymede); solarSystem.Add(Callisto); solarSystem.Add(Leda); solarSystem.Add(Himalia);
												solarSystem.Add(Lysithea); solarSystem.Add(Elara); solarSystem.Add(Ananke); solarSystem.Add(Carme); solarSystem.Add(Pasiphae);
												solarSystem.Add(Sinope);
												// Saturn
												SpaceObject Pan = new Moon("Pan", 133, 14, 0.575, 0, "Grey", Saturn);
												SpaceObject Atlas = new Moon("Atlas", 137, 20, 0.602, 0, "Grey", Saturn);
												SpaceObject Prometheus = new Moon("Prometheus", 139, 86, 0.613, 0, "Grey", Saturn);
												SpaceObject Pandora = new Moon("Pandora", 141, 81, 0.629, 0, "Grey", Saturn);
												SpaceObject Epimetheus = new Moon("Epimetheus", 151, 69, 0.649, 0, "Grey", Saturn);
												SpaceObject Janus = new Moon("Janus", 151, 179, 0.694, 0, "Grey", Saturn);
												SpaceObject Mimas = new Moon("Mimas", 185, 198, 0.942, 0.942, "Grey", Saturn);
												SpaceObject Enceladus = new Moon("Enceladus", 238, 252, 1.37, 1.37, "White", Saturn);
												SpaceObject Tethys = new Moon("Tethys", 294, 531, 1.89, 1.89, "Grey", Saturn);
												SpaceObject Telesto = new Moon("Telesto", 294, 24, 1.89, 0, "Grey", Saturn);
												SpaceObject Calypso = new Moon("Calypso", 294, 21, 1.89, 0, "Grey", Saturn);
												SpaceObject Dione = new Moon("Dione", 377, 561, 2.74, 2.74, "Grey", Saturn);
												SpaceObject Helene = new Moon("Helene", 377, 32, 2.74, 0, "Grey", Saturn);
												SpaceObject Rhea = new Moon("Rhea", 527, 764, 4.52, 4.52, "Grey", Saturn);
												SpaceObject Titan = new Moon("Titan", 1222, 2574.75, 15.95, 16, "Orange", Saturn);
												SpaceObject Hyperion = new Moon("Hyperion", 1481, 135, 21.28, 0, "Red/Brown", Saturn);
												SpaceObject Lapetus = new Moon("Lapetus", 3560, 735, 79.32, 79.32, "Dark", Saturn);
												SpaceObject Phoebe = new Moon("Phoebe", 12952, 106, 550.48, 0, "Red/Brown", Saturn);
												solarSystem.Add(Pan); solarSystem.Add(Atlas); solarSystem.Add(Prometheus); solarSystem.Add(Pandora); solarSystem.Add(Epimetheus);
												solarSystem.Add(Janus); solarSystem.Add(Mimas); solarSystem.Add(Enceladus); solarSystem.Add(Tethys); solarSystem.Add(Telesto);
												solarSystem.Add(Calypso); solarSystem.Add(Dione); solarSystem.Add(Helene); solarSystem.Add(Rhea); solarSystem.Add(Titan);
												solarSystem.Add(Hyperion); solarSystem.Add(Lapetus); solarSystem.Add(Phoebe);
												// Uranus
												SpaceObject Cordelia = new Moon("Cordelia", 49, 20, 0.335, 0, "Grey", Uranus);
												SpaceObject Ophelia = new Moon("Ophelia", 53, 21, 0.376, 0, "Grey", Uranus);
												SpaceObject Bianca = new Moon("Bianca", 59, 27, 0.435, 0, "Grey", Uranus);
												SpaceObject Cressida = new Moon("Cressida", 61, 41, 0.463, 0, "Grey", Uranus);
												SpaceObject Desdemona = new Moon("Desdemona", 62, 32, 0.474, 0, "Grey", Uranus);
												SpaceObject Juliet = new Moon("Juliet", 64, 53, 0.493, 0, "Grey", Uranus);
												SpaceObject Portia = new Moon("Portia", 66, 67, 0.513, 0, "Grey", Uranus);
												SpaceObject Rosalind = new Moon("Rosalind", 70, 36, 0.558, 0, "Grey", Uranus);
												SpaceObject Belinda = new Moon("Belinda", 75, 45, 0.623, 0, "Grey", Uranus);
												SpaceObject Puck = new Moon("Puck", 86, 81, 0.761, 0, "Grey", Uranus);
												SpaceObject Miranda = new Moon("Miranda", 129, 240, 1.413, 1.413, "Grey", Uranus);
												SpaceObject Ariel = new Moon("Ariel", 191, 580, 2.52, 2.52, "Grey", Uranus);
												SpaceObject Umbriel = new Moon("Umbriel", 266, 590, 4.14, 4.14, "Dark Grey", Uranus);
												SpaceObject Titania = new Moon("Titania", 436, 788.4, 8.71, 8.71, "Neutral Grey", Uranus);
												SpaceObject Oberon = new Moon("Oberon", 583, 760, 13.46, 13.46, "Red/Brown", Uranus);
												SpaceObject Caliban = new Moon("Caliban", 7230, 72, 579.7, 0, "Dark Grey", Uranus);
												SpaceObject Stephano = new Moon("Stephano", 8004, 16, 677.4, 0, "Dark Grey", Uranus);
												SpaceObject Sycorax = new Moon("Sycorax", 12180, 150, 1287.1, 0, "Dark Grey", Uranus);
												SpaceObject Prospero = new Moon("Prospero", 166000, 25, 1977.3, 0, "Dark Grey", Uranus);
												SpaceObject Setebos = new Moon("Setebos", 17420, 24, 2235.5, 0, "Dark Grey", Uranus);
												solarSystem.Add(Cordelia); solarSystem.Add(Ophelia); solarSystem.Add(Bianca); solarSystem.Add(Cressida);
												solarSystem.Add(Desdemona); solarSystem.Add(Juliet); solarSystem.Add(Portia); solarSystem.Add(Rosalind);
												solarSystem.Add(Belinda); solarSystem.Add(Puck); solarSystem.Add(Miranda); solarSystem.Add(Ariel); solarSystem.Add(Umbriel);
												solarSystem.Add(Titania); solarSystem.Add(Oberon); solarSystem.Add(Caliban); solarSystem.Add(Stephano); solarSystem.Add(Sycorax);
												solarSystem.Add(Prospero); solarSystem.Add(Setebos);
												// Neptune
												SpaceObject Naiad = new Moon("Naiad", 48, 33, 0.295, 0.295, "Grey", Neptun);
												SpaceObject Thalassa = new Moon("Thalassa", 50, 41, 0.311, 0.311, "Grey", Neptun);
												SpaceObject Despina = new Moon("Despina", 52, 75, 0.335, 0.335, "Grey", Neptun);
												SpaceObject Galatea = new Moon("Galatea", 62, 88, 0.429, 0.429, "Grey", Neptun);
												SpaceObject Larissa = new Moon("Larissa", 73, 97, 0.555, 0.555, "Grey", Neptun);
												SpaceObject Proteus = new Moon("Proteus", 117, 210, 1.122, 1.122, "Dark Grey", Neptun);
												SpaceObject Triton = new Moon("Triton", 355, 1353.4, 5.875, 5.877, "Reddish", Neptun);
												SpaceObject Nereid = new Moon("Nereid", 5510, 170, 360.13, 0, "Grey", Neptun);
												solarSystem.Add(Naiad); solarSystem.Add(Thalassa); solarSystem.Add(Despina); solarSystem.Add(Galatea); solarSystem.Add(Larissa);
												solarSystem.Add(Proteus); solarSystem.Add(Triton); solarSystem.Add(Nereid);
												// Pluto 
												SpaceObject Charon = new Moon("Charon", 20, 603.6, 6.4, 6.4, "Grey/Brown", Pluto);
												SpaceObject Styx = new Moon("Styx", 42, 10, 20.2, 20.2, "Red", Pluto);
												SpaceObject Nix = new Moon("Nix", 49, 20, 24.9, 24.9, "Red", Pluto);
												SpaceObject Kerberos = new Moon("Kerberos", 59, 5, 32.1, 32.1, "Red", Pluto);
												SpaceObject Hydra = new Moon("Hydra", 65, 35, 38.2, 38.2, "Grey", Pluto);
												solarSystem.Add(Charon); solarSystem.Add(Styx); solarSystem.Add(Nix); solarSystem.Add(Kerberos); solarSystem.Add(Hydra);
												// Asteroids
												SpaceObject Vesta = new Asteroid("Vesta", 353000, 262.5, 1325.829, 0.223, "Dark gray", Sun);
												solarSystem.Add(Vesta);
												// Comets
												SpaceObject Halley = new Comet("Comet Halley", 2670000, 7.5, 27794.95425, 2.2, "Dark Grey", Sun);
												solarSystem.Add(Halley);
												// Belts
												SpaceObject mainAst = new AsteroidBelt("The main belt", 418874, 1, 2027.0942, 0, "Red/Brown", Sun);
												solarSystem.Add(mainAst);
								}
				}
}