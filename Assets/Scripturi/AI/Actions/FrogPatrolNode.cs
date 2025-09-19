// Assets/Scripts/AI/Actions/FrogPatrolNode.cs
using UnityEngine;

public class FrogPatrolNode : Node
{
    private readonly Transform   _frog;
    private readonly Rigidbody2D _rb;
    private readonly float       _hForce;
    private readonly float       _vForce;
    private readonly float       _jumpInterval;
    private readonly float[]     _phaseDurations;   // duration for phase 0 and 1
    private readonly int         _initialDir;       // +1 or –1
    private int                  _phase;            // 0 or 1
    private float                _phaseTimer;
    private float                _nextJumpTime;

    /// <param name="frog">Your frog transform</param>
    /// <param name="rb">Its Rigidbody2D</param>
    /// <param name="hForce">Horizontal jump force</param>
    /// <param name="vForce">Vertical jump force</param>
    /// <param name="jumpInterval">Seconds between jumps</param>
    /// <param name="durationLeft">Seconds to patrol left</param>
    /// <param name="durationRight">Seconds to patrol right</param>
    /// <param name="initialDirection">-1 to start left, +1 to start right</param>
    public FrogPatrolNode(
        Transform frog,
        Rigidbody2D rb,
        float       hForce,
        float       vForce,
        float       jumpInterval,
        float       durationLeft,
        float       durationRight,
        int         initialDirection = -1
    ) {
        _frog            = frog;
        _rb              = rb;
        _hForce          = hForce;
        _vForce          = vForce;
        _jumpInterval    = jumpInterval;
        _initialDir      = initialDirection < 0 ? -1 : 1;
        // Order durations so phase0 matches initialDir
        if (_initialDir < 0)
            _phaseDurations = new float[]{ durationLeft, durationRight };
        else
            _phaseDurations = new float[]{ durationRight, durationLeft };

        _phase         = 0;
        _phaseTimer    = _phaseDurations[_phase];
        _nextJumpTime  = Time.time + _jumpInterval;
    }

    public override NodeState Evaluate()
    {
        if (_rb == null)
        {
            _state = NodeState.Failure;
            return _state;
        }

        float dt = Time.deltaTime;

        // 1) Advance phase timer, flip if needed
        _phaseTimer -= dt;
        if (_phaseTimer <= 0f)
        {
            _phase = 1 - _phase;  // toggle 0↔1
            _phaseTimer = _phaseDurations[_phase];
        }

        // 2) Jump when it's time
        if (Time.time >= _nextJumpTime)
        {
            // reset any y‐velocity
            _rb.velocity = Vector2.zero;

            // compute direction: phase0 uses initialDir, phase1 flips
            int dir = (_phase == 0 ? _initialDir : -_initialDir);

            // apply impulse
            _rb.AddForce(
                new Vector2(_hForce * dir, _vForce),
                ForceMode2D.Impulse
            );

            // flip sprite
            Vector3 s = _frog.localScale;
            s.x = Mathf.Abs(s.x) * (dir < 0 ? -1 : 1);
            _frog.localScale = s;

            // schedule next jump
            _nextJumpTime = Time.time + _jumpInterval;
        }

        _state = NodeState.Running;
        return _state;
    }
}
