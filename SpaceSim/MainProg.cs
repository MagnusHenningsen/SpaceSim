using SpaceSim;
using System;
using System.Collections.Generic;

class Astronomy {
    public static void Main() {
        SolarSystem sys = new SolarSystem();
        List<SpaceObject> solarSystem = sys.solarSystem;
        Boolean accepted = false;
        String inp;
        double days = 0;
        while (!accepted) {
            Console.Write("Time (days) since 0: ");
            inp = Console.ReadLine();
            if (double.TryParse(inp, out double val)) {
                days = val;
                accepted = true;
            } else {

            }
        }

        Boolean found = false;
        while (!found) {
            Console.Write("Which celestial object do you want information for? ");
            inp = Console.ReadLine();

            foreach (SpaceObject x in solarSystem) {

                if (x.Name.Equals(inp, StringComparison.OrdinalIgnoreCase)) {
                    found = true;
                    x.Draw();
                    Console.WriteLine($"Position after {days} days: {x.getPosition(days, false, 0)}");
                    List<SpaceObject> satellites = new List<SpaceObject>();
                    solarSystem.ForEach((y) => {
                        if (y.Orbits != null && y.Orbits.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)) {
                            satellites.Add(y);
                        }
                    });
                    Console.WriteLine($"Number of natural satellites found for {x.Name}: {satellites.Count}");
                    satellites.ForEach((sat) => {
                        sat.Draw();
                        Console.WriteLine($"Position after {days} days: {sat.getPosition(days, false, 0)}");
                    });
                    break;
                }
            };
            if (!found) {
                Console.WriteLine($"Didn't find a celestial object named {inp}");
            }

        }
    }
}
