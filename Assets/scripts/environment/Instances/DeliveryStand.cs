using UnityEngine;

public class DeliveryStand : StandInstance
{

    public void OnReceiveItem(ItemInstance item, BaseAgent agent)
    {
        foreach (var i in env.goals)
        {
            if (i.item == item.ItemData && agent.transform.childCount > 0 && agent.transform.GetChild(0).gameObject != null)
            {
                // Handle goal item delivery logic here
                env.CompletedGoal();
                DestroyImmediate(agent.transform.GetChild(0).gameObject);
                item = null;
                env.goals.Remove(i);
                return;
            }
        }
        Debug.Log("Delivered item is not a goal item, Thrashing it anyway!");
        if (agent.transform.childCount > 0 && agent.transform.GetChild(0).gameObject != null)
        {
            Destroy(agent.transform.GetChild(0).gameObject);
        }
        item = null;
        return;
    }



    public override void Start()
    {
        // Register this delivery stand in the environment
        env.deliveryStands.Add(this);
    }

    public override void OnDestroy()
    {
        env.deliveryStands.Remove(this);
    }
}