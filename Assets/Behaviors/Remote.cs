using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSim.Behaviors
{
    public class Remote : IBehavior
    {
        [JsonIgnore]
        public IArbiter Arbiter { get; set; }
        public float Speed { get; set; } = RobotDefaults.Speed;

        public void Update(RobotSensors sensors)
        {
            var v = Input.GetAxis("Vertical");
            var h = Input.GetAxis("Horizontal");

            var robotCommand = new RobotCommand
            {
                LeftWheelSpeed = Speed * Math.Max(Math.Min(v + h, 1), -1),
                RightWheelSpeed = Speed * Math.Max(Math.Min(v - h, 1), -1)
            };

            Arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
