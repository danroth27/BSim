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
        public float Gain { get; set; } = 1;
        public float Speed { get; set; } = RobotDefaults.Speed;
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            if (state == State.Start)
            {
                if (sensors.LeftProximitySensor || sensors.RightProximitySensor)
                {
                    state = State.Avoiding;
                }
            }
            else if (state == State.Avoiding)
            {
                RobotCommand robotCommand = RobotCommand.NoCommand;
                if (sensors.LeftProximitySensor)
                {
                    robotCommand = new RobotCommand(SpeedWithGain(Gain), SpeedWithGain(-Gain));
                }
                else if (sensors.RightProximitySensor)
                {
                    robotCommand = new RobotCommand(SpeedWithGain(-Gain), SpeedWithGain(Gain));
                }
                else
                {
                    state = State.Start;
                }
                arbiter.ExecuteRobotCommand(robotCommand, this);
            }
        }

        private float SpeedWithGain(float gain) => Math.Min(Math.Max(Speed * (2 * gain + 1), -Speed), Speed);

        private enum State
        {
            Start, Avoiding
        }
    }
}
