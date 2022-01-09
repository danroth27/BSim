using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class Cruise : IBehavior
    {
        [JsonIgnore]
        public IArbiter Arbiter { get; set; }
        public float LeftWheelSpeed { get; set; } = RobotDefaults.Speed;
        public float RightWheelSpeed { get; set; } = RobotDefaults.Speed;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = new RobotCommand(LeftWheelSpeed, RightWheelSpeed);
            Arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
