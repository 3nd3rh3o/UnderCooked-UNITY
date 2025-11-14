using System.Collections.Generic;

public class SeqEnd : Action
{
    private StandInstance stand;
    private List<ItemInstance> items;
    private ItemInstance outputItemIfStandFull;
    private List<StandInstance> standsReserved;

    public SeqEnd(StandInstance stand, List<ItemInstance> items, ItemInstance outputItemIfStandFull, List<StandInstance> standsReserved = null)
    {
        this.stand = stand;
        this.items = items;
        this.outputItemIfStandFull = outputItemIfStandFull;
        this.standsReserved = standsReserved;
    }
    public void Execute(BaseAgent agent)
    {
        stand.reserved = false;
        foreach (ItemInstance item in items)
        {
            item.UnReserve();
        }
        if (standsReserved != null)
            foreach (StandInstance s in standsReserved)
                s.UnReserve();
        outputItemIfStandFull?.UnReserve();
    }

    public bool IsDone(BaseAgent agent)
    {
        return true;
    }
}