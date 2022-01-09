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
        public float LeftWheelSpeed { get; set; } = RobotDefaults.Speed;
        public float RightWheelSpeed { get; set; } = RobotDefaults.Speed;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = new RobotCommand(LeftWheelSpeed, RightWheelSpeed);
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
