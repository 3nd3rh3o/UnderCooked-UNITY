using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseAgent : MonoBehaviour
{
    public ActionSeq currentAction;
    public Environment environment;
    public TaskTree.Node currentNode;

    public GameObject navSurf;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (environment == null)
            return;
        if (currentAction == null || currentAction.actions.Count == 0)
        {
            GetComponent<NavMeshAgent>().isStopped = true;
            currentAction = null;
            TaskTree.Node leaf = SelectTaskInGoal();
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
            else
            {
                if (goal.item != null)
                    goal.taskTree = new TaskTree(environment.knownRecipes, goal.item);
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