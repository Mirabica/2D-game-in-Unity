
using UnityEngine;

public class PeriodicJumpNode : Node
{
    private readonly Rigidbody2D _rb;
    private readonly float       _jumpForce;
    private readonly float       _interval;
    private float                _nextJumpTime;

    public PeriodicJumpNode(Rigidbody2D rb, float jumpForce, float interval)
    {
        _rb           = rb;
        _jumpForce    = jumpForce;
        _interval     = interval;
        _nextJumpTime = Time.time + _interval;
    }

    public override NodeState Evaluate()
    {
        if (_rb == null)
        {
            _state = NodeState.Failure;
            return _state;
        }

        if (Time.time < _nextJumpTime)
        {
            // Not time yet → fail so we fall back to chase
            _state = NodeState.Failure;
            return _state;
        }

        // Time to jump!
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

        // Schedule next
        _nextJumpTime = Time.time + _interval;

        _state = NodeState.Success;
        return _state;
    }
}
