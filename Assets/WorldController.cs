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
    public GameObject robotPrefab, lightSourcePrefab, puckPrefab, wallPrefab;
    public GameObject simObjects;
    public GameObject worldBackground;
    public RobotPanelController robotPanel;
    public Dropdown simulationSelectorDropdown;
    public Dropdown worldEditorDropdown;
    public Toggle fantasyModeToggle;
    public Slider latencySlider;
    private GameObject selectedObject;

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
        if (selectedObject != null && Input.GetKey(KeyCode.Delete))
        {
            if (selectedObject.CompareTag("Robot"))
            {
                robotPanel.SelectedRobot(null);
            }
            GameObject.Destroy(selectedObject);
        }
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
                    LoadSimulationObject(lightSource, lightSourcePrefab);
                    break;
                case Puck puck:
                    LoadSimulationObject(puck, puckPrefab);
                    break;
                case Wall wall:
                    LoadWall(wall);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown simulation object type: {simObject.GetType().Name}");
            }
        }

        fantasyModeToggle.isOn = simulation.Options.FantasyMode;
        latencySlider.value = simulation.Options.Latency / Time.fixedDeltaTime;
    }

    public void ResetSimulation() =>
        LoadSimulation(Simulations.PrebuiltSimulations[simulationSelectorDropdown.value]);

    public void ClearWorld()
    {
        robotPanel.SelectedRobot(null);
        foreach (Transform simObject in simObjects.transform)
        {
            GameObject.Destroy(simObject.gameObject);
        }
    }

    public GameObject LoadSimulationObject(SimulationObject simObject, GameObject prefab) =>
        Instantiate(prefab, simObject.Position, Quaternion.Euler(0, 0, simObject.Rotation), simObjects.transform);

    public void LoadRobot(Robot simRobot)
    {
        var robot = LoadSimulationObject(simRobot, robotPrefab).GetComponent<RobotController>();
        robotPanel.SelectedRobot(robot);
        robot.UpdateBehaviors(simRobot.Behaviors);
    }

    public void LoadWall(Wall simWall)
    {
        var wall = LoadSimulationObject(simWall, wallPrefab);
        var localScale = wall.transform.localScale;
        localScale.x = simWall.Length;
        wall.transform.localScale = localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.rawPointerPress == worldBackground)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            mousePosition.z = 0;

            var addToWorld = worldEditorDropdown.options[worldEditorDropdown.value].text;
            GameObject prefab = null;
            switch (addToWorld)
            {
                case "Puck":
                    prefab = puckPrefab;
                    break;
                case "Robot":
                    prefab = robotPrefab;
                    break;
                case "Light":
                    prefab = lightSourcePrefab;
                    break;
                case "Wall":
                    prefab = wallPrefab;
                    break;
            }

            if (prefab != null)
            {
                var instance = Instantiate(prefab, mousePosition, Quaternion.identity);
                instance.transform.SetParent(simObjects.transform);
                SetSelectedOjbect(instance);
            }
        }
        else
        {
            SetSelectedOjbect(eventData.rawPointerPress);
        }
    }

    private void SetSelectedOjbect(GameObject selectedObject)
    {
        this.selectedObject = selectedObject;
        if (selectedObject != null && selectedObject.CompareTag("Robot"))
        {
            robotPanel.SelectedRobot(selectedObject.GetComponent<RobotController>());
        }
    }
}
