using System.Linq;
using NUnit;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseAgent : MonoBehaviour
{
    public ActionSeq currentAction;
    public Environment environment;
    public TaskTree.Node currentNode;

    public string currentActionName;

    public GameObject navSurf;
    private Vector3 wanderTarget;

    void Update()
    {
        if (environment == null)
            return;
        if (currentNode != null && currentNode.isDead)
        {
            currentNode = null;
        }
        if (currentAction == null || currentAction.actions==null || currentAction.actions.Count == 0)
        {
            currentAction = null;
            TaskTree.Node leaf = SelectTaskInGoal();
            BuildActionSeq(leaf);
            if (leaf != null && (currentAction == null || currentAction.actions==null || currentAction.actions.Count == 0))
            {
                leaf.inProgress = false;
                return;
            }
            if (currentAction == null && wanderTarget == Vector3.zero)
            {
                wanderTarget = new Vector3(Random.Range(-5, 5), 0, Random.Range(-2, 2));
                GetComponent<NavMeshAgent>().isStopped = false;
                GetComponent<NavMeshAgent>().destination = wanderTarget;
            }
            else if (currentAction != null)
            {
                if (leaf != null)
                {
                    leaf.inProgress = true;
                }
                GetComponent<NavMeshAgent>().isStopped = true;
                wanderTarget = Vector3.zero;
            }

        }
        if ((transform.position - wanderTarget).magnitude < 2)
        {
            wanderTarget = new Vector3(Random.Range(-5, 5), 0, Random.Range(-2, 2));
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().destination = wanderTarget;
        }
<<<<<<< HEAD
        if (currentAction == null || currentAction.actions==null || currentAction.actions.Count == 0)
        {
            
            return;
        }
        
=======
        if (currentAction == null || currentAction.actions == null || currentAction.actions.Count == 0)
        {

            return;
        }

>>>>>>> 42827093851d1c3573b8e8dfa7fc0025952bbde1
        currentActionName = currentAction != null && currentAction.actions.Count > 0 ? currentAction.actions[0].GetType().ToString() : "Idle";
        currentAction?.Execute(this);
    }

    // Overide moi pour changer la facon de selectionner la tache a faire.
    TaskTree.Node SelectTaskInGoal()
    {
        if (environment.goals == null || environment.goals.Count == 0)
            return null;
        foreach (Goal goal in environment.goals)
        {
            if (goal.taskTree != null && goal.taskTree.getLeafTodo() != null)
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