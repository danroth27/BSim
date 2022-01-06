using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    internal class WallFollow : IBehavior
    {
        private readonly IArbiter arbiter;
        private State state = State.Start;
        private float wallLostTime;

        public WallFollow(IArbiter arbiter)
        {
            this.arbiter = arbiter;
        }

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
                arbiter.ExecuteRobotCommand(robotCommand, this);
                
                if (!sensors.LeftProximitySensor)
                {
                    state = State.LeftLost;
                    wallLostTime = sensors.Time;
                }
            }
            else if (state == State.LeftLost)
            {
                var robotCommand = new RobotCommand(SpeedWithGain(), Speed);
                arbiter.ExecuteRobotCommand(robotCommand, this);

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
                arbiter.ExecuteRobotCommand(robotCommand, this);

                if (!sensors.RightProximitySensor)
                {
                    state = State.RightLost;
                    wallLostTime = sensors.Time;
                }
            }
            else if (state == State.RightLost)
            {
                var robotCommand = new RobotCommand(Speed, SpeedWithGain());
                arbiter.ExecuteRobotCommand(robotCommand, this);

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
                arbiter.ExecuteRobotCommand(robotCommand, this);
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
