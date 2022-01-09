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
        [JsonIgnore]
        public IArbiter Arbiter { get; set; }
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float Gain { get; set; } = 1;
        public float TargetLightLevel { get; set; } = 1;
        public float ErrorTolerance { get; set; } = 0.001f;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.Stop;
            var avgLightLevel = (sensors.LeftLightSensor + sensors.RightLightSensor) / 2;
            var lightDiff = TargetLightLevel - avgLightLevel;
            if (Math.Abs(lightDiff) > ErrorTolerance)
            {
                robotCommand = RobotCommand.Straight(Speed * Gain * lightDiff);
            }
            Arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
