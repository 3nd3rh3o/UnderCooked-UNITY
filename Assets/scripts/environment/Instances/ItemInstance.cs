

using JetBrains.Annotations;
using UnityEngine;

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

    public void OnPickupFromStand(BaseAgent agent)
    {
        GameObject go = Object.Instantiate(ItemData.ItemGOPrefab);
        go.transform.localPosition = Vector3.forward * 0.55f;
        go.transform.SetParent(agent.transform, false);
    }
    
    public void OnDropInStand(BaseAgent agent)
    {
        GameObject go = agent.transform.GetChild(0).gameObject;
        MonoBehaviour.Destroy(go);
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
