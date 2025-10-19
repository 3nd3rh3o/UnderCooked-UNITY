

using Unity.AI.Navigation;
using UnityEngine;

public class DropItemInWorld : Action
{
    private ItemInstance item;
    private Environment env;

    public DropItemInWorld(ItemInstance item, Environment env)
    {
        this.item = item;
        this.env = env;
    }

    public void Execute(BaseAgent agent)
    {
        Transform itemTransform = agent.transform.GetChild(0);
        itemTransform.GetComponent<Rigidbody>().isKinematic = false;
        itemTransform.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        itemTransform.GetComponent<Collider>().enabled = true;
        itemTransform.localPosition = Vector3.forward;
        env.itemInWorld.Add(new (itemTransform, item));
        agent.transform.DetachChildren();
        agent.navSurf.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public bool IsDone(BaseAgent agent)
    {
        return agent.transform.childCount == 0;
    }
}