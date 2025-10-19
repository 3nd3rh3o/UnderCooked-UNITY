
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class TakeItemInWorld : Action
{
    private ItemInstance item;
    private Environment env;
    private Transform transform;
    public TakeItemInWorld(ItemInstance item, Transform transform, Environment env)
    {
        this.item = item;
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
        env.itemInWorld.RemoveAll(i => i.Item2 == item && i.Item1 == transform);
        agent.navSurf.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public bool IsDone(BaseAgent agent)
    {
        return agent.transform.childCount == 1;
    }
}