using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public VisualElement root;
    public VisualTreeAsset goalTemplate;
    public VisualTreeAsset scoreInfos;
    public VisualElement sI;
    public List<VisualElement> goalElements = new();
    public Environment env;
    public Item baseItem;
    public float maxInterval;
    public float interval = 0;
    public int maxGoals;

    void AddGoal(float initialTime, Item item)
    {
        VisualElement goalInstance = goalTemplate.Instantiate();
        root.Add(goalInstance);
        goalInstance.Q<ProgressBar>().title = item.name;
        goalInstance.Q<ProgressBar>().highValue = initialTime;
        goalInstance.Q<ProgressBar>().value = initialTime;
        if (item.render != null)
            goalInstance.Q<VisualElement>("texture").style.backgroundImage = new StyleBackground(item.render);
        env.goals.Add(new Goal(item, initialTime, null, env));
        goalElements.Add(goalInstance);
    }

    public void UpdateScore(float currentMultiplier, float score) // UGLY
    {
        VisualElement scoreInfosRoot = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("scoreInfos");
        VisualElement scoreRoot = scoreInfosRoot.Q<VisualElement>("scoreContainer");
        VisualElement multiplierRoot = scoreInfosRoot.Q<VisualElement>("multiplierContainer");

        scoreRoot.Q<Label>("score").text = "" + score;

        multiplierRoot.Q<Label>("multiplier").text = "" + currentMultiplier;
        multiplierRoot.Q<Label>("multiplier").style.color = Color.Lerp(Color.yellow, Color.red, (float)(currentMultiplier - env.minMultiplier) / (env.maxMultiplier - env.minMultiplier));
    }



    void Start() // UGLY
    {
        env.uIController = this;
        env.maxGoalsCompletedInARow = 0;
        env.totalGoalsCompleted = 0;
        env.goalsCompletedInARow = 0;
        env.currentMultiplier = env.minMultiplier;
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("main");
        sI = scoreInfos.Instantiate();
        VisualElement scoreInfosRoot = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("scoreInfos");
        scoreInfosRoot.Add(sI);
        sI.style.position = Position.Absolute;
        sI.style.bottom = 10;
        sI.style.left = 0;
        sI.style.right = 0;
        maxInterval = env.goalIntervalTimer;
        maxGoals = env.maxConcurrentGoals;
        env.goals = new();
        env.currentScore = 0f;
        UpdateScore(env.currentMultiplier, env.currentScore);
    }

    void Update()
    {
        if (env.goals.Count < maxGoals)
        {
            if (interval <= 0)
            {
                AddGoal(150f, env.possibleOutputs[Random.Range(0, env.possibleOutputs.Count)]);
                interval = maxInterval;
            }
            else
            {
                interval -= Time.deltaTime;
            }
        }

        // Si num goal est different de num goalInstances
        // => detruire toutes les goalInstances
        // => recreer toutes les goalInstances en fonction de env.goals

        if (goalElements.Count != env.goals.Count)
        {
            foreach (var goalVE in goalElements)
            {
                goalVE.RemoveFromHierarchy();
            }
            goalElements.Clear();
            for (int i = 0; i < env.goals.Count; i++)
            {
                if (env.goals[i].item != null)
                {
                    AddGoal(env.goals[i].initialTime, env.goals[i].item); // j'ai une fonction pour ça, donc je l'utilise
                }
            }
        }
        for (int i = 0; i < env.goals.Count; i++)
        {
            env.goals[i].remainingTime -= Time.deltaTime;
            if (env.goals[i].remainingTime < 0)
            {
                env.FailedGoal();
            }
            goalElements[i].Q<ProgressBar>().value = env.goals[i].remainingTime;
        }
        env.goals.RemoveAll(g => g.remainingTime <= 0);
    }
}
