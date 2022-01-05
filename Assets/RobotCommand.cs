using System.Runtime.InteropServices;

namespace BSim
{
    public class RobotCommand
    {
        public RobotCommand()
        {
        }

        public RobotCommand(float leftWheelSpeed, float rightWheelSpeed)
        {
            LeftWheelSpeed = leftWheelSpeed;
            RightWheelSpeed = rightWheelSpeed;
        }

        public float LeftWheelSpeed { get; set; }
        public float RightWheelSpeed { get; set; }

        public bool Equals(RobotCommand robotCommand) =>
            robotCommand.LeftWheelSpeed == LeftWheelSpeed && robotCommand.RightWheelSpeed == RightWheelSpeed;

        public static RobotCommand Spin(float speed = RobotDefaults.Speed) =>
            new RobotCommand { LeftWheelSpeed = -speed, RightWheelSpeed = speed };

        public static RobotCommand Straight(float speed = RobotDefaults.Speed) =>
            new RobotCommand { LeftWheelSpeed = speed, RightWheelSpeed = speed };
    }
}
