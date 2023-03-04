using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
            this.rotationalPeriod= rotationalPeriod;
            this.color = color;
            this.orbits = orbits;
        }
        public virtual void Draw() {
            Console.Write($"Name: {name}, Orbital radius: {orbitalRadius}, Orbital period: {orbitalPeriod}, Radius: {radius}, Rotational period: {rotationalPeriod}, color: {color}, Orbits: ");
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
    public class Initialize {

        public List<SpaceObject> SolarSystem() { return new InitializeSolarSystem().solarSystem; }
    }

}
