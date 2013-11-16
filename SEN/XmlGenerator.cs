using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SEN
{
    public class XmlGenerator
    {
        //path to our XML
        public const string path = @"vehicles.xml";
        
        //the xdoc representation of our XML
        public XDocument xml;

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlGenerator()
        {
            //constructor logic

            //load the xml from the set path
            xml = XDocument.Load(path);
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
                //remove all childs of the root and save the file
                xml.Element("root").RemoveAll();
                xml.Save(path);
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
                    xml.Descendants("Vehicle")
                       .Where(child => child.Descendants("id").First().Value == a)
                       .Remove();
                    xml.Save(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(String.Format("Unable to remove action with number: {0}. \r\n" + e.ToString(), a));
                }
            }
        }
    }
}
