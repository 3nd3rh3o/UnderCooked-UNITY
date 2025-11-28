using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Scriptable Objects/Environment")]
public class Environment : ScriptableObject
{
    [SerializeField]
    public List<Goal> goals = new();
    public UIController uIController;
    public List<Tuple<Transform, ItemInstance>> itemInWorld = new();
    public List<Tuple<Transform, ItemInstance, StandInstance>> itemsOnStands = new();
    public List<StandInstance> stands;
    public List<Recipe> knownRecipes;
    public List<StandInstance> deliveryStands;
    public List<Item> possibleOutputs = new();
    public float maxMultiplier = 5f;
    public float minMultiplier = 1f;
    public float currentMultiplier = 1f;
    public float multiplierIncreasePerGoal = 0.5f;
    public float multiplierDecreasePerFail = 1f;
    public int maxConcurrentGoals = 3;
    public float goalIntervalTimer = 5f;
    public float currentScore = 0f;
    public float scorePerGoal = 100f;
    public int maxGoalsCompletedInARow = 0;
    public int goalsCompletedInARow = 0;
    public int totalGoalsCompleted = 0;

    public void PopNode(TaskTree.Node node)
    {
        foreach (Goal goal in goals)
        {
            if (goal.taskTree != null)
            {
                goal.taskTree.PopNode(node);
            }
        }
    }

    public void FailedGoal()
    {
        currentMultiplier = Mathf.Max(minMultiplier, currentMultiplier - multiplierDecreasePerFail);
        uIController.UpdateScore(currentMultiplier, currentScore);
        goalsCompletedInARow = 0;
        Debug.Log("Lost the goals streak. Our MAX is at " + maxGoalsCompletedInARow);
    }
    public void CompletedGoal()
    {
        currentScore += scorePerGoal * currentMultiplier;
        currentMultiplier = Mathf.Min(maxMultiplier, currentMultiplier + multiplierIncreasePerGoal);
        uIController.UpdateScore(currentMultiplier, currentScore);
        totalGoalsCompleted++;
        goalsCompletedInARow++;
        if (goalsCompletedInARow > maxGoalsCompletedInARow)
        {
            maxGoalsCompletedInARow = goalsCompletedInARow;
        }
        Debug.Log("Currently " + goalsCompletedInARow + " goals completed in a row. Our MAX is at " + maxGoalsCompletedInARow);
        Debug.Log("In total, we completed " + totalGoalsCompleted + " goals");
    }
}