using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class Home : IBehavior
    {
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float Gain { get; set; } = 10;
        public float LightMin { get; set; } = 0.1f;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.NoCommand;
            if ((sensors.LeftLightSensor + sensors.RightLightSensor) / 2 > LightMin)
            {
                var lightDiff = sensors.RightLightSensor - sensors.LeftLightSensor;
                robotCommand = new RobotCommand
                {
                    LeftWheelSpeed = Math.Max(Math.Min(Speed + Gain * lightDiff, 2), -2),
                    RightWheelSpeed = Math.Max(Math.Min(Speed - Gain * lightDiff, 2), -2)
                };
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
