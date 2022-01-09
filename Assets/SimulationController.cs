using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationController : MonoBehaviour
{
    public Button startButton, stopButton, stepButton, resetButton;
    private bool isStepping;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isStepping)
        {
            Time.timeScale = 0;
            isStepping = false;
        }
    }

    public void StartSimulation()
    {
        Time.timeScale = 1;
        startButton.interactable = false;
        stopButton.interactable = true;
        stepButton.interactable = false;
        resetButton.interactable = false;
    }

    public void StopSimulation()
    {
        Time.timeScale = 0;
        startButton.interactable = true;
        stopButton.interactable = false;
        stepButton.interactable = true;
    }

    public void StepSimulation()
    {
        isStepping = true;
        Time.timeScale = 1;
    }

    public void ResetSimulation()
    {

    }
}
