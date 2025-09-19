using UnityEngine;

public class OwlPatrolNode : Node
{
    private Transform   _enemy;
    private Rigidbody2D _rb;
    private float       _speed, _baseY;
    private float[]     _durations = new float[4];
    private int         _phase;
    private float       _timer;

    public OwlPatrolNode(
        Transform   enemy,
        Rigidbody2D rb,
        float       speed,
        float       d0, float d1, float d2, float d3,
        float       flightHeight  // new
    ) {
        _enemy      = enemy;
        _rb         = rb;
        _speed      = speed;
        _durations[0]=d0; _durations[1]=d1;
        _durations[2]=d2; _durations[3]=d3;
        _phase      = 0;
        _timer      = _durations[0];
        _baseY      = flightHeight;
    }

    public override NodeState Evaluate()
    {
        // 1) Horizontal
        int dir = (_phase == 0 || _phase == 3) ? -1 : 1;
        float vx = dir * _speed;

        // Keep Y locked to _baseY
        Vector2 next = new Vector2(
            _rb.position.x + vx * Time.deltaTime,
            _baseY
        );
        _rb.MovePosition(next);

        // Countdown & phase switch
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
            AdvancePhase();

        _state = NodeState.Running;
        return _state;
    }

    private void AdvancePhase()
    {
        _phase = (_phase + 1) % 4;
        _timer = _durations[_phase];
        // Flip facing
        Vector3 s = _enemy.localScale;
        bool movingLeft = (_phase == 0 || _phase == 3);
        s.x = Mathf.Abs(s.x) * (movingLeft ? -1 : 1);
        _enemy.localScale = s;
    }
}
