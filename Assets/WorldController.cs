using BSim.Simulations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldController : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler
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
    private Camera eventCamera;
    private Vector3 wallStart;
    private BoxCollider2D worldCollider;

    void Awake()
    {
        worldCollider = GetComponentInChildren<BoxCollider2D>();
    }

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

    public void OnPointerDown(PointerEventData eventData)
    {
        var addToWorld = worldEditorDropdown.options[worldEditorDropdown.value].text;
        if (addToWorld == "Wall")
        {
            eventCamera = eventData.enterEventCamera;
            wallStart = GetMousePosition(eventData);
            var wall = Instantiate(wallPrefab, wallStart, Quaternion.identity);
            wall.transform.SetParent(simObjects.transform);
            var wallScale = wall.transform.localScale;
            wallScale.x = 0;
            wall.transform.localScale = wallScale;
            SetSelectedOjbect(wall);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        var addToWorld = worldEditorDropdown.options[worldEditorDropdown.value].text;
        if (addToWorld == "Wall")
        {
            var wallEnd = GetMousePosition(eventData);
            if (worldCollider.OverlapPoint(wallEnd))
            {
                selectedObject.transform.SetPositionAndRotation((wallStart + wallEnd) / 2, Quaternion.FromToRotation(Vector3.right, wallEnd - wallStart));
                var wallScale = selectedObject.transform.localScale;
                wallScale.x = (wallEnd - wallStart).magnitude;
                selectedObject.transform.localScale = wallScale;
            }
        }
    }

    private Vector3 GetMousePosition(PointerEventData eventData)
    {
        var mousePosition = eventCamera.ScreenToWorldPoint(eventData.position);
        mousePosition.z = 0;
        return mousePosition;
    }
}
