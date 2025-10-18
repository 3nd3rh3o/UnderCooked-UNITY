public class UseStand : Action
{
    private StandInstance stand;
    private TaskTree.Node node;

    public UseStand(StandInstance stand, TaskTree.Node node, Environment env)
    {
        this.stand = stand;
        this.node = node;
    }

    public void Execute(BaseAgent agent)
    {
        if (stand.processingTimer == -1f)
        {
            stand.StartProcessing(node);
        }
        else
        {
            stand.UpdateProcessing(node.recipe);
        }
    }

    public bool IsDone(BaseAgent agent)
    {
        if (stand.standData.isGenerator)
            return true;
        else
            throw new System.NotImplementedException();        
    }
}