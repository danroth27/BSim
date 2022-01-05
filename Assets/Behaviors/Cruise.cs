using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    internal class Cruise : IBehavior
    {
        private readonly IArbiter arbiter;

        public Cruise(IArbiter arbiter)
        {
            this.arbiter = arbiter;
        }

        public float LeftWheelSpeed { get; set; } = RobotDefaults.Speed;
        public float RightWheelSpeed { get; set; } = RobotDefaults.Speed;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = new RobotCommand(LeftWheelSpeed, RightWheelSpeed);
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
