
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
           throw new System.NotImplementedException();
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