using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpaceSim {
    public class SpaceObject {
        protected String name;
        protected double orbitalRadius;
        protected double orbitalPeriod;
        protected double radius;
        protected double rotationalPeriod;
        protected string color;
        //
        protected SpaceObject orbits;
        public string Name { get => name; set => name = value; }
        public double OrbitalRadius { get => orbitalRadius; set => orbitalRadius = value; }
        public double OrbitalPeriod { get => orbitalPeriod; set => orbitalPeriod = value; }
        public double Radius { get => radius; set => radius = value; }
        public double RotationalPeriod { get => rotationalPeriod; set => rotationalPeriod = value; }
        public string Color { get => color; set => color = value; }
        public SpaceObject Orbits { get => orbits; set => orbits = value; }
        public SpaceObject(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits = null) {
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
        public Tuple<double, double> getPosition(double days, Boolean isFocused, double centerRad) {
            if (orbits == null || isFocused) {
                return Tuple.Create(0.0, 0.0);
            } else {
                double angle = (days / orbitalPeriod) * 2 * Math.PI;
                double X; double Y;
                if (this is Moon) { // deprecated?
                    X = (orbitalRadius * 10 + centerRad ) * Math.Cos(angle);
                    Y = (orbitalRadius * 10 + centerRad ) * Math.Sin(angle);
                } else { 
                    X = (orbitalRadius + centerRad) * Math.Cos(angle);
                    Y = (orbitalRadius + centerRad) * Math.Sin(angle);
                    }
																if (orbits.orbits != null) {
																				(double parentX, double parentY) = orbits.getPosition(days, false, centerRad);
																				X += parentX;
																				Y += parentY;
																}

																return Tuple.Create(X, Y);
												}
								}

				}

    public class Star : SpaceObject {
        public Star(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Star : ");
            base.Draw();
        }
    }
    public class Planet : SpaceObject {
        public Planet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Planet : ");
            base.Draw();
        }
    }
    public class Moon : SpaceObject {
        public Moon(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Moon : ");
            base.Draw();
        }
    }
    public class Comet : SpaceObject {
        public Comet(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() { 
            Console.Write("Comet : "); 
            base.Draw();
        } 
    }
    public class Asteroid : SpaceObject {
        public Asteroid(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Asteroid : ");

            base.Draw();
        }
    }
    public class AsteroidBelt : SpaceObject {
        public AsteroidBelt(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Asteroid belt : ");
            base.Draw();
        }
    }
    public class Dwarf : SpaceObject {
        public Dwarf(string name, double orbitalRadius, double radius, double orbitalPeriod, double rotationalPeriod, string color, SpaceObject orbits) : base(name, orbitalRadius, radius, orbitalPeriod, rotationalPeriod, color, orbits) { }
        public override void Draw() {
            Console.Write("Dwarf planet : ");
            base.Draw();
        }
    }
    public class Initialize {

        public List<SpaceObject> SolarSystem() { return new InitializeSolarSystem().solarSystem; }
    }

}
