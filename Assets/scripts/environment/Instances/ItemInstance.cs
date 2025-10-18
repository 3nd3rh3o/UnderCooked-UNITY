

public class ItemInstance
{
    public Item ItemData;
    private bool reserved = false;

    public ItemInstance(Item item)
    {
        this.ItemData = item;
    }
    // retourne false si deja reservé
    public bool Reserve()
    {
        if (!reserved)
        {
            reserved = true;
            return true;
        }
        return false;
    }

    public bool IsReserved()
    {
        return reserved;
    }

    public void UnReserve()
    {
        reserved = false;
    }
}
