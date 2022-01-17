using BSim.Behaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSim
{
    public interface IBehavior
    {
        void Update(RobotSensors sensors);

        void SetArbiter(IArbiter arbiter);
    }
}
