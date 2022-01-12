using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace BSim.Simulations
{
    public class Simulation
    {
        public string Name { get; set; }
        public SimulationOptions Options { get; set; } = new SimulationOptions();
        public List<SimulationObject> Objects { get; set; } = new List<SimulationObject>();
    }
}
