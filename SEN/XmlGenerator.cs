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
        private List<Vehicle> VehicleList;

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlGenerator()
        {
            //constructor logic
            VehicleList = new List<Vehicle>();
        }

        public List<Vehicle> getVehicles()
        {
            lock (ProjectSEN._lock)
            {
                Console.WriteLine("Vehicles loaded:{0}", VehicleList.Count);
                return VehicleList;
            }
        }

        /// <summary>
        /// Method used to add a vehicle to our List
        /// </summary>
        /// <param name="id">Vehicle ID</param>
        /// <param name="type">Vehicle type (Bike, Bus, Car)</param>
        /// <param name="location">Location (NESW)</param>
        /// <param name="direction">Direction (NESW)</param>
        public void GenerateVehicle(string id, VehicleType type, Location location, Direction direction)
        {
            Vehicle newVehicle = new Vehicle();
            newVehicle.Id = id;
            newVehicle.Type = type;
            newVehicle.Location = location;
            newVehicle.Direction = direction;

            lock (ProjectSEN._lock)
            {
                VehicleList.Add(newVehicle);
            }
        }

        /// <summary>
        /// Method used to clear our XML
        /// </summary>
        public void Clear()
        {
            lock (ProjectSEN._lock)
            {
                VehicleList.Clear();
            }
        }

        /// <summary>
        /// Method used to clear our XML
        /// </summary>
        public void ClearActions(List<string> ActionList)
        {
            lock (ProjectSEN._lock)
            {
                for (int i = 0; i < VehicleList.Count; i++)
                {
                    Vehicle vehicle = VehicleList[i];
                    if (ActionList.Contains(vehicle.Id))
                    {
                        VehicleList.RemoveAt(i);
                    }
                }
            }
        }
    }
}
