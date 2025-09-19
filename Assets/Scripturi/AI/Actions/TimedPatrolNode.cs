using UnityEngine;

public class TimedPatrolNode : Node
{
    private Transform    _enemy;
    private Rigidbody2D  _rb;
    private float        _moveSpeed;
    private float[]      _durations = new float[4];
    private int          _phase;
    private float        _timer;

    /// <summary>
    /// durationLeft1  : phase 0 (move left)
    /// durationRight1 : phase 1 (move right)
    /// durationRight2 : phase 2 (move right)
    /// durationLeft2  : phase 3 (move left)
    /// </summary>
    public TimedPatrolNode(
        Transform enemy,
        Rigidbody2D rb,
        float moveSpeed,
        float durationLeft1,
        float durationRight1,
        float durationRight2,
        float durationLeft2
    ) {
        _enemy      = enemy;
        _rb         = rb;
        _moveSpeed  = moveSpeed;
        _durations[0] = durationLeft1;
        _durations[1] = durationRight1;
        _durations[2] = durationRight2;
        _durations[3] = durationLeft2;
        _phase      = 0;
        _timer      = _durations[0];
    }

    public override NodeState Evaluate()
    {
        // 1) Determine direction for this phase
        int dirSign = (_phase == 0 || _phase == 3) ? -1 : 1;
        _rb.velocity = new Vector2(dirSign * _moveSpeed, _rb.velocity.y);

        // 2) Countdown
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            AdvancePhase();

        _state = NodeState.Running;
        return _state;
    }

    private void AdvancePhase()
    {
        // cycle phases 0→1→2→3→0…
        _phase = (_phase + 1) % 4;
        _timer = _durations[_phase];

        // flip sprite to face new direction
        Vector3 s = _enemy.localScale;
        bool movingLeft = (_phase == 0 || _phase == 3);
        s.x = Mathf.Abs(s.x) * (movingLeft ? -1 : 1);
        _enemy.localScale = s;
    }
}
