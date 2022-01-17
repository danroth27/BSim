using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class Avoid : IBehavior
    {
        private State state = State.Start;
        private State previousState = State.Start;
        public float Gain { get; set; } = 1;
        public float Speed { get; set; } = RobotDefaults.Speed;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            UnityEngine.Debug.Log($"Avoid state={state}, previousState={previousState}");

            RobotCommand robotCommand = RobotCommand.NoCommand;
            if (state == State.Start)
            {
                if (!(sensors.LeftProximitySensor && sensors.RightProximitySensor && Gain < 0))
                {
                    if (sensors.LeftProximitySensor && previousState != State.Right)
                    {
                        state = State.Left;
                    }
                    else if (sensors.RightProximitySensor && previousState != State.Left)
                    {
                        state = State.Right;
                    }
                }
                previousState = State.Start;
            }
            else if (state == State.Left)
            {
                robotCommand = new RobotCommand(SpeedWithGain(Gain), SpeedWithGain(-Gain));
                if (!sensors.LeftProximitySensor || sensors.LeftProximitySensor && sensors.RightProximitySensor&& Gain < 0)
                {
                    previousState = state;
                    state = State.Start;
                }
            }
            else if (state == State.Right)
            {
                robotCommand = new RobotCommand(SpeedWithGain(-Gain), SpeedWithGain(Gain));
                if (!sensors.RightProximitySensor || sensors.LeftProximitySensor && sensors.RightProximitySensor && Gain < 0)
                {
                    previousState = state;
                    state = State.Start;
                }
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }

        private float SpeedWithGain(float gain) => Math.Min(Math.Max(Speed * (2 * gain + 1), -Speed), Speed);

        private enum State
        {
            Start, Left, Right
        }
    }
}
