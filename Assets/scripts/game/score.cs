using System.Collections.Generic;
using UnityEngine;

public class score : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Environment env;
    public int currentScore = 0;
    public float multiplier = 1.0f;
    public float maxMultiplier = 5.0f;
    public float minMultiplier = 1.0f;
    public float multiplierMaxDecay = 20f;
    public float multiplierIncreasePerSuccess = 0.5f;
    public float multiplierDecreasePerFailure = 1.0f;
    public int scorePerGoal = 100;
    public float multiplierCurrentTime = 0f;
    public int goalAmount = 0;

    // Update is called once per frame
    void Start()
    {
        goalAmount = env.goals.Count;
        Debug.Log("Score: " + currentScore + " Multiplier: " + multiplier);
    }
    void Update()
    {
        multiplierCurrentTime += Time.deltaTime;
        if (multiplierCurrentTime >= multiplierMaxDecay)
        {
            multiplier -= multiplierDecreasePerFailure;
            multiplierCurrentTime = 0f;
            if (multiplier < minMultiplier)
            {
                multiplier = minMultiplier;
            }
        }
        if (env.goals.Count != goalAmount)
        {
            if (env.goals.Count < goalAmount)
            {
                currentScore += Mathf.RoundToInt(scorePerGoal * multiplier);
                multiplier += multiplierIncreasePerSuccess;
                multiplierCurrentTime = 0f;
                Debug.Log("Score: " + currentScore + " Multiplier: " + multiplier);
                if (multiplier > maxMultiplier)
                {
                    multiplier = maxMultiplier;
                }
            }
            goalAmount = env.goals.Count;
        }
    }
}
