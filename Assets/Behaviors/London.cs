using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Timeline.Actions;

namespace BSim.Behaviors
{
    internal class London : IBehavior
    {
        private readonly IArbiter arbiter;
        private State state = State.Forward;
        private float startTime = 0;
        private float epsilon = 0.001f;

        public London(IArbiter arbiter)
        {
            this.arbiter = arbiter;
        }

        public float Length { get; set; } = 8;
        public float FixedTimeDelta { get; set; } = 0.02f;
        public float Speed { get; set; } = RobotDefaults.Speed;
        public int TimeStepsPerTurn => (int)Math.Round(Math.PI / (4 * FixedTimeDelta * Speed));
        public float TimeAlignedSpeed => (float)(Math.PI / (4 * FixedTimeDelta * TimeStepsPerTurn));

        public void Update(RobotSensors sensors)
        {
            var robotCommand = RobotCommand.NoCommand;
            if (state == State.Forward)
            {
                robotCommand = RobotCommand.Straight(TimeAlignedSpeed);
                if (sensors.Time + epsilon >= startTime + Length / TimeAlignedSpeed)
                {
                    state = State.Turn;
                    startTime = sensors.Time;
                }

            }
            else if (state == State.Turn)
            {
                robotCommand = RobotCommand.Spin(TimeAlignedSpeed);
                if (sensors.Time + epsilon >= startTime + FixedTimeDelta * TimeStepsPerTurn)
                {
                    state = State.Forward;
                    startTime = sensors.Time;
                }
            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }

        enum State
        {
            Forward, Turn
        }
    }
}
