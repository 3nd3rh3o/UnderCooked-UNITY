using System.Collections.Generic;

public class SeqEnd : Action
{
    private StandInstance stand;
    private List<ItemInstance> items;

    public SeqEnd(StandInstance stand, List<ItemInstance> items)
    {
        this.stand = stand;
        this.items = items;    
    }
    public void Execute(BaseAgent agent)
    {
        stand.reserved = false;
        foreach (ItemInstance item in items)
        {
            item.UnReserve();
        }
    }
}