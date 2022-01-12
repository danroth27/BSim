using System.Collections.Generic;

namespace BSim.Simulations
{
    public class Robot : SimulationObject
    {
        public List<IBehavior> Behaviors { get; set; } = new List<IBehavior>();
    }
}
