using System;

namespace BSim
{
    public class RobotSensors
    {
        const float pushThreshold = 0.1f;
        public float LeftLightSensor { get; set; }
        public float RightLightSensor { get; set; }
        public bool LeftProximitySensor { get; set; }
        public bool RightProximitySensor { get; set; }
        public float BumperForce { get; set; }
        public bool IsBumping { get; set; }
        public bool IsPushing => IsBumping && BumperForce < 100f;
        public float LeftWheelSpeed { get; set; }
        public float RightWheelSpeed { get; set; }
        public float Time { get; set; }
    }
}
