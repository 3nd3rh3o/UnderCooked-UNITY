using static TaskTree.Node;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActionSeq
{
    public List<Action> actions;

    public static bool CanBuild(TaskTree.Node node, Environment env)
    {
        // node.recipe.stand; => le stand
        // node.recipe.inputs; => les items a prendre
        // node.recipe.result; => l'item a produire

        if (
            env.stands == null || env.stands.Count == 0 ||
            ((env.itemInWorld == null || env.itemInWorld.Count == 0) &&
            (env.itemsOnStands == null || env.itemsOnStands.Count == 0)) ||
            node == null
        )
            return false;
        foreach (StandInstance stand in env.stands)
        {
            if (stand.standData == node.recipe.stand && !stand.reserved)
            {
                int countItemsFound = 0;
                foreach (Tuple<Transform, ItemInstance> item in env.itemInWorld)
                {
                    if (node.recipe.inputs.Contains(item.Item2.ItemData) && !item.Item2.IsReserved())
                    {
                        countItemsFound++;
                    }
                }
                foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
                {
                    if (node.recipe.inputs.Contains(item.Item2.ItemData) && !item.Item2.IsReserved())
                    {
                        countItemsFound++;
                    }
                }
                if (countItemsFound >= node.recipe.inputs.Count)
                {
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    public ActionSeq(TaskTree.Node node, Environment env)
    {
        actions = new List<Action>();

        // ETAPE 1 : Dire au monde => Je veux aller sur tel stand.
        // ETAPE 2 : Pour chaque item(Dabbord sur une table ou par terre, puis dans les slots out des stands), dire au monde => Je vais les prendres.
        // ETAPE 3 : Pour chacun, ajouter action movetoitem + pickupitem + moveToStand + putItemInStand.
        // ETAPE 4 : then, UseStand.
        // ETAPE 4 BIS : Si goal, DeliverGoal.
        // ETAPE 5 : DestroyNode.
        StandInstance stand = null;
        foreach (StandInstance s in env.stands)
        {
            if (s.standData == node.recipe.stand && !s.reserved)
            {
                stand = s;
                break;
            }
        }
        // (item, inWorld?, standContaining)
        List<Tuple<ItemInstance, bool, StandInstance>> itemsToGet = new List<Tuple<ItemInstance, bool, StandInstance>>();
        foreach (Item input in node.recipe.inputs)
        {
            bool found = false;
            foreach (Tuple<Transform, ItemInstance> item in env.itemInWorld)
            {
                if (item.Item2.ItemData == input && !item.Item2.IsReserved())
                {
                    itemsToGet.Add(new(item.Item2, true, null));
                    found = true;
                    break;
                }
            }
            if (found) continue;
            foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
            {
                if (item.Item2.ItemData == input && !item.Item2.IsReserved())
                {
                    itemsToGet.Add(new(item.Item2, false, item.Item3));
                    break;
                }
            }
        }

        // ETAPE 1 : creer les actions
        for (int i = 0; i < itemsToGet.Count; i++)
        {
            Tuple<ItemInstance, bool, StandInstance> itemTuple = itemsToGet[i];
            ItemInstance item = itemTuple.Item1;
            bool inWorld = itemTuple.Item2;
            StandInstance itemStand = inWorld ? null : itemTuple.Item3;
            item.Reserve();
            if (inWorld)
            {
                actions.Add(new MoveToItem(item, env));
                actions.Add(new TakeItemInWorld(item, env));
            }
            else
            {
                actions.Add(new MoveToStand(itemStand, env));
                actions.Add(new TakeItemInStand(item, itemStand, env));
            }
            actions.Add(new MoveToStand(stand, env));
            actions.Add(new DropItemInStand(item, stand, env));
        }
        actions.Add(new UseStand(stand, node.recipe, env));


        actions.Add(new SeqEnd(stand, itemsToGet.ConvertAll(t => t.Item1)));
    }
    

    public void Execute(BaseAgent agent)
    {
        if (actions == null || actions.Count == 0)
            return;
        Action current = actions[0];
        current.Execute(agent);
        // Si l'action est finie, on la retire de la liste.
        if (current.IsDone())
        {
            actions.RemoveAt(0);
        }
    }
}