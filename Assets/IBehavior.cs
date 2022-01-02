using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BSim
{
    public interface IBehavior
    {
        RobotCommand GetCommand(RobotSensors sensors);
    }

    public class Task
    {

    }

    public class Simulation
    {

    }
}
