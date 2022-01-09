using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSim
{
    public interface IBehavior
    {
        void Update(RobotSensors sensors);

        IArbiter Arbiter { get; set; }
    }
}
