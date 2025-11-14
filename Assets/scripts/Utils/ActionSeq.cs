using static TaskTree.Node;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ActionSeq
{
    public List<Action> actions;
    // TODO : Si il manque un item, ajouter une node a l'arbre des taches.
    public static bool CanBuild(TaskTree.Node node, Environment env)
    {
        // node.recipe.stand; => le stand
        // node.recipe.inputs; => les items a prendre
        // node.recipe.result; => l'item a produire
        // Si le stand a un truc dans la sortie, le sortir.

        if (
            env.stands == null ||
            ((env.itemInWorld == null) &&
            (env.itemsOnStands == null))
        )
            return false;
        // Check si goal dans le monde, si oui, annuler la sequence et creer une sequence de livraison.
        foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
        {
            if (!(item.Item2.IsReserved()) && IsGoalItem(item.Item2, env))
            {
                return true;
            }
        }

        if (node == null || node.recipe == null)
            return false;
        foreach (StandInstance stand in env.stands)
        {
            if (stand.standData == node.recipe.stand && !stand.reserved)
            {
                // TODO : recuperer les items pas trouvés.
                int countItemsFound = 0;
                List<Item> missingItems = new List<Item>();
                missingItems.AddRange(node.recipe.inputs);

                foreach (Tuple<Transform, ItemInstance> item in env.itemInWorld)
                {
                    if (node.recipe.inputs.Contains(item.Item2.ItemData) && !item.Item2.IsReserved())
                    {
                        countItemsFound++;
                        missingItems.Remove(item.Item2.ItemData);
                    }
                }
                foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
                {
                    if (node.recipe.inputs.Contains(item.Item2.ItemData) && !item.Item2.IsReserved())
                    {
                        countItemsFound++;
                        missingItems.Remove(item.Item2.ItemData);
                    }
                }
                if (countItemsFound >= node.recipe.inputs.Count)
                {
                    return true;
                }
                // Si il manque un item, ajouter une node a l'arbre des taches.
                foreach (Item missingItem in missingItems)
                {
                    Recipe r = TaskTree.getRecipeProducing(missingItem, env.knownRecipes);
                    node.nextNodes.Add(new TaskTree.Node(r, env.knownRecipes));
                }
                return false;
            }
        }
        return false;
    }

    private static bool IsGoalItem(ItemInstance item, Environment env)
    {
        foreach (Goal goal in env.goals)
        {
            if (goal.item == item.ItemData)
                return true;
        }
        return false;
    }
    public ActionSeq(TaskTree.Node node, Environment env)
    {
        actions = new List<Action>();
        // Check si goal dans le monde, si oui, annuler la sequence et creer une sequence de livraison.
        foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
        {
            if (!(item.Item2.IsReserved()) && IsGoalItem(item.Item2, env))
            {
                // Annuler la construction de la sequence et remplacer par une sequence de livraison de goal.
                actions = new List<Action>();
                actions.Add(new MoveToStand(item.Item3, item.Item1, env));
                actions.Add(new TakeItemInStand(item.Item2, item.Item3, env));
                actions.Add(new MoveToStand(env.deliveryStands[0], env.deliveryStands[0].transform, env));
                actions.Add(new DropItemInStand(item.Item2, env.deliveryStands[0], env));
                actions.Add(new SeqEnd(env.deliveryStands[0], new List<ItemInstance> { item.Item2 }, null, new List<StandInstance> { item.Item3 }));
                return;

            }
        }
        // ETAPE 1 : Dire au monde => Je veux aller sur tel stand.
        // ETAPE 2 : Pour chaque item(Dabbord sur une table ou par terre, puis dans les slots out des stands), dire au monde => Je vais les prendres.
        // ETAPE 3 : Pour chacun, ajouter action movetoitem + pickupitem + moveToStand + putItemInStand.
        // ETAPE 4 : then, UseStand.
        // ETAPE 4 BIS : Si goal, DeliverGoal.
        // ETAPE 5 : DestroyNode.
        StandInstance stand = null;
        Transform standTransform = null;
        foreach (StandInstance s in env.stands)
        {
            if (s.standData == node.recipe.stand && !s.reserved)
            {
                stand = s;
                standTransform = s.transform;
                break;
            }
        }
        // (item, inWorld?, standContaining)
        List<Tuple<ItemInstance, bool, StandInstance, Transform>> itemsToGet = new List<Tuple<ItemInstance, bool, StandInstance, Transform>>();
        foreach (Item input in node.recipe.inputs)
        {
            bool found = false;
            foreach (Tuple<Transform, ItemInstance> item in env.itemInWorld)
            {
                if (item.Item2.ItemData == input && !item.Item2.IsReserved() && !itemsToGet.Exists(t => t.Item1 == item.Item2))
                {
                    itemsToGet.Add(new(item.Item2, true, null, item.Item1));
                    found = true;
                    break;
                }
            }
            if (found) continue;
            foreach (Tuple<Transform, ItemInstance, StandInstance> item in env.itemsOnStands)
            {
                if (item.Item2.ItemData == input && !item.Item2.IsReserved() && !itemsToGet.Exists(t => t.Item1 == item.Item2))
                {
                    itemsToGet.Add(new(item.Item2, false, item.Item3, item.Item1));
                    break;
                }
            }
        }

        // ETAPE 1 : creer les actions
        // Si stand pas vide, on le vide.
        if (stand.output != null && !stand.standData.isGenerator)
        {
            stand.output.Reserve();
            actions.Add(new MoveToStand(stand, standTransform, env));
            actions.Add(new TakeItemInStand(stand.output, stand, env));
            actions.Add(new DropItemInWorld(stand.output, env));
            actions.Add(new MoveToStand(stand, standTransform, env));
        }

        List<StandInstance> containers = new List<StandInstance>();

        for (int i = 0; i < itemsToGet.Count; i++)
        {
            Tuple<ItemInstance, bool, StandInstance, Transform> itemTuple = itemsToGet[i];
            ItemInstance item = itemTuple.Item1;
            bool inWorld = itemTuple.Item2;
            StandInstance itemStand = inWorld ? null : itemTuple.Item3;
            Transform itemTransform = itemTuple.Item4;
            item.Reserve();
            if (!inWorld)
            {
                itemStand.Reserve();
                containers.Add(itemStand);
            }
            if (inWorld)
            {
                actions.Add(new MoveToItem(item, itemTransform, env));
                actions.Add(new TakeItemInWorld(item, itemTransform, env));
            }
            else
            {
                actions.Add(new MoveToStand(itemStand, itemTransform, env));
                actions.Add(new TakeItemInStand(item, itemStand, env));
            }
            actions.Add(new MoveToStand(stand, standTransform, env));
            actions.Add(new DropItemInStand(item, stand, env));
        }
        actions.Add(new MoveToStand(stand, standTransform, env));
        stand.Reserve();

        // Si le stand est un container, on le met sur le superStand.
        if (stand.standData.isContainer)
        {
            actions.Add(new TakeContainer(stand, standTransform, env));
            StandInstance superStand = env.stands.Find(s => s.standData == stand.standData.containerFor);
            actions.Add(new MoveToStand(superStand, superStand.transform, env));
            actions.Add(new PutContainer(stand, superStand, env));
            actions.Add(new UseStand(stand, node, env));
            actions.Add(new MoveToStand(stand, standTransform, env));
            actions.Add(new TakeContainer(stand, standTransform, env));
            actions.Add(new DropContainer(stand, env));
        }
        else
        {
            actions.Add(new UseStand(stand, node, env));
        }





        actions.Add(new SeqEnd(stand, itemsToGet.ConvertAll(t => t.Item1), stand.output, containers));
    }

    public void Execute(BaseAgent agent)
    {
        if (actions == null || actions.Count == 0)
            return;
        Action current = actions[0];
        current.Execute(agent);
        // Si l'action est finie, on la retire de la liste.
        if (current.IsDone(agent))
        {
            actions.RemoveAt(0);
        }
    }
}