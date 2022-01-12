using UnityEngine;

namespace BSim.Simulations
{
    public abstract class SimulationObject
    {
        public Vector2 Position { get; set; } = Vector2.zero;
        public float Rotation { get; set; }
    }
}
