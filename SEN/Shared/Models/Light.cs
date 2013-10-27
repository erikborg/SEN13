using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEN.Shared.Models
{
    public class Light
    {
        public Location Location { get; set; }
        
        public TrafficLightNumber Number { get; set; }

        public TrafficLightState State { get; set; }
    }
}
