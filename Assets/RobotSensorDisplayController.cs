using BSim.Behaviors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotSensorDisplayController : MonoBehaviour
{
    RobotController robot;
    Text sensorDisplay;

    // Start is called before the first frame update
    void Start()
    {
        sensorDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (robot == null)
        {
            sensorDisplay.text = "No robot selected";
        }
        else
        {
            var sensors = robot.GetRobotSensors();
            var behavior = robot.GetExecutingBehavior();
            var behaviorName = behavior != null ? behavior.GetType().Name.ToFriendlyName() : "None";

            if (sensors != null)
            {
                sensorDisplay.text =
$@"Robot sensors:

Left light: {sensors.LeftLightSensor:f2}
Right light: {sensors.RightLightSensor:f2}
Left proximity: {sensors.LeftProximitySensor}
Right proximity: {sensors.RightProximitySensor}
Bumper force: {sensors.BumperForce:f2}
Bumping: {sensors.IsBumping}
Pushing: {sensors.IsPushing}
Left wheel speed: {sensors.LeftWheelSpeed:f2}
Right wheel speed: {sensors.RightWheelSpeed:f2}
Behavior: {behaviorName}";
            }
        }
    }

    public void SetRobotToDisplay(RobotController robot)
    {
        this.robot = robot;
    }
}
