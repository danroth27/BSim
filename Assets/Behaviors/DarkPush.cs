using BSim;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim.Behaviors
{
    public class DarkPush : IBehavior
    {
        private State state = State.Start;
        private float startTime;
        private bool spinLeft;
        private Random random = new Random();
        private IArbiter arbiter;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public float BackupTime { get; set; } = 0.1f;
        public float SpinTime { get; set; } = 0.5f;
        public float ForwardTime { get; set; } = 0.1f;
        public float Speed { get; set; } = RobotDefaults.Speed;
        public float LightLevel { get; set; } = 0;

        public void Update(RobotSensors sensors)
        {
            if (state == State.Start)
            {
                var avgLightLevel = (sensors.LeftLightSensor + sensors.RightLightSensor) / 2;
                if (sensors.IsPushing && avgLightLevel <= LightLevel)
                {
                    startTime = sensors.Time;
                    state = State.Backup;
                }
            }
            else if (state == State.Backup)
            {
                var robotCommand = RobotCommand.Straight(-RobotDefaults.Speed);
                arbiter.ExecuteRobotCommand(robotCommand, this);
                if (sensors.Time > startTime + BackupTime)
                {
                    startTime = sensors.Time;
                    state = State.Spin;
                    spinLeft = random.Next(2) == 0;
                }
            }
            else if (state == State.Spin)
            {
                var robotCommand = RobotCommand.Spin(spinLeft ? RobotDefaults.Speed : -RobotDefaults.Speed);
                arbiter.ExecuteRobotCommand(robotCommand, this);
                if (sensors.Time > startTime + SpinTime)
                {
                    startTime = sensors.Time;
                    state = State.Forward;
                }
            }
            else if (state == State.Forward)
            {
                var robotCommand = RobotCommand.Straight();
                arbiter.ExecuteRobotCommand(robotCommand, this);
                if (sensors.Time > startTime + ForwardTime)
                {
                    state = State.Start;
                    arbiter.ExecuteRobotCommand(RobotCommand.NoCommand, this);
                }
            }
        }

        private enum State
        {
            Start, Backup, Spin, Forward
        }
    }
}
