using BSim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Behaviors
{
    internal class Escape : IBehavior
    {
        RobotCommand previousCommand;

        public float BackupTime { get; set; }
        public float SpinTime { get; set; }
        public float ForwardTime { get; set; }
        public int Speed { get; set; }

        public RobotCommand GetCommand(RobotSensors sensors)
        {
            throw new NotImplementedException();
        }
    }
}
