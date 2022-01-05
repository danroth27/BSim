using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BSim.Behaviors
{
    internal class Remote : IBehavior
    {
        private readonly IArbiter arbiter;

        public Remote(IArbiter arbiter)
        {
            this.arbiter = arbiter;
        }

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

            arbiter.ExecuteRobotCommand(robotCommand, this);
        }
    }
}
