using UnityEngine;

public class BaseAgent : MonoBehaviour
{
    public ActionSeq currentAction;
    public Environment environment;
    public TaskTree.Node currentNode;

    void Start()
    {

    }

    void Update()
    {
        if (environment == null)
            return;
        if (currentAction == null || currentAction.actions.Count == 0)
        {
            currentAction = null;
            TaskTree.Node leaf = SelectTaskInGoal();
            if (leaf != null) return;
            BuildActionSeq(leaf);
        }
        currentAction?.Execute(this);
    }

    // Overide moi pour changer la facon de selectionner la tache a faire.
    TaskTree.Node SelectTaskInGoal()
    {
        if (environment.goals == null || environment.goals.Count == 0)
            return null;
        foreach (Goal goal in environment.goals)
        {
            if (goal.taskTree != null)
            {
                return goal.taskTree.getLeafTodo();
            }
        }
        return null;
    }

    // Appelle ActionSeq.CanBuild puis ActionSeq constructor. Utilise un constructeur
    // different pour pour changer le comportement de l'agent.
    void BuildActionSeq(TaskTree.Node node)
    {
        if (ActionSeq.CanBuild(node, environment))
        {
            currentNode = node;            
            currentAction = new ActionSeq(node, environment);
        }
    }
}