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

            var rnd1 = r.Next(0, 4);
            var rnd2 = r.Next(0, 4);
            do
            {
                rnd2 = r.Next(0, 4);
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
           
        }


    }
}
