using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotPanelController : MonoBehaviour
{
    public WorldController world;
    public Button programRobotButton;
    public RobotSensorDisplayController robotSensorDisplay;
    private RobotController robot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedRobot(RobotController robot)
    {
        this.robot = robot;
        robotSensorDisplay.SetRobotToDisplay(robot);
        programRobotButton.interactable = robot != null;
    }

    public void ProgramRobot()
    {
        robot.ShowRobotProgrammer();
    }
}
