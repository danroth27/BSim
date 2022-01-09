using BSim.Simulations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldController : MonoBehaviour, IPointerClickHandler
{
    public GameObject robotPrefab, lightSourcePrefab, puckPrefab;
    public GameObject simObjects;
    public RobotSensorDisplayController robotDisplay;
    public Dropdown simulationSelectorDropdown;


    // Start is called before the first frame update
    void Start()
    {
        var simulationNames = new List<string>(Simulations.PrebuiltSimulations.Select(s => s.Name));
        simulationSelectorDropdown.AddOptions(simulationNames);
        LoadSimulation(Simulations.PrebuiltSimulations[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(int index)
    {
        LoadSimulation(Simulations.PrebuiltSimulations[index]);
    }

    void LoadSimulation(Simulation simulation)
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

    public void ResetSimulation() =>
        LoadSimulation(Simulations.PrebuiltSimulations[simulationSelectorDropdown.value]);

    public void ClearWorld()
    {
        robotDisplay.SetRobotToDisplay(null);
        foreach (Transform simObject in simObjects.transform)
        {
            GameObject.Destroy(simObject.gameObject);
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
