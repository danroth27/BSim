using System;

namespace BSim
{
    public class RobotSensors
    {
        public float LeftLightSensor { get; set; }
        public float RightLightSensor { get; set; }
        public bool LeftProximitySensor { get; set; }
        public bool RightProximitySensor { get; set; }
        public bool IsBumping { get; set; }
        public bool IsPushing => IsBumping && (Math.Abs(LeftWheelSpeed) > 0 || Math.Abs(RightWheelSpeed) > 0);
        public int LeftWheelSpeed { get; set; }
        public int RightWheelSpeed { get; set; }
        public float TimeStep { get; set; }
    }
}
