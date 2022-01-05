namespace BSim
{
    public interface IRobotController
    {
        void ExecuteRobotCommand(RobotCommand command);

        public RobotCommand ExecutingRobotCommand { get; set; }
    }
}
