using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    Dictionary<string, Task> tasksDict = new Dictionary<string, Task>();

    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        setFirstLevelTasks();

        Globals.tasks = tasksDict;
        GameEvents.current.onCompleteDigging += completeDigging;
        GameEvents.current.onFoundationFilled += completeFoundation;
        GameEvents.current.onStartSecondLevel += setSecondLevelTasks;
        GameEvents.current.onStartSecondLevel += resetPoints;
        GameEvents.current.onSecondLevelFinished += completeSecondLevel;
        GameEvents.current.onStartRoofLevel += setRoofLevelTasks;
        GameEvents.current.onStartRoofLevel += resetPoints;
        GameEvents.current.onRoofLevelFinished += completeRoof;
        GameEvents.current.onResetLevel += resetPoints;

    }
    void resetPoints()
    {
        PointsLostHandler.resetPoints();
    }
    void setFirstLevelTasks()
    {
        tasksDict = new Dictionary<string, Task>();
        tasksDict.Add("Dig", new Task("Dig", "Use the excavator to dig a hole for the foundation", false));
        tasksDict.Add("Fill", new Task("Fill", "Fill the foundation with cement using cement mixer", false));
        tasksDict.Add("Walls", new Task("Walls", "Construct the frame of the building with wooden planks", false));
    }
    void setSecondLevelTasks()
    {
        tasksDict = new Dictionary<string, Task>();
        tasksDict.Add("Plaster", new Task("Plaster", "Fill in the plasterboards of the building", false));
        tasksDict.Add("Hammer", new Task("Hammer", "Hammer in the plasterboards of the building", false));

    }

    void setRoofLevelTasks()
    {
        tasksDict = new Dictionary<string, Task>();
        tasksDict.Add("PlanksRoof", new Task("PlanksRoof", "Construct the roof of the building", false));
    }

    // Update is called once per frame
    public void removeTask(string name)
    {
        tasksDict.Remove(name);
        Globals.tasks = tasksDict;
    }
    public void addTask(string name, Task task)
    {
        tasksDict.Add(name, task);
        Globals.tasks = tasksDict;
    }
    public void completeDigging()
    {
        removeTask("Dig");
        GameEvents.current.UpdateTaskList();
    }
    public void completeFoundation()
    {
        removeTask("Fill");
        GameEvents.current.UpdateTaskList();
    }
    public void completeFirstFloor()
    {
        removeTask("Walls");
        GameEvents.current.UpdateTaskList();
    }
    public void completeSecondLevel()
    {
        removeTask("Plaster");
        GameEvents.current.UpdateTaskList();
    }
    public void completeRoof()
    {
        removeTask("PlanksRoof");
        GameEvents.current.UpdateTaskList();
    }
    public void reset()
    {
        switch (Globals.currentLevel)
        {
            case 1:
                setFirstLevelTasks();
                break;
            case 2:
                setSecondLevelTasks();
                break;
            case 3:
                setRoofLevelTasks();
                break;
            default:
                break;
        }
    }
}

public class Task
{
    string name;
    string taskString;
    bool completed;
    public Task(string name, string taskString, bool completed)
    {
        this.name = name;
        this.taskString = taskString;
        this.completed = completed;
    }
    public string getName()
    {
        return name;
    }
    public string getTaskString()
    {
        return taskString;
    }
    public void setCompleted(bool completed)
    {
        this.completed = completed;
    }
    public bool getCompleted()
    {
        return completed;
    }
}