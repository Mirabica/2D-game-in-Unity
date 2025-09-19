public enum NodeState
{
    Running,
    Success,
    Failure
}

public abstract class Node
{
    protected NodeState _state;
    public NodeState State => _state;

    public abstract NodeState Evaluate();
}
