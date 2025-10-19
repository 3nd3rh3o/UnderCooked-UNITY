using System.Collections.Generic;

public class SeqEnd : Action
{
    private StandInstance stand;
    private List<ItemInstance> items;
    private ItemInstance outputItemIfStandFull;

    public SeqEnd(StandInstance stand, List<ItemInstance> items, ItemInstance outputItemIfStandFull)
    {
        this.stand = stand;
        this.items = items;
        this.outputItemIfStandFull = outputItemIfStandFull;
    }
    public void Execute(BaseAgent agent)
    {
        stand.reserved = false;
        foreach (ItemInstance item in items)
        {
            item.UnReserve();
        }
        outputItemIfStandFull?.UnReserve();
    }

    public bool IsDone(BaseAgent agent)
    {
        return true;
    }
}