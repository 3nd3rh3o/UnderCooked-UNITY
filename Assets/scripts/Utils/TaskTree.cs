using System.Collections.Generic;
using System.Linq;

public class TaskTree
{
    public Item result;
    private Node root;
    private class Node
    {
        public Recipe recipe;
        public List<Node> nextNodes;
        public Node(Recipe recipe, List<Recipe> knownRecipes)
        {
            nextNodes = new List<Node>();
            foreach (Item i in recipe.inputs)
            {
                Recipe r = getRecipeProducing(i, knownRecipes);
                nextNodes.Add(new Node(r, knownRecipes));
            }
        }
    }
    public TaskTree(List<Recipe> knownRecipes, Item target)
    {
        result = target;
        Recipe r = getRecipeProducing(target, knownRecipes);
        root = new Node(r, knownRecipes);
    }


    protected static Recipe getRecipeProducing(Item target, List<Recipe> knownRecipes)
    {
        foreach (Recipe r in knownRecipes)
        {
            if (r.result == target)
                return r;
        }
        throw new KeyNotFoundException();
    }

    public ref Recipe getLeafTodo()
    {
        
    }
}