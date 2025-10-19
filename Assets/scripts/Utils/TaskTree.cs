using System;
using System.Collections.Generic;
public class TaskTree
{
    public Item result;
    private Node root;
    // Un noeud de l'arbre.
    public class Node
    {
        // contient recettes et noeuds enfants.
        public Recipe recipe;
        public List<Node> nextNodes; // null ou vide quand feuille.
        // constructeur réccursif.
        public Node(Recipe recipe, List<Recipe> knownRecipes)
        {
            nextNodes = new List<Node>();
            this.recipe = recipe;
            foreach (Item i in recipe.inputs)
            {
                Recipe r = getRecipeProducing(i, knownRecipes);
                nextNodes.Add(new Node(r, knownRecipes));
            }
        }
        // retourne la feuille gauche.
        public Node GetLeafTodo()
        {
            if (nextNodes[0].nextNodes == null || nextNodes[0].nextNodes.Count == 0)
                return nextNodes[0];
            else
                return nextNodes[0].GetLeafTodo();
        }

        public void PopNode(Node node)
        {
            foreach (Node n in nextNodes)
            {
                if (n == node)
                {
                    nextNodes.Remove(n);
                    return;
                }
                else
                {
                    n.PopNode(node);
                }
            }
        }
    }
    
    // Construit l'arborescence de tâches par objet.
    // Attention ! ne prends pas en compte les déplacements !!!
    // Il faudra un script approprié pour gerer les différentes "façon" de fabriquer
    // un produit.
    // Si il y a besoin d'un contenant(poelle) il faut savoir le gerer.
    // Mais coté agent.(Tag sur le stand?)
    public TaskTree(List<Recipe> knownRecipes, Item target)
    {
        result = target;
        Recipe r = getRecipeProducing(target, knownRecipes);
        root = new Node(r, knownRecipes);
    }

    // Retourne la recette qui produit l'objet fourni en paramêtres
    public static Recipe getRecipeProducing(Item target, List<Recipe> knownRecipes)
    {
        foreach (Recipe r in knownRecipes)
        {
            if (r.result == target)
                return r;
        }
        throw new KeyNotFoundException();
    }

    // On fetch une node de l'arbre, et on le retourne. Il sera donné en argument
    // quand on voudra le supprimer de la taskList.
    // (retourne la feuille gauche de l'arbre).
    public Node getLeafTodo()
    {
        if (root == null)
            return null;
        if (root.nextNodes == null || root.nextNodes.Count == 0)
            return root;
        else
            return root.GetLeafTodo();
    }


    // Pop la node de l'arbre.
    public void PopNode(Node node)
    {
        // Si c'est la racine.
        if (node == root)
        {
            root = null;
            return;
        }
        foreach (Node n in root.nextNodes)
        {
            if (n == node)
            {
                root.nextNodes.Remove(n);
                return;
            }
            else
            {
                n.PopNode(node);
            }
        }
    }


    // TODO : AddNode. Si une node a été pop par une autre task, il faut pouvoir en remettre une.
}