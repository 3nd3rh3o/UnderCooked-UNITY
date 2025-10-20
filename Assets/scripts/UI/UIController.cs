using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public VisualElement root;
    public VisualTreeAsset goalTemplate;
    public List<VisualElement> goalElements = new ();
    public Item item;
    public Environment env;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        env.goals = new();
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("root");
        AddGoal(100f, item);
        AddGoal(200f, item);
    }

    // Update is called once per frame
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
                    goalInstance.Q<VisualElement>("Texture").style.backgroundImage = new StyleBackground(goal.item.render);
                    goalElements.Add(goalInstance);
                }
            }
        }
        for (int i = 0; i < env.goals.Count; i++)
        {
            if (env.goals[i].item == null) continue;
            env.goals[i].remainingTime -= Time.deltaTime;
            goalElements[i].Q<ProgressBar>().value = env.goals[i].remainingTime;
        }
        env.goals.RemoveAll(g => g.remainingTime <= 0 && g.item != null);
    }

    public void AddGoal(float initialTime, Item item)
    {
        VisualElement goalInstance = goalTemplate.Instantiate();
        root.Add(goalInstance);
        goalInstance.Q<ProgressBar>().title = item.name;
        goalInstance.Q<ProgressBar>().highValue = initialTime;
        goalInstance.Q<ProgressBar>().value = initialTime;
        goalInstance.Q<VisualElement>("Texture").style.backgroundImage = new StyleBackground(item.render);
        env.goals.Add(new Goal(item, initialTime, null, env));
        goalElements.Add(goalInstance);
    }
}
