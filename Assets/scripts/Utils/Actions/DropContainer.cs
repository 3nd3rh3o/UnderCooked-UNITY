using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DropContainer : Action
{
    private StandInstance container;
    private Environment env;

    public DropContainer(StandInstance container, Environment env)
    {
        this.container = container;
        this.env = env;
    }

    public void Execute(BaseAgent agent)
    {
        Transform container = agent.transform.GetChild(0);
        container.localPosition = Vector3.zero;
        agent.transform.DetachChildren();
        container.position = agent.transform.position + Vector3.forward * 1.0f + Vector3.up * 1f;
        container.GetComponent<Rigidbody>().isKinematic = false;
        container.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        container.GetComponent<Collider>().enabled = true;
        
        container.GetComponent<NavMeshObstacle>().enabled = true;
        
        agent.navSurf.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public bool IsDone(BaseAgent agent)
    {
        return agent.transform.childCount == 0;
    }
}