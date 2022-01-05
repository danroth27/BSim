using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim
{
    public interface IArbiter
    {
        void ExecuteRobotCommand(RobotCommand command, IBehavior behavior);
    }
}
