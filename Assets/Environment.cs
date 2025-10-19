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
}