using UnityEngine;

public class DeliveryStand : StandInstance
{

    public void OnReceiveItem(ItemInstance item, BaseAgent agent)
    {
        foreach (var i in env.goals)
        {
            if (i.item == item.ItemData)
            {
                Debug.Log("Goal item delivered!");
                // Handle goal item delivery logic here
                Destroy(agent.transform.GetChild(0).gameObject);
                item = null;
                env.goals.Remove(i);
                return;
            }
        }
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