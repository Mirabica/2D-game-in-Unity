using UnityEngine;

public class ZigZagChaseNode : Node
{
    private Transform   _enemy, _player;
    private Rigidbody2D _rb;
    private float       _hSpeed, _amplitude, _frequency, _baseY, _startTime;

    public ZigZagChaseNode(
        Transform   enemy,
        Transform   player,
        Rigidbody2D rb,
        float       horizontalSpeed,
        float       amplitude,
        float       frequency,
        float       flightHeight  // new
    ) {
        _enemy      = enemy;
        _player     = player;
        _rb         = rb;
        _hSpeed     = horizontalSpeed;
        _amplitude  = amplitude;
        _frequency  = frequency;
        _baseY      = flightHeight;     // store the desired center Y
        _startTime  = Time.time;
    }

    public override NodeState Evaluate()
    {
        if (_player == null || _enemy == null || _rb == null)
        {
            _state = NodeState.Failure;
            return _state;
        }

        // Horizontal chase
        float dirX = Mathf.Sign(_player.position.x - _enemy.position.x);
        float vx   = dirX * _hSpeed;

        // Vertical zig-zag around _baseY
        float t     = Time.time - _startTime;
        float offset= _amplitude * Mathf.Sin(2f * Mathf.PI * _frequency * t);
        float desiredY = _baseY + offset;

        // Move physically using MovePosition
        Vector2 next = new Vector2(
            _rb.position.x + vx * Time.deltaTime,
            desiredY
        );
        _rb.MovePosition(next);

        // Flip sprite if needed
        if ((_enemy.localScale.x > 0 && dirX < 0) ||
            (_enemy.localScale.x < 0 && dirX > 0))
        {
            Vector3 s = _enemy.localScale; s.x *= -1; _enemy.localScale = s;
        }

        _state = NodeState.Running;
        return _state;
    }
}
