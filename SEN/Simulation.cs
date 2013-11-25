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
using System.Web.Script.Serialization;

namespace SEN
{
    class Simulation
    {
        private List<Vehicle> vehicles;
        private XmlGenerator xmlGenerator;
        private State state;
        private Server server;

        private List<string> Actions;

        /// Intersection movement will be based on quadrants and their occupation.
        /// The intersection is devided in 4 quadrants:
        /// 
        ///          N
        /// 
        ///        0 | 1
        ///   W   -------   E
        ///        3 | 2
        ///      
        ///          S
        ///        
        /// Traffic coming from the north will be using the following quadrants based on their direction:
        /// Right: 0
        /// North: 0 - 3
        /// Left: 0 - 3 - 2
        /// 
        /// Based on the occupation of the quadrants we can determine which traffic streams are allowed to pass.
        /// A seperate calculation will be implemented for car's turning right. This is because busses turning left will cause a collision.

        private List<bool> grid;

        public Simulation(Server server)
        {
            this.server = server;
            this.state = new State();
            this.vehicles = new List<Vehicle>();
            xmlGenerator = new XmlGenerator();
            this.readFromXml();
        }

        private List<Vehicle> readFromXml()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            xmlGenerator.xml = XDocument.Load(XmlGenerator.path);

            foreach (XElement vehic in xmlGenerator.xml.Root.Nodes())
            {
                Vehicle vehicle = new Vehicle();
                vehicle.Id = vehic.Element("id").Value;

                vehicle.Type =
                    (vehic.Element("type").Value.ToLower() == "car") ? VehicleType.Car :
                    (vehic.Element("type").Value.ToLower() == "bike") ? VehicleType.Bicycle : VehicleType.Bus;

                vehicle.Location =
                    vehic.Element("location").Value.ToLower() == "north" ? Location.North :
                    vehic.Element("location").Value.ToLower() == "east" ? Location.East :
                    vehic.Element("location").Value.ToLower() == "south" ? Location.South : Location.West;

                vehicle.Direction =
                    vehic.Element("direction").Value.ToLower() == "north" ? Direction.North :
                    vehic.Element("direction").Value.ToLower() == "east" ? Direction.East :
                    vehic.Element("direction").Value.ToLower() == "south" ? Direction.South : Direction.West;

                vehicles.Add(vehicle);
            }
            return vehicles;
        }

        public void Run()
        {
            while (true)
            {
                State state = new State();

                //Add vehicles, lights and actions
                state.VehicleState = readFromXml();
                state.LightState = getLightsAndState(state.VehicleState);
                state.Action = this.Actions;

                //send state object as json
                var json = new JavaScriptSerializer().Serialize(state);
                server.sendString(json);

                //remove the vehicles with actions from the xml
                this.xmlGenerator.ClearXML(this.Actions);
                // wait van 10000 miliseconden, of meer :)
                Thread.Sleep(15000);
            }
        }

        private List<Light> getLightsAndState(List<Vehicle> vehicles)
        {
            //reset the grid to be able to determine the lightstates
            grid = resetGrid();

            List<Light> lights = new List<Light>();

            for (int i = 0; i < 20; i++)
            {
                Light light = new Light()
                {
                    Number = ((i % 5) == 0) ? TrafficLightNumber.CarLeft :
                             ((i % 5) == 1) ? TrafficLightNumber.CarStraight :
                             ((i % 5) == 2) ? TrafficLightNumber.CarRight :
                             ((i % 5) == 3) ? TrafficLightNumber.Bus : TrafficLightNumber.Bicycle,

                    Location = (i < 5) ? Shared.Location.North :
                               (i >= 5 && i < 10) ? Shared.Location.East :
                               (i >= 10 && i < 15) ? Shared.Location.South : Shared.Location.West,

                    State = TrafficLightState.Red
                };

                lights.Add(light);
            }

            List<Light> greenLights = new List<Light>();
            Actions = new List<string>();
            getLightState(lights, vehicles, Actions, greenLights, 0);


            return lights;
        }

        private List<Light> getLightState(List<Light> lights, List<Vehicle> vehicles, List<string> actions, List<Light> greenLights, int index)
        {
            var topPrioLight = (from a in lights
                                from b in vehicles
                                where (a.Location == b.Location && a.Number == b.TrafficLightNumber)
                                orderby b.Priority descending
                                select b).ToList();

            if (index < topPrioLight.Count)
            {

                // Traffic light with the highest prio is turned green if the given route is still free
                if (checkIfRouteIsAvailable(topPrioLight[index].Location, topPrioLight[index].Direction))
                {
                    switch (topPrioLight[index].Type)
                    {
                        // Extra check for the green light in case we're dealing with a left- or right turning bus
                        case VehicleType.Bus:
                            int direc = getDirection((int)topPrioLight[index].Location, (int)topPrioLight[index].Direction);
                            if (direc == 1)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].Location) && (x.Number == topPrioLight[index].TrafficLightNumber))).First().State = TrafficLightState.BusLeft;
                            }
                            else if (direc == 2)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].Location) && (x.Number == topPrioLight[index].TrafficLightNumber))).First().State = TrafficLightState.BusRight;
                            }
                            else if (direc == 0)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].Location) && (x.Number == topPrioLight[index].TrafficLightNumber))).First().State = TrafficLightState.GreenOrBusStraight;
                            }
                            break;

                        // In all other cases use the regular green light state
                        default:
                            lights.Where(x => ((x.Location == topPrioLight[index].Location) && (x.Number == topPrioLight[index].TrafficLightNumber))).First().State = TrafficLightState.GreenOrBusStraight;
                            break;
                    }
                    //add the newly turned green light to the list of green lights
                    greenLights.Add(lights.Where(x => ((x.Location == topPrioLight[index].Location) && (x.Number == topPrioLight[index].TrafficLightNumber))).First());
                    actions.Add(topPrioLight[index].Id);
                }
                getLightState(lights, vehicles, actions, greenLights, ++index);
            }

            return lights;
        }

        public int getDirection(int location, int direction)
        {
            var sum = location - direction;

            return (sum % 2 == 0) ? 0 :
                (sum == -1 || sum == 3) ? 1 : 2;
        }

        public List<bool> resetGrid()
        {
            grid = new List<bool>();

            for (int i = 0; i < 4; i++)
            {
                grid.Add(false);
            }

            return grid;
        }

        #region availability check

        public bool checkIfRouteIsAvailable(Shared.Location location, Direction direction)
        {
            switch (location)
            {
                case Shared.Location.North:
                    switch (direction)
                    {
                        case Direction.East:
                            if (grid[0] == false && grid[3] == false && grid[2] == false)
                            {
                                grid[0] = true;
                                grid[3] = true;
                                grid[2] = true;
                                return true;
                            }
                            break;

                        case Direction.South:
                            if (grid[0] == false && grid[3] == false)
                            {
                                grid[0] = true;
                                grid[3] = true;
                                return true;
                            }
                            break;

                        case Direction.West:
                            if (grid[0] == false)
                            {
                                grid[0] = true;
                                return true;
                            }
                            break;
                    }
                    break;

                case Shared.Location.East:
                    switch (direction)
                    {
                        case Direction.South:
                            if (grid[1] == false && grid[0] == false && grid[3] == false)
                            {
                                grid[1] = true;
                                grid[0] = true;
                                grid[3] = true;
                                return true;
                            }
                            break;

                        case Direction.West:
                            if (grid[1] == false && grid[0] == false)
                            {
                                grid[1] = true;
                                grid[0] = true;
                                return true;
                            }
                            break;

                        case Direction.North:
                            if (grid[1] == false)
                            {
                                grid[1] = true;
                                return true;
                            }
                            break;
                    }
                    break;

                case Shared.Location.South:
                    switch (direction)
                    {
                        case Direction.West:
                            if (grid[2] == false && grid[1] == false && grid[0] == false)
                            {
                                grid[2] = true;
                                grid[1] = true;
                                grid[0] = true;
                                return true;
                            }
                            break;

                        case Direction.North:
                            if (grid[2] == false && grid[1] == false)
                            {
                                grid[2] = true;
                                grid[1] = true;
                                return true;
                            }
                            break;

                        case Direction.East:
                            if (grid[2] == false)
                            {
                                grid[2] = true;
                                return true;
                            }
                            break;
                    }
                    break;

                case Shared.Location.West:
                    switch (direction)
                    {
                        case Direction.North:
                            if (grid[3] == false && grid[2] == false && grid[1] == false)
                            {
                                grid[3] = true;
                                grid[2] = true;
                                grid[1] = true;
                                return true;
                            }
                            break;

                        case Direction.East:
                            if (grid[3] == false && grid[2] == false)
                            {
                                grid[3] = true;
                                grid[2] = true;
                                return true;
                            }
                            break;

                        case Direction.South:
                            if (grid[3] == false)
                            {
                                grid[3] = true;
                                return true;
                            }
                            break;
                    }
                    break;
            }

            return false;
        }
        #endregion
    }
}
