using BSim;
using BSim.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RobotProgrammerController : MonoBehaviour
{
    public Button addButton, removeButton;
    public ListViewController behaviorsListViewController, taskListViewController;
    public PropertiesGridController propertiesGridController;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var behaviorType in Behaviors.GetBehaviorTypes())
        {
            behaviorsListViewController.AddListViewItem(behaviorType, behaviorType.Name.ToFriendlyName());
        }

        foreach (var behavior in ProgrammableRobot.Behaviors.Reverse())
        {
            taskListViewController.AddListViewItem(behavior, behavior.GetType().Name.ToFriendlyName());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IProgrammableRobot ProgrammableRobot { get; set; }

    public void BehaviorListViewSelectionChanged(GameObject unselected, GameObject selected)
    {
        addButton.interactable = selected != null;
    }

    public void TaskListViewSelectionChanged(GameObject unselected, GameObject selected)
    {
        removeButton.interactable = selected != null;

        if (selected != null)
        {
            var behaviorProperties = Behaviors.GetBehaviorProperties((IBehavior)taskListViewController.SelectedValue);
            propertiesGridController.AddPropertyEditors(behaviorProperties);
        }
    }

    public void AddSelectedBehaviorToTask()
    {
        var behaviorType = (Type) behaviorsListViewController.SelectedValue;
        var behavior = (IBehavior) Activator.CreateInstance(behaviorType, ProgrammableRobot.Arbiter);
        taskListViewController.AddListViewItem(behavior, behaviorType.Name.ToFriendlyName());
        ProgrammableRobot.Behaviors.Insert(0, behavior);
        ProgrammableRobot.Arbiter.SetBehaviorPrioritiesInOrder(ProgrammableRobot.Behaviors);

    }

    public void RemoveSelectedBehaviorFromTask()
    {
        var behavior = (IBehavior)taskListViewController.SelectedValue;
        ProgrammableRobot.Behaviors.Remove(behavior);
        taskListViewController.RemoveSelectedItem();
    }


}
