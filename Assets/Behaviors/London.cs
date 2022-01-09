using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class London : IBehavior
    {
        private State state = State.Forward;
        private float startTime = -1;
        private float epsilon = 0.001f;

        [JsonIgnore]
        public IArbiter Arbiter { get; set; }
        public float Length { get; set; } = 8;
        public float FixedTimeDelta { get; set; } = 0.02f;
        public float Speed { get; set; } = RobotDefaults.Speed;
        public int TimeStepsPerTurn => (int)Math.Round(Math.PI / (4 * FixedTimeDelta * Speed));
        public float TimeAlignedSpeed => (float)(Math.PI / (4 * FixedTimeDelta * TimeStepsPerTurn));

        public void Update(RobotSensors sensors)
        {
            if (startTime == -1) startTime = sensors.Time;
            
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
            Arbiter.ExecuteRobotCommand(robotCommand, this);
        }

        enum State
        {
            Forward, Turn
        }
    }
}
