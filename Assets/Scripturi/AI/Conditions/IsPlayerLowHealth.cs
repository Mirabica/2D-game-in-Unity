using UnityEngine;

public class IsPlayerLowHealth : Node
{
    private HealthController _healthController;
    private int _threshold;

    public IsPlayerLowHealth(HealthController healthController, int threshold = 1)
    {
        _healthController = healthController;
        _threshold = threshold;
    }

    public override NodeState Evaluate()
{
    if (_healthController == null)
    {
        Debug.LogWarning("⚠️ IsPlayerLowHealth: HealthController is null!");
        _state = NodeState.Failure;
        return _state;
    }

    Debug.Log("Player health is " + _healthController.playerHealth);

    if (_healthController.playerHealth <= _threshold)
    {
        Debug.Log("✅ Player is low on health → enemy will chase faster!");
        _state = NodeState.Success;
        return _state;
    }

    _state = NodeState.Failure;
    return _state;
}
}
