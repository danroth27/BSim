using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    internal class AntiMoth : IBehavior
    {
        private readonly Random random = new Random();
        private State state = State.Start;
        private float startTime, spinTime, spinSpeed;
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float LightLevel { get; set; } = 1;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.NoCommand;
            if (state == State.Start)
            {
                if ((sensors.LeftLightSensor + sensors.RightLightSensor) / 2 > LightLevel)
                {
                    spinSpeed = random.Next(2) == 0 ? Speed : -Speed;
                    startTime = sensors.Time;
                    spinTime = (float) (random.NextDouble() * Math.PI / (2 * Speed));
                    state = State.Spin;
                }
            }
            else if (state == State.Spin)
            {
                robotCommand = RobotCommand.Spin(spinSpeed);
                if (sensors.Time > startTime + spinTime)
                {
                    state = State.Start;
                }
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }

        private enum State
        {
            Start, Spin
        }
    }
}
