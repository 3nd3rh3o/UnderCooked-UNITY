using UnityEngine;
using UnityEngine.AI;

public class MoveToItem : Action
{
    private ItemInstance item;
    private Transform transform;

    public MoveToItem(ItemInstance item, Transform transform, Environment env)
    {
        this.item = item;
        this.transform = transform;
    }

    public void Execute(BaseAgent agent)
    {
        agent.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        agent.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public bool IsDone(BaseAgent agent)
    {
        throw new System.NotImplementedException();
    }
}