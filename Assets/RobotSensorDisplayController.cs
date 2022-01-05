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
        robot = FindObjectOfType<RobotController>();
        sensorDisplay = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var sensors = robot.GetRobotSensors();

        sensorDisplay.text =
$@"Left light: {sensors.LeftLightSensor:f2}
Right light: {sensors.RightLightSensor:f2}
Left proximity: {sensors.LeftProximitySensor}
Right proximity {sensors.RightProximitySensor}
Bumper force: {sensors.BumperForce:f2}
Bumping: {sensors.IsBumping}
Pushing: {sensors.IsPushing}
Left wheel speed: {sensors.LeftWheelSpeed:f2}
Right wheel speed: {sensors.RightWheelSpeed:f2}";
    }
}
