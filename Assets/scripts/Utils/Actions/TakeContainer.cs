
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class TakeContainer : Action
{
    private StandInstance stand;
    private Environment env;
    private Transform transform;
    public TakeContainer(StandInstance stand, Transform transform, Environment env)
    {
        this.stand = stand;
        this.env = env;
        this.transform = transform;
    }

    public void Execute(BaseAgent agent)
    {
        transform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
        transform.localPosition = Vector3.forward * 0.5f;
        transform.SetParent(agent.transform, false);
        agent.navSurf.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public bool IsDone(BaseAgent agent)
    {
        return agent.transform.childCount == 1;
    }
}