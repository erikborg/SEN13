using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using SEN.Shared.Models;
using SEN.Shared.Enums;
using SEN.Shared;

namespace SEN
{
    public class XmlGenerator
    {
        //path to our XML
        public const string path = @"vehicles.xml";
        
        //the xdoc representation of our XML
        public XDocument xml;

        private Object xmlLock = new Object();

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlGenerator()
        {
            //constructor logic

            //load the xml from the set path
            xml = XDocument.Load(path);
        }

        public List<Vehicle> readFromXml()
        {
            List<Vehicle> vehicles = new List<Vehicle>();
            xml = XDocument.Load(XmlGenerator.path);

            lock (xmlLock)
            {
                foreach (XElement vehic in xml.Root.Nodes())
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
            }
            return vehicles;
        }

        /// <summary>
        /// Method used to add a vehicle to our XML
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="type">Vehicle type (Bike, Bus, Car)</param>
        /// <param name="location">Location (NESW)</param>
        /// <param name="direction">Direction (NESW)</param>
        public bool GenerateVehicle(string id, string type, string location, string direction)
        {
            try
            {
                lock (xmlLock)
                {
                    //add nodes, representing the vehicle
                    XElement vehicle =
                        new XElement("Vehicle",
                            new XElement("id", id),
                            new XElement("type", type),
                            new XElement("location", location),
                            new XElement("direction", direction));

                    //add the nodes to the root element
                    xml.Element("root").Add(vehicle);

                    //save the changes to the file
                    //move the save action to a different spot when adding bulk data
                    xml.Save(path);
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Method used to clear our XML
        /// </summary>
        public bool ClearXML()
        {
            try
            {
                lock (xmlLock)
                {
                    //remove all childs of the root and save the file
                    xml.Element("root").RemoveAll();
                    xml.Save(path);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Method used to clear our XML
        /// </summary>
        public void ClearXML(List<string> actions)
        {
            foreach (string a in actions)
            {
                try
                {
                    lock (xmlLock)
                    {
                        List<XElement> list = xml.Root.Elements("Vehicle").ToList<XElement>();
                        foreach (XElement Vehicle in list)
                        {
                            if (Vehicle.Element("id").Value == a)
                            {
                                Vehicle.Remove();
                            }
                        }
                        xml.Save(path);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(String.Format("Unable to remove action with number: {0}. \r\n" + e.ToString(), a));
                }
            }
        }
    }
}
