using SEN.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEN.Shared.Models
{
    public class Vehicle
    {
        // the id, 
        public string id { get; set; }

        // car bus or bike
        public VehicleType type { get; set; }

        // N E S W
        public Location location { get; set; }

        // N E S W
        public Direction direction { get; set; }

        // the location and direction determine which traffic light the vehicle is queued for

        // priority -> (number of cars in queue) * (time since 1st car in queue)
        // duration !> 30sec (?)

        public TrafficLightNumber TrafficLightNumer
        {
            get
            {
                switch (type)
                {
                    case VehicleType.Bus:
                        return TrafficLightNumber.Bus;

                    case VehicleType.Bicycle:
                        return TrafficLightNumber.Bicycle;

                    default:
                        return getCarLight();
                }
            }
        }

        public TrafficLightNumber getCarLight()
        {
            var sum = (int)location - (int)direction;

            return (sum % 2 == 0) ? TrafficLightNumber.CarStraight : 
                (sum == -1 || sum == 3) ? TrafficLightNumber.CarLeft : TrafficLightNumber.CarRight;
        }
    }
}
