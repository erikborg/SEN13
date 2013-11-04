using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEN.Shared.Models;
using SEN.Shared.Enums;
using SEN.Shared;
using System.Xml.Linq;
using System.Net.Sockets;
using System.Threading;

namespace SEN
{
    class Simulation
    {
        private List<Vehicle> vehicles;
        private XmlGenerator xmlGenerator;
        private State state;
        private Server server;

        public Simulation(Server server)
        {
            this.server = server;
            this.state = new State();
            this.vehicles = new List<Vehicle>();
            xmlGenerator = new XmlGenerator();
            this.readFromXml();
        }

        private void readFromXml()
        {
            xmlGenerator.xml = XDocument.Load(XmlGenerator.path);

            foreach (XElement vehic in xmlGenerator.xml.Root.Nodes())
            {
                Vehicle vehicle = new Vehicle();
                vehicle.id = vehic.Element("id").Value;

                vehicle.type =
                    (vehic.Element("type").Value.ToLower() == "car") ? VehicleType.Car :
                    (vehic.Element("type").Value.ToLower() == "bike") ? VehicleType.Bicycle : VehicleType.Bus;

                vehicle.location =
                    vehic.Element("location").Value.ToLower() == "north" ? Location.North :
                    vehic.Element("location").Value.ToLower() == "east" ? Location.East :
                    vehic.Element("location").Value.ToLower() == "south" ? Location.South : Location.West;

                vehicle.direction =
                    vehic.Element("direction").Value.ToLower() == "north" ? Direction.North :
                    vehic.Element("direction").Value.ToLower() == "east" ? Direction.East :
                    vehic.Element("direction").Value.ToLower() == "south" ? Direction.South : Direction.West;

                vehicles.Add(vehicle);
            }
        }

        public void Run()
        {
            while (true)
            {
                // hier komt alle logic welke actions verstuurd moet worden etc
                // vehicles + lightstates + actions

                // verstuur data naar server clients
                server.sendString("kip is lekker");
                // wait van 10000 miliseconden, of meer :)
                Thread.Sleep(10000);
            }
        }
    }
}
