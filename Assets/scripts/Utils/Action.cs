public interface Action
{
    public void Execute(BaseAgent agent);
    public bool IsDone();
}