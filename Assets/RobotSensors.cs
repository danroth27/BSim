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
        public bool IsBumping { get; set; }
        public bool IsPushing { get; set; }
        public float LeftWheelSpeed { get; set; }
        public float RightWheelSpeed { get; set; }
        public float TimeStep { get; set; }
        public float AngularVelocity { get; set; }
        public float Velocity { get; set; }
    }
}
