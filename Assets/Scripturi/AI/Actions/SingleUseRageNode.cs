using UnityEngine;

public class SingleUseRageNode : Node
{
    private readonly Transform        _frog;
    private readonly Transform        _player;
    private readonly HealthController _hc;
    private readonly Rigidbody2D      _rb;
    private readonly float            _radius;
    private readonly float            _jumpForce;
    private readonly float            _jumpInterval;
    private readonly float            _duration;

    private bool   _started        = false;
    private float  _startTime      = 0f;
    private float  _nextJumpTime   = 0f;

    /// <param name="frog">Frog transform</param>
    /// <param name="player">Player transform</param>
    /// <param name="hc">HealthController</param>
    /// <param name="rb">Frog’s Rigidbody2D</param>
    /// <param name="radius">How close (< radius) to trigger</param>
    /// <param name="jumpForce">Upward impulse each jump</param>
    /// <param name="jumpInterval">Seconds between jumps</param>
    /// <param name="duration">How long (s) the rage lasts</param>
    public SingleUseRageNode(
        Transform        frog,
        Transform        player,
        HealthController hc,
        Rigidbody2D      rb,
        float            radius,
        float            jumpForce,
        float            jumpInterval,
        float            duration
    ) {
        _frog          = frog;
        _player        = player;
        _hc            = hc;
        _rb            = rb;
        _radius        = radius;
        _jumpForce     = jumpForce;
        _jumpInterval  = jumpInterval;
        _duration      = duration;
    }

    public override NodeState Evaluate()
    {
        // 1) Haven’t started yet → check trigger condition
        if (!_started)
        {
            if (_hc.playerHealth <= 1 &&
                Vector2.Distance(_frog.position, _player.position) <= _radius)
            {
                _started      = true;
                _startTime    = Time.time;
                _nextJumpTime = Time.time; // immediate first jump
                _state        = NodeState.Running;
            }
            else
            {
                _state = NodeState.Failure;
            }
            return _state;
        }

        // 2) Already started → check if we’re still in the rage window
        float elapsed = Time.time - _startTime;
        if (elapsed <= _duration)
        {
            // time to jump?
            if (Time.time >= _nextJumpTime)
            {
                _rb.velocity = Vector2.zero;
                _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _nextJumpTime = Time.time + _jumpInterval;
            }
            _state = NodeState.Running;
            return _state;
        }

        // 3) Rage duration complete → succeed once and never run again
        _state = NodeState.Failure;
        return _state;
    }
}
