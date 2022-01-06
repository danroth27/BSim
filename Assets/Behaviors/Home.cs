using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    internal class Home : IBehavior
    {
        private readonly IArbiter arbiter;

        public Home(IArbiter arbiter)
        {
            this.arbiter = arbiter;
        }

        public float Speed { get; set; } = RobotDefaults.Speed;
        public float Gain { get; set; } = 10;
        public float LightMin { get; set; } = 0.1f;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.NoCommand;
            if ((sensors.LeftLightSensor + sensors.RightLightSensor) / 2 > LightMin)
            {
                var lightDiff = sensors.RightLightSensor - sensors.LeftLightSensor;
                robotCommand = new RobotCommand
                {
                    LeftWheelSpeed = Speed + Gain * lightDiff,
                    RightWheelSpeed = Speed - Gain * lightDiff
                };
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
