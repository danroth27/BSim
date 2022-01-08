using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSim
{
    public interface IProgrammableRobot
    {
        IList<IBehavior> Behaviors { get; }
        FixedPriorityArbiter Arbiter { get; }
    }
}
