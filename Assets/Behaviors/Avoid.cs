using System;

namespace BSim.Behaviors
{
    public class Avoid : IBehavior
    {
        private State state = State.Start;
        public float Gain { get; set; } = 1;
        public float Speed { get; set; } = RobotDefaults.Speed;
        private IArbiter arbiter;
        private Timer leftTimer = new Timer();
        private Timer rightTimer = new Timer();
        private float leakyTurnTime = 0.02f;
        private float turnTimeLeak = 0.001f;
        private float turnTimeSample = 0;

        public void SetArbiter(IArbiter arbiter) => this.arbiter = arbiter;

        public void Update(RobotSensors sensors)
        {
            UnityEngine.Debug.Log($"Avoid leakyTurnTime: {leakyTurnTime}, turnTimeSample: {turnTimeSample}");

            leakyTurnTime = Math.Max(Math.Min(leakyTurnTime + turnTimeSample - turnTimeLeak, (float) Math.PI / (2 * Speed)), 0.02f);
            turnTimeSample = 0;

            RobotCommand robotCommand = RobotCommand.NoCommand;
            if (leftTimer.Test(sensors.Time))
            {
                robotCommand = new RobotCommand(SpeedWithGain(Gain), SpeedWithGain(-Gain));
            }
            else if (rightTimer.Test(sensors.Time))
            {
                robotCommand = new RobotCommand(SpeedWithGain(-Gain), SpeedWithGain(Gain));
            }
            else if (sensors.LeftProximitySensor && sensors.RightProximitySensor && Gain < 0)
            {
                leftTimer = new Timer();
                rightTimer = new Timer();
            }
            else if (sensors.LeftProximitySensor)
            {
                turnTimeSample = 0.04f;
                leftTimer = new Timer(sensors.Time, leakyTurnTime);
                robotCommand = new RobotCommand(SpeedWithGain(Gain), SpeedWithGain(-Gain));
            }
            else if (sensors.RightProximitySensor)
            {
                turnTimeSample = 0.04f;
                rightTimer = new Timer(sensors.Time, leakyTurnTime);
                robotCommand = new RobotCommand(SpeedWithGain(-Gain), SpeedWithGain(Gain));

            }
            arbiter.ExecuteRobotCommand(robotCommand, this);
        }

        private float SpeedWithGain(float gain) => Math.Min(Math.Max(Speed * (2 * gain + 1), -Speed), Speed);

        private enum State
        {
            Start, Left, Right
        }

        private class Timer
        {
            private float startTime;
            private float duration;

            public Timer() { }

            public Timer(float startTime, float duration)
            {
                this.startTime = startTime;
                this.duration = duration;
            }

            public bool Test(float currentTime) => currentTime < startTime + duration;
        }
    }
}
