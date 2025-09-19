using System.Collections.Generic;

public class Sequence : Node
{
    private readonly List<Node> _nodes;

    public Sequence(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool anyRunning = false;

        foreach (var node in _nodes)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    _state = NodeState.Failure;
                    return _state;
                case NodeState.Running:
                    anyRunning = true;
                    break;
            }
        }

        _state = anyRunning ? NodeState.Running : NodeState.Success;
        return _state;
    }
}
