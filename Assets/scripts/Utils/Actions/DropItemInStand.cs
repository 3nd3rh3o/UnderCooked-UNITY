
public class DropItemInStand : Action
{
    private ItemInstance item;
    private StandInstance stand;
    private Environment env;

    public DropItemInStand(ItemInstance item, StandInstance stand, Environment env)
    {
        this.item = item;
        this.env = env;
        this.stand = stand;
    }

    public void Execute(BaseAgent agent)
    {
        // Logic to take the item
        if (stand is DeliveryStand deliveryStand)
        {
            deliveryStand.OnReceiveItem(item, agent);
        }
        else
        {
            item.OnDropInStand(agent);
            if (stand.inputs == null)
            {
                stand.inputs = new ();
            }
            stand.inputs.Add(item);
            //env.itemsOnStands.Add(new(stand.transform, item, stand));
        }
    }

    public bool IsDone(BaseAgent agent)
    {
        if (agent.transform.childCount == 0)
        {
            return true;
        }
        return false;
    }
}