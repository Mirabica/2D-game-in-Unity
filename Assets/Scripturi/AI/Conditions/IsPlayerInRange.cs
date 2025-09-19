using UnityEngine;

public class IsPlayerInRange : Node
{
    private Transform _enemy;
    private Transform _player;
    private float _detectionRadius;

    public IsPlayerInRange(Transform enemy, Transform player, float radius)
    {
        _enemy = enemy;
        _player = player;
        _detectionRadius = radius;
    }

    public override NodeState Evaluate()
    {
        if (_player == null)
        {
            Debug.LogWarning("IsPlayerInRange: Player reference is missing!");
            _state = NodeState.Failure;
            return _state;
        }

        float distance = Vector2.Distance(_enemy.position, _player.position);

        if (distance <= _detectionRadius)
        {
            _state = NodeState.Success;
            Debug.Log("I see the player!");
            return _state;
            
        }

        _state = NodeState.Failure;
        return _state;
    }
}
