using BSim.Simulations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SimulationSelectorController : MonoBehaviour
{
    public Dropdown simulationSelectorDropdown;
    public Canvas canvas;
    public WorldController world;

    // Start is called before the first frame update
    void Start()
    {
        var simulationNames = new List<string>(Simulations.PrebuiltSimulations.Select(s => s.Name));
        simulationSelectorDropdown.AddOptions(simulationNames);
        world.LoadSimulation(Simulations.PrebuiltSimulations[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged(int index)
    {
        world.LoadSimulation(Simulations.PrebuiltSimulations[index]);
    }
}
