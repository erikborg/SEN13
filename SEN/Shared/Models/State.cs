using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEN.Shared.Models
{
    public class State
    {
        public List<Vehicle> VehicleState { get; set; }

        public List<Light> LightState { get; set; }

        public List<string> Action { get; set; }
    }
}
