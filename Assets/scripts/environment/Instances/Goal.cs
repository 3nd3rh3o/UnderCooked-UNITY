using System;
using UnityEngine.UIElements;


[Serializable]
public class Goal
{
    public Item item;
    public bool itemInDelivery = false;
    public float remainingTime;
    public float initialTime;
    public TaskTree taskTree;


    public Goal(Item item, float time, TaskTree tree, Environment env)
    {
        this.item = item;
        this.initialTime = time;
        this.remainingTime = time;
        this.taskTree = new TaskTree(env.knownRecipes, item);
    }
}