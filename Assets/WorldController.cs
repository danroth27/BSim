using BSim.Simulations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;

public class WorldController : MonoBehaviour, IPointerClickHandler
{
    public GameObject robotPrefab, lightSourcePrefab, puckPrefab;
    public GameObject simObjects;
    public RobotSensorDisplayController robotDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearWorld()
    {
        robotDisplay.SetRobotToDisplay(null);
        foreach (Transform simObject in simObjects.transform)
        {
            GameObject.Destroy(simObject.gameObject);
        }
    }

    public void LoadSimulation(Simulation simulation)
    {
        ClearWorld();

        foreach (var simObject in simulation.Objects)
        {
            switch (simObject)
            {
                case Robot robot:
                    LoadRobot(robot);
                    break;
                case LightSource lightSource:
                    Instantiate(lightSourcePrefab, lightSource.Position, Quaternion.identity, simObjects.transform);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown simulation object type: {simObject.GetType().Name}");
            }
        }
    }

    public void LoadRobot(Robot simRobot)
    {
        var robot = Instantiate(robotPrefab, simRobot.Position, Quaternion.Euler(0, 0, simRobot.Rotation), simObjects.transform).GetComponent<RobotController>();
        robotDisplay.SetRobotToDisplay(robot);
        robot.UpdateBehaviors(simRobot.Behaviors);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        var instance = Instantiate(puckPrefab, mousePosition, Quaternion.identity);
        instance.transform.SetParent(simObjects.transform);
    }
}
