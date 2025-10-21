using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public VisualElement root;
    public VisualTreeAsset goalTemplate;
    public List<VisualElement> goalElements = new();
    public Environment env;
    public Item baseItem;
    public float maxInterval = 5;
    public float interval = 0;
    public int maxGoals = 3;

    void AddGoal(float initialTime, Item item)
    {
        VisualElement goalInstance = goalTemplate.Instantiate();
        root.Add(goalInstance);
        goalInstance.Q<ProgressBar>().title = item.name;
        goalInstance.Q<ProgressBar>().highValue = initialTime;
        goalInstance.Q<ProgressBar>().value = initialTime;
        goalInstance.Q<VisualElement>("texture").style.backgroundImage = new StyleBackground(item.render);
        env.goals.Add(new Goal(item, initialTime, null, env));
        goalElements.Add(goalInstance);
    }

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("main");

        env.goals = new();
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
            foreach (var goal in env.goals)
            {
                if (goal.item != null)
                {
                    VisualElement goalInstance = goalTemplate.Instantiate();
                    root.Add(goalInstance);
                    goalInstance.Q<ProgressBar>().title = goal.item.name;
                    goalInstance.Q<ProgressBar>().highValue = goal.initialTime;
                    goalInstance.Q<ProgressBar>().value = goal.remainingTime;
                    if (goal.item.render != null)
                        goalInstance.Q<VisualElement>("texture").style.backgroundImage = new StyleBackground(goal.item.render);
                    goalElements.Add(goalInstance);
                }
            }
        }
        for (int i = 0; i < env.goals.Count; i++)
        {
            env.goals[i].remainingTime -= Time.deltaTime;
            goalElements[i].Q<ProgressBar>().value = env.goals[i].remainingTime;
        }
        env.goals.RemoveAll(g => g.remainingTime <= 0);
    }
}
