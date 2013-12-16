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
        public static Object _lock = new Object();

        XmlGenerator XmlGenerator;
        Server server;
        Simulator simulator;
        string ip;

        //vehicle properties
        int vehicleID = 0;
        Random r = new Random();
        string location { get; set; }
        string direction { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProjectSEN()
        {
            server = null;
            InitializeComponent();
            XmlGenerator = new XmlGenerator();
        }

        private void serverStart_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                server = new Server();
                ip = server.getIP();
                ipLabel.Text = ip;
                simulator = new Simulator(server, XmlGenerator);
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
            XmlGenerator.Clear();
            listBox1.Items.Insert(0, "Cleared");
        }

        private void createCarButton_Click(object sender, EventArgs e)
        {
            // create car
            generateVehicle(VehicleType.Car);
        }

        private void createBikeButton_Click(object sender, EventArgs e)
        {
            // create bike
            generateVehicle(VehicleType.Bicycle);
        }

        private void createBusButton_Click(object sender, EventArgs e)
        {
            // create bus
            generateVehicle(VehicleType.Bus);
        }

        private void generateVehicle(VehicleType vehicle)
        {
            var location = r.Next(0, 4);
            var direction = r.Next(0, 4);
            do
            {
                direction = r.Next(0, 4);
            } while (location == direction);

            //add a vehicle to our XML. If it's added successfully, up the vehicle ID and add a log entry
            XmlGenerator.GenerateVehicle(vehicleID.ToString(), vehicle, (Location)location, (Direction)direction);

            vehicleID++;
            //add the text to our log
            listBox1.Items.Insert(0, String.Concat("Succesfully added a ", vehicle, " to the XML file"));
        }

        #endregion

    }
}
