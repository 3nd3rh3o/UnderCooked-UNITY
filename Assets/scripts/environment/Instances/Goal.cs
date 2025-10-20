using System;
using UnityEngine.UIElements;


[Serializable]
public class Goal
{
    public Item item;
    public float remainingTime;
    public TaskTree taskTree;


    public Goal(Item item, float time, TaskTree tree, Environment env)
    {
        this.item = item;
        this.remainingTime = time;
        this.taskTree = new TaskTree(env.knownRecipes, item);
    }
}