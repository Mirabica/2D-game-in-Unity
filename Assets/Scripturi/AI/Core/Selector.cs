using System.Collections.Generic;

public class Selector : Node
{
    private readonly List<Node> _nodes;

    public Selector(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in _nodes)
        {
            var result = node.Evaluate();

            if (result == NodeState.Success)
            {
                _state = NodeState.Success;
                return _state;
            }

            if (result == NodeState.Running)
            {
                _state = NodeState.Running;
                return _state;
            }
        }

        _state = NodeState.Failure;
        return _state;
    }
}
