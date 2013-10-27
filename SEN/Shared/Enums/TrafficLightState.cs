using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEN.Shared
{
    public enum TrafficLightState : byte
    {
        Off = 0,
        OrangeFlick = 1,
        Red = 2,
        Orange = 3,
        GreenOrBusStraight = 4,
        BusRight = 5,
        BusLeft = 6
    }
}
