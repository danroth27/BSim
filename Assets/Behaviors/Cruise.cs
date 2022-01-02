using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    internal class Cruise : IBehavior
    {
        public int LeftWheelSpeed { get; set; }
        public int RightWheelSpeed { get; set; }

        public RobotCommand GetCommand(RobotSensors sensors)
        {
            return new RobotCommand 
            { 
                LeftWheelSpeed = LeftWheelSpeed, 
                RightWheelSpeed = RightWheelSpeed 
            };
        }
    }
}
