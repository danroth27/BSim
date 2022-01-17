using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class Gizmo : IBehavior
    {
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float Gain { get; set; } = 1;
        public float TargetLightLevel { get; set; } = 0.5f;
        public float ErrorTolerance { get; set; } = 0.001f;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.Stop;
            var avgLightLevel = (sensors.LeftLightSensor + sensors.RightLightSensor) / 2;
            var lightDiff = TargetLightLevel - avgLightLevel;
            if (Math.Abs(lightDiff) > ErrorTolerance)
            {
                robotCommand = RobotCommand.Straight(Math.Min(Math.Max(Speed * Gain * lightDiff, -Speed), Speed));
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
