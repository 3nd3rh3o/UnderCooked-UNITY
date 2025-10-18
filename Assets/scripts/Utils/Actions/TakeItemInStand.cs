using UnityEngine;

public class TakeItemInStand : Action
{
    private ItemInstance item;
    private StandInstance stand;

    public TakeItemInStand(ItemInstance item, StandInstance stand, Environment env)
    {
        this.item = item;
        this.stand = stand;
    }

    public void Execute(BaseAgent agent)
    {
        if (agent.transform.childCount > 0)
        {
            return;
        }
        // Logic to take the item from the stand and give it to the agent.
        item.OnPickupFromStand(agent);
        stand.RemoveItem(item);
    }

    public bool IsDone(BaseAgent agent)
    {
        if (agent.transform.childCount > 0)
        {
            return true;
        }
        return false;
    }
}