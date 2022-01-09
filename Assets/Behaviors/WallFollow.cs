using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class WallFollow : IBehavior
    {
        private State state = State.Start;
        private float wallLostTime;

        [JsonIgnore]
        public IArbiter Arbiter { get; set; }
        public float Gain { get; set; } = 1;
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float WallLostTimeout { get; set; } = 1;

        public void Update(RobotSensors sensors)
        {
            if (state == State.Start)
            {
                if (sensors.LeftProximitySensor)
                {
                    state = State.Left;
                }
                else if (sensors.RightProximitySensor)
                {
                    state = State.Right;
                }
            }
            else if (state == State.Left)
            {
                var robotCommand = new RobotCommand(Speed, SpeedWithGain());
                Arbiter.ExecuteRobotCommand(robotCommand, this);
                
                if (!sensors.LeftProximitySensor)
                {
                    state = State.LeftLost;
                    wallLostTime = sensors.Time;
                }
            }
            else if (state == State.LeftLost)
            {
                var robotCommand = new RobotCommand(SpeedWithGain(), Speed);
                Arbiter.ExecuteRobotCommand(robotCommand, this);

                if (sensors.LeftProximitySensor)
                {
                    state = State.Left;
                }
                else if (sensors.Time > wallLostTime + WallLostTimeout)
                {
                    state = State.Stop;
                }
            }
            else if (state == State.Right)
            {
                var robotCommand = new RobotCommand(SpeedWithGain(), Speed);
                Arbiter.ExecuteRobotCommand(robotCommand, this);

                if (!sensors.RightProximitySensor)
                {
                    state = State.RightLost;
                    wallLostTime = sensors.Time;
                }
            }
            else if (state == State.RightLost)
            {
                var robotCommand = new RobotCommand(Speed, SpeedWithGain());
                Arbiter.ExecuteRobotCommand(robotCommand, this);

                if (sensors.RightProximitySensor)
                {
                    state = State.Right;
                }
                else if (sensors.Time > wallLostTime + WallLostTimeout)
                {
                    state = State.Stop;
                }
            }
            else if (state == State.Stop)
            {
                var robotCommand = RobotCommand.NoCommand;
                Arbiter.ExecuteRobotCommand(robotCommand, this);
                state = State.Start;
            }
        }

        private float SpeedWithGain() => Math.Min(Math.Max(Speed * (1 - Gain), -Speed), Speed);

        private enum State
        {
            Start, Left, Right, LeftLost, RightLost, Stop
        }
    }
}
