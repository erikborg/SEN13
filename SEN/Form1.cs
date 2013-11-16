using SEN.Shared.Models;
using SEN.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SEN.Shared;
using System.Web.Script.Serialization;

namespace SEN
{
    public partial class ProjectSEN : Form
    {
        //lock object, used in XmlGenerator class to lock the usage of the xml file.
        //also used to lock the usage of the xml file when sending a new batch of data.
        private Object _lock = new Object();
        private Object _lock2 = new Object();

        XmlGenerator XmlGenerator;
        Server server;
        Simulator simulator;
        string ip;

        //vehicle properties
        int vehicleID = 0;
        Random r = new Random();
        string location { get; set; }
        string direction { get; set; }

        List<string> Actions;

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

        public List<bool> grid;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectSEN()
        {
            server = null;
            InitializeComponent();
            XmlGenerator = new XmlGenerator();

            //DEBUGGING
            location = "North";
            direction = "South";
        }

        private void serverStart_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new Server();
                ip = server.getIP();
                ipLabel.Text = ip;
                simulator = new Simulator(server);
                simulator.Start();
                serverStartButton.Text = "Stop server";
            }
            else
            {
                server.close();
                server = null;
                simulator.Stop();
                simulator = null;
                serverStartButton.Text = "Start server";
            }
        }

        #region XML button actions

        private void clearButton_Click(object sender, EventArgs e)
        {
            lock (_lock)
            {
                // clearButton XML
                XmlGenerator.ClearXML();
                listBox1.Items.Insert(0, "Cleared XML");
            }
        }

        private void createCarButton_Click(object sender, EventArgs e)
        {
            // create car
            generateVehicle("car");
        }

        private void createBikeButton_Click(object sender, EventArgs e)
        {
            // create bike
            generateVehicle("bike");
        }

        private void createBusButton_Click(object sender, EventArgs e)
        {
            // create bus
            generateVehicle("bus");
        }

        private void generateVehicle(string vehicle)
        {
            string logEntry = "";

            var rnd1 = r.Next(0, 3);
            var rnd2 = r.Next(0, 3);
            do
            {
                rnd2 = r.Next(0, 3);
            } while (rnd1 == rnd2);

            this.location = getLocationOrDirection(rnd1);
            this.direction = getLocationOrDirection(rnd2);

            lock (_lock)
            {
                //add a vehicle to our XML. If it's added successfully, up the vehicle ID and add a log entry
                if (XmlGenerator.GenerateVehicle(vehicleID.ToString(), vehicle, this.location, this.direction))
                {
                    vehicleID++;
                    logEntry = String.Concat("Succesfully added a ", vehicle, " to the XML file");
                }
                //notify the user that something went wrong
                else
                {
                    logEntry = String.Concat("Failed to add a ", vehicle, " to the XML file. View console output for details");
                }
            }

            //add the text to our log
            listBox1.Items.Insert(0, logEntry);
        }

        private string getLocationOrDirection(int locdir)
        {
            switch (locdir)
            {
                case 0:
                    return "North";
                case 1:
                    return "East";
                case 2:
                    return "South";
                case 3:
                    return "West";
                default:
                    return null;
            }
        }

        #endregion

        private void sendButton_Click(object sender, EventArgs e)
        {
            State state = new State();

            //Add vehicles, lights and actions
            state.VehicleState = readFromXml();
            state.LightState = getLightsAndState(state.VehicleState);
            state.Action = this.Actions;
            
            //send state object as json
            var json = new JavaScriptSerializer().Serialize(state);
            Console.WriteLine(json);

            //remove the vehicles with actions from the xml
            this.XmlGenerator.ClearXML(this.Actions);
        }
            
        private List<Vehicle> readFromXml()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            this.XmlGenerator.xml = XDocument.Load(XmlGenerator.path);

            foreach (XElement vehic in this.XmlGenerator.xml.Root.Nodes())
            {
                Vehicle vehicle = new Vehicle();
                vehicle.id = vehic.Element("id").Value;

                vehicle.type =
                    (vehic.Element("type").Value.ToLower() == "car") ? VehicleType.Car :
                    (vehic.Element("type").Value.ToLower() == "bike") ? VehicleType.Bicycle : VehicleType.Bus;

                vehicle.location =
                    vehic.Element("location").Value.ToLower() == "north" ? SEN.Shared.Location.North :
                    vehic.Element("location").Value.ToLower() == "east" ? SEN.Shared.Location.East :
                    vehic.Element("location").Value.ToLower() == "south" ? SEN.Shared.Location.South : SEN.Shared.Location.West;

                vehicle.direction =
                    vehic.Element("direction").Value.ToLower() == "north" ? SEN.Shared.Enums.Direction.North :
                    vehic.Element("direction").Value.ToLower() == "east" ? SEN.Shared.Enums.Direction.East :
                    vehic.Element("direction").Value.ToLower() == "south" ? SEN.Shared.Enums.Direction.South : SEN.Shared.Enums.Direction.West;

                vehicles.Add(vehicle);
            }

            return vehicles;
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
                                where (a.Location == b.location && a.Number == b.TrafficLightNumer)
                                orderby b.priority descending
                                select b).ToList();

            if (index < topPrioLight.Count)
            {

                // Traffic light with the highest prio is turned green if the given route is still free
                if (checkIfRouteIsAvailable(topPrioLight[index].location, topPrioLight[index].direction))
                {
                    switch (topPrioLight[index].type)
                    {
                        // Extra check for the green light in case we're dealing with a left- or right turning bus
                        case VehicleType.Bus:
                            int direc = getDirection((int)topPrioLight[index].location, (int)topPrioLight[index].direction);
                            if (direc == 1)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].location) && (x.Number == topPrioLight[index].TrafficLightNumer))).First().State = TrafficLightState.BusLeft;
                            }
                            else if (direc == 2)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].location) && (x.Number == topPrioLight[index].TrafficLightNumer))).First().State = TrafficLightState.BusRight;
                            }
                            else if (direc == 0)
                            {
                                lights.Where(x => ((x.Location == topPrioLight[index].location) && (x.Number == topPrioLight[index].TrafficLightNumer))).First().State = TrafficLightState.GreenOrBusStraight;
                            }
                            break;

                        // In all other cases use the regular green light state
                        default:
                            lights.Where(x => ((x.Location == topPrioLight[index].location) && (x.Number == topPrioLight[index].TrafficLightNumer))).First().State = TrafficLightState.GreenOrBusStraight;
                            break;
                    }
                    //add the newly turned green light to the list of green lights
                    greenLights.Add(lights.Where(x => ((x.Location == topPrioLight[index].location) && (x.Number == topPrioLight[index].TrafficLightNumer))).First());
                    actions.Add(topPrioLight[index].id);
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
