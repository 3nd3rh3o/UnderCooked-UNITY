

using Unity.AI.Navigation;
using UnityEngine;

public class PutContainer : Action
{
    private StandInstance container;
    private StandInstance superContainer;
    private Environment env;

    public PutContainer(StandInstance container, StandInstance superContainer, Environment env)
    {
        this.container = container;
        this.superContainer = superContainer;
        this.env = env;
    }

    public void Execute(BaseAgent agent)
    {
        Transform container = agent.transform.GetChild(0);
        container.localPosition = Vector3.zero;
        agent.transform.DetachChildren();
        container.position = superContainer.transform.position + Vector3.up * 1.0f;
        container.GetComponent<Rigidbody>().isKinematic = false;
        container.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        container.GetComponent<Collider>().enabled = true;
        
        agent.navSurf.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public bool IsDone(BaseAgent agent)
    {
        return agent.transform.childCount == 0;
    }
}