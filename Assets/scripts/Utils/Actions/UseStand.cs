public class UseStand : Action
{
    private StandInstance stand;

    public UseStand(StandInstance stand, Recipe recipe, Environment env)
    {
        this.stand = stand;
    }

    public void Execute(BaseAgent agent)
    {

    }

    public bool IsDone()
    {
        throw new System.NotImplementedException();
    }
}