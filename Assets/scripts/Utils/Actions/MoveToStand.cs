using UnityEngine;
using UnityEngine.AI;

public class MoveToStand : Action
{
    private StandInstance stand;
    private Transform transform;

    public MoveToStand(StandInstance stand, Transform transform, Environment env)
    {
        this.stand = stand;
        this.transform = transform;
    }

    public void Execute(BaseAgent agent)
    {
        agent.GetComponent<NavMeshAgent>().SetDestination(transform.position);
        agent.GetComponent<NavMeshAgent>().isStopped = false;
    }

    public bool IsDone(BaseAgent agent)
    {
        float distance = Vector3.Distance(agent.transform.position, transform.position);
        return distance < 1.5f;
    }
}