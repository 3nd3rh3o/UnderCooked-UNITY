using System.Collections.Generic;
using UnityEngine;
using System;

public class StandInstance : MonoBehaviour
{
    public Environment env;
    public Stand standData;
    public List<ItemInstance> inputs;
    public ItemInstance output;
    public BaseAgent reservedBy;
    public bool reserved = false;

    public float processingTimer;

    public void StartProcessing()
    {
        processingTimer = standData.processing_time;
        // need player ? handle case.
    }

    public void UpdateProcessing(float deltaTime)
    {
        if (processingTimer > 0)
        {
            processingTimer -= deltaTime;
            if (processingTimer <= 0)
            {
                OnProcessingComplete();
            }
        }
    }

    public void OnProcessingComplete()
    {
        // Logic to create output item and/or pop node.
    }


    // MONO METHODS DONT TOUCH
    void Start()
    {
        env.stands.Add(this);   
    }


}