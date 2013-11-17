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
        // the id
        public string id { get; set; }

        // car bus or bike
        public VehicleType type { get; set; }

        // N E S W
        public Location location { get; set; }

        // N E S W
        public Direction direction { get; set; }

        // priority
        public int priority
        {
            get
            {
                return getPriority();
            }
        }

        // the location and direction determine which traffic light the vehicle is queued for

        // priority -> (number of cars in queue) * (time since 1st car in queue)
        // duration !> 30sec (?)

        public TrafficLightNumber TrafficLightNumber
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
                        return (getDirection() == 0) ? TrafficLightNumber.CarStraight :
                            (getDirection() == 1) ? TrafficLightNumber.CarLeft : TrafficLightNumber.CarRight;
                }
            }
        }

        /// <summary>
        /// Helper method to determine the direction in which a given vehicle is headed.
        /// </summary>
        /// <returns>0 (= Straight), 1 (= Left), 2 (= Right)</returns>
        public int getDirection()
        {
            var sum = (int)location - (int)direction;

            return (sum % 2 == 0) ? 0 :
                (sum == -1 || sum == 3) ? 1 : 2;

            //return (sum % 2 == 0) ? TrafficLightNumber.CarStraight : 
            //    (sum == -1 || sum == 3) ? TrafficLightNumber.CarLeft : TrafficLightNumber.CarRight;
        }

        /// <summary>
        /// Helper method to determine the priority of the given vehicle in queue, based on it's type, location and direction.
        /// </summary>
        /// <returns>Priority (int)</returns>
        public int getPriority()
        {
            int priority = 0;

            //switch the vehicle type to detemine the initial priority.
            switch (type)
            {
                case VehicleType.Bus:
                    priority += 10000;
                    break;
                case VehicleType.Car:
                    priority += 1000;
                    break;
                default:
                    return priority;
            }

            //switch the direction of the vehicle to determine additional priority.
            switch (getDirection())
            {
                //straight
                case 0:
                    priority += (location == Location.North || location == Location.South) ? 100 : 40;
                    break;
                //left
                case 1:
                    priority += (location == Location.West || location == Location.East) ? 80 : 60;
                    break;
                //right
                case 2:
                    priority += (location == Location.West || location == Location.East) ? 80 : 60;
                    break;
            }

            return priority;
        }
    }
}
