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

    void AddGoal(float initialTime, Item item)
    {
        var newGoal = new Goal(item, initialTime, null, env);
        env.goals.Add(newGoal);
    }

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("main");

        env.goals = new();
        AddGoal(100f, baseItem);
        AddGoal(200f, baseItem);
    }

    void Update()
    {

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
                    goalInstance.Q<ProgressBar>().highValue = goal.remainingTime;
                    goalInstance.Q<ProgressBar>().value = goal.remainingTime;
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
