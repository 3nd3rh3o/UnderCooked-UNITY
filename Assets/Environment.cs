using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Scriptable Objects/Environment")]
public class Environment : ScriptableObject
{
    [SerializeField]
    public List<Goal> goals = new();
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
    }
    public void CompletedGoal()
    {
        currentScore += scorePerGoal * currentMultiplier;
        Debug.Log("Goal completed! +" + scorePerGoal * currentMultiplier + " score. Total score: " + currentScore);
        currentMultiplier = Mathf.Min(maxMultiplier, currentMultiplier + multiplierIncreasePerGoal);
    }
}