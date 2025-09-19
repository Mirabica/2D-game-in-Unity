using UnityEngine;

public class ChasePlayer : Node
{
    private Transform _enemy;
    private Transform _player;
    private Rigidbody2D _rb;
    private float _moveSpeed;

    public ChasePlayer(Transform enemy, Transform player, Rigidbody2D rb, float moveSpeed)
    {
        _enemy = enemy;
        _player = player;
        _rb = rb;
        _moveSpeed = moveSpeed;
    }

    public override NodeState Evaluate()
    {
        if (_player == null || _rb == null)
        {
            _state = NodeState.Failure;
            return _state;
        }

        float direction = Mathf.Sign(_player.position.x - _enemy.position.x);
        _rb.velocity = new Vector2(direction * _moveSpeed, _rb.velocity.y);

        // Flip enemy sprite if needed
        if ((_enemy.localScale.x > 0 && direction < 0) || (_enemy.localScale.x < 0 && direction > 0))
        {
            Vector3 scale = _enemy.localScale;
            scale.x *= -1;
            _enemy.localScale = scale;
        }

        _state = NodeState.Running;
        return _state;
    }
}
