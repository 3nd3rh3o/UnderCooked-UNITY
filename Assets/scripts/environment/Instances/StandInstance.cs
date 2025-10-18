using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView;

public class StandInstance : MonoBehaviour
{
    public Environment env;
    public Stand standData;
    public List<ItemInstance> inputs;
    public ItemInstance output;
    public BaseAgent reservedBy;
    public bool reserved = false;

    public float processingTimer = -1f;
    private TaskTree.Node node;

    public void StartProcessing(TaskTree.Node node)
    {
        processingTimer = standData.processing_time;
        // need player ? handle case.
    }

    public void UpdateProcessing(Recipe recipe)
    {
        if (processingTimer > 0)
        {
            processingTimer -= Time.deltaTime;
        }
        else
            OnProcessingComplete(recipe);
    }

    public void OnProcessingComplete(Recipe recipe)
    {
        // Logic to create output item and/or pop node.
        processingTimer = -1f;
        inputs?.Clear();
        output = new ItemInstance(recipe.result);
        env.itemsOnStands.Add(new(transform, output, this));
        env.PopNode(node);
    }


    // MONO METHODS DONT TOUCH
    void Start()
    {
        env.stands.Add(this);
    }


    void OnDestroy()
    {
        env.stands.Remove(this);
    }
}