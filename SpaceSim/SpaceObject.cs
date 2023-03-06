using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace SpaceSim {
    public class SpaceObject {
        protected String name;
        protected double orbitalRadius;
        protected double orbitalPeriod;
        protected double radius;
        protected double rotationalPeriod;
        protected Color color;
        //
        protected SpaceObject orbits;
        public string Name { get => name; set => name = value; }
        public double OrbitalRadius { get => orbitalRadius; set => orbitalRadius = value; }
        public double OrbitalPeriod { get => orbitalPeriod; set => orbitalPeriod = value; }
        public double Radius { get => radius; set => radius = value; }
        public double RotationalPeriod { get => rotationalPeriod; set => rotationalPeriod = value; }
        public Color Color { get => color; set => color = value; }
        public SpaceObject Orbits { get => orbits; set => orbits = value; }
        

        public SpaceObject(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits = null) {
            this.name = name;
            this.orbitalRadius = orbitalRadius;
            this.orbitalPeriod = orbitalPeriod;
            this.radius = radius;
            this.rotationalPeriod = rotationalPeriod;
            this.color = color;
            this.orbits = orbits;
        }
        public virtual void Draw() {
            string colorName = typeof(Colors).GetProperties().FirstOrDefault(p => (Color)p.GetValue(null) == color)?.Name;
            Console.Write($"Name: {name}\nOrbital radius: {orbitalRadius}'000 km\nOrbital period: {orbitalPeriod} day(s)\nRadius: {radius}'000 km\nRotational period: {rotationalPeriod} day(s)\nColor: {colorName}\nOrbits: ");
            if (orbits != null) { Console.Write($"{orbits.name} "); } else { Console.Write("N/A "); };
            Console.WriteLine();
        }
        public override string ToString() {
            string colorName = typeof(Colors).GetProperties().FirstOrDefault(p => (Color)p.GetValue(null) == color)?.Name;
            string retur = $"Name: {name}\nOrbital radius: {orbitalRadius}'000 km\nOrbital period: {orbitalPeriod} day(s)\nRadius: {radius}'000 km\nRotational period: {rotationalPeriod} day(s)\nColor: {colorName}\nOrbits: ";
            retur += orbits != null ? $"{orbits.name}" : "N/A";

            return retur;
        }
        public Tuple<double, double, double> getPosition(double days, Boolean isFocused, double centerRad) {
            if (orbits == null || isFocused) {
                return Tuple.Create(0.0, 0.0, 0.0);
            } else {
                double angle = (days / orbitalPeriod) * 2 * Math.PI;
                double x; double y; double distance;
                double dist;
                if (this is Moon) {
                    dist = (OrbitalRadius + centerRad);
                    x = dist * Math.Cos(angle);
                    y = dist * (Math.Sin(angle));
                } else {
                    dist = ((OrbitalRadius / 4000)) + centerRad / 2;
                    x = dist * Math.Cos(angle);
                    y = dist * (Math.Sin(angle));
                }

                distance = Math.Sqrt(x * x + y * y);
                distance *= 2;
                return Tuple.Create(x, y, distance);
            }
        }

    public class Star : SpaceObject {
        public Star(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Star : ");
            base.Draw();
        }
    }
    public class Planet : SpaceObject {
        public Planet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Planet : ");
            base.Draw();
        }
    }
    public class Moon : SpaceObject {
        public Moon(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Moon : ");
            base.Draw();
        }
    }
    public class Comet : SpaceObject {
        public Comet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Comet : ");
            base.Draw();
        }
    }
    public class Asteroid : SpaceObject {
        public Asteroid(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Asteroid : ");

            base.Draw();
        }
    }
    public class AsteroidBelt : SpaceObject {
        public AsteroidBelt(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Asteroid belt : ");
            base.Draw();
        }
    }
    public class Dwarf : SpaceObject {
        public Dwarf(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, Color color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Dwarf planet : ");
            base.Draw();
        }
    }
        
    public class SolarSystem {
        List<SpaceObject> _solarSystem = new List<SpaceObject>();
        public List<SpaceObject> solarSystem{ get => _solarSystem; set => _solarSystem = value; }
        public SolarSystem() {
            SpaceObject Sun = new Star("Sun", 0, 696, 0, 27, Colors.OrangeRed, null);
            _solarSystem.Add(Sun);
            // Belts
            SpaceObject mainAst = new AsteroidBelt("The main belt", 418874, 0, 2027.0942, 0, Colors.SaddleBrown, Sun);
            _solarSystem.Add(mainAst);
            // Planets
            SpaceObject Mercury = new Planet("Mercury", 57910, 2.439, 87.97, 59, Colors.SlateGray, Sun);
            SpaceObject Venus = new Planet("Venus", 108200, 6.051, 224.7, 243, Colors.White, Sun);
            SpaceObject Earth = new Planet("Earth", 149600, 6.371, 365.26, 1, Colors.DodgerBlue, Sun);
            SpaceObject Mars = new Planet("Mars", 227940, 3.389, 686.98, 1.026, Colors.Firebrick, Sun);
            SpaceObject Jupiter = new Planet("Jupiter", 778330, 69.911, 4332.71, 0.417, Colors.Brown, Sun);
            SpaceObject Saturn = new Planet("Saturn", 1429400, 58.232, 10759.5, 0.446, Colors.SandyBrown, Sun);
            SpaceObject Uranus = new Planet("Uranus", 2870990, 25.362, 30685, 0.708, Colors.Teal, Sun);
            SpaceObject Neptun = new Planet("Neptune", 4504300, 24.622, 60190, 0.671, Colors.Cyan, Sun);
            _solarSystem.Add(Mercury); _solarSystem.Add(Venus); _solarSystem.Add(Earth); _solarSystem.Add(Mars);
            _solarSystem.Add(Jupiter); _solarSystem.Add(Saturn); _solarSystem.Add(Uranus); _solarSystem.Add(Neptun);

            // Dwarf
            SpaceObject Pluto = new Dwarf("Pluto", 5913520, 1.188, 90550, 6.39, Colors.SandyBrown, Sun);
            _solarSystem.Add(Pluto);
            // Moons of 
            // Earth
            SpaceObject Luna = new Moon("Luna", 384, 1.74, 27.32, 27.32, Colors.White, Earth);
            _solarSystem.Add(Luna);
            // Mars
            SpaceObject Phobos = new Moon("Phobos", 9.380, 0.011267, 0.3189, 0.31875, Colors.DarkGray, Mars);
            SpaceObject Deimos = new Moon("Deimos", 23.460, 0.006, 1.26, 1.26, Colors.Sienna, Mars);
            _solarSystem.Add(Phobos); _solarSystem.Add(Deimos);
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
            _solarSystem.Add(Metis); _solarSystem.Add(Adrastea); _solarSystem.Add(Amalthea); _solarSystem.Add(Thebe); _solarSystem.Add(Io);
            _solarSystem.Add(Europa); _solarSystem.Add(Ganymede); _solarSystem.Add(Callisto); _solarSystem.Add(Leda); _solarSystem.Add(Himalia);
            _solarSystem.Add(Lysithea); _solarSystem.Add(Elara); _solarSystem.Add(Ananke); _solarSystem.Add(Carme); _solarSystem.Add(Pasiphae);
            _solarSystem.Add(Sinope);
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
            _solarSystem.Add(Pan); _solarSystem.Add(Atlas); _solarSystem.Add(Prometheus); _solarSystem.Add(Pandora); _solarSystem.Add(Epimetheus);
            _solarSystem.Add(Janus); _solarSystem.Add(Mimas); _solarSystem.Add(Enceladus); _solarSystem.Add(Tethys); _solarSystem.Add(Telesto);
            _solarSystem.Add(Calypso); _solarSystem.Add(Dione); _solarSystem.Add(Helene); _solarSystem.Add(Rhea); _solarSystem.Add(Titan);
            _solarSystem.Add(Hyperion); _solarSystem.Add(Lapetus); _solarSystem.Add(Phoebe);
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
            _solarSystem.Add(Cordelia); _solarSystem.Add(Ophelia); _solarSystem.Add(Bianca); _solarSystem.Add(Cressida);
            _solarSystem.Add(Desdemona); _solarSystem.Add(Juliet); _solarSystem.Add(Portia); _solarSystem.Add(Rosalind);
            _solarSystem.Add(Belinda); _solarSystem.Add(Puck); _solarSystem.Add(Miranda); _solarSystem.Add(Ariel); _solarSystem.Add(Umbriel);
            _solarSystem.Add(Titania); _solarSystem.Add(Oberon); _solarSystem.Add(Caliban); _solarSystem.Add(Stephano); _solarSystem.Add(Sycorax);
            _solarSystem.Add(Prospero); _solarSystem.Add(Setebos);
            // Neptune
            SpaceObject Naiad = new Moon("Naiad", 48, 0.033, 0.295, 0.295, Colors.DarkGray, Neptun);
            SpaceObject Thalassa = new Moon("Thalassa", 50, 0.041, 0.311, 0.311, Colors.DarkGray, Neptun);
            SpaceObject Despina = new Moon("Despina", 52, 0.075, 0.335, 0.335, Colors.DarkGray, Neptun);
            SpaceObject Galatea = new Moon("Galatea", 62, 0.088, 0.429, 0.429, Colors.DarkGray, Neptun);
            SpaceObject Larissa = new Moon("Larissa", 73, 0.097, 0.555, 0.555, Colors.DarkGray, Neptun);
            SpaceObject Proteus = new Moon("Proteus", 117, 0.210, 1.122, 1.122, Colors.DimGray, Neptun);
            SpaceObject Triton = new Moon("Triton", 355, 1.3534, 5.875, 5.877, Colors.Peru, Neptun);
            SpaceObject Nereid = new Moon("Nereid", 5510, 0.170, 360.13, 0, Colors.DarkGray, Neptun);
            _solarSystem.Add(Naiad); _solarSystem.Add(Thalassa); _solarSystem.Add(Despina); _solarSystem.Add(Galatea); _solarSystem.Add(Larissa);
            _solarSystem.Add(Proteus); _solarSystem.Add(Triton); _solarSystem.Add(Nereid);
            // Pluto 
            SpaceObject Charon = new Moon("Charon", 20, 0.6036, 6.4, 6.4, Colors.RosyBrown, Pluto);
            SpaceObject Styx = new Moon("Styx", 42, 0.010, 20.2, 20.2, Colors.Sienna, Pluto);
            SpaceObject Nix = new Moon("Nix", 49, 0.020, 24.9, 24.9, Colors.Sienna, Pluto);
            SpaceObject Kerberos = new Moon("Kerberos", 59, 0.005, 32.1, 32.1, Colors.Red, Pluto);
            SpaceObject Hydra = new Moon("Hydra", 65, 0.035, 38.2, 38.2, Colors.DarkGray, Pluto);
            _solarSystem.Add(Charon); _solarSystem.Add(Styx); _solarSystem.Add(Nix); _solarSystem.Add(Kerberos); _solarSystem.Add(Hydra);
            // Asteroids
            SpaceObject Vesta = new Asteroid("Vesta", 353000, 0.2625, 1325.829, 0.223, Colors.DimGray, Sun);
            _solarSystem.Add(Vesta);
            // Comets
            SpaceObject Halley = new Comet("Comet Halley", 2670000, 0.0055, 27794.95425, 2.2, Colors.DimGray, Sun);
            _solarSystem.Add(Halley);
        }
    }

}
