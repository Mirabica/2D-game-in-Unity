using UnityEngine;

public class JumpChaseNode : Node
{
    private readonly Transform   _frog;
    private readonly Transform   _player;
    private readonly Rigidbody2D _rb;
    private readonly Transform   _groundCheck;
    private readonly LayerMask   _groundLayer;
    private readonly float       _groundRadius;
    private readonly float       _hForce;
    private readonly float       _vForce;
    private          bool        _hasJumped = false;

    public JumpChaseNode(
        Transform    frog,
        Transform    player,
        Rigidbody2D  rb,
        Transform    groundCheck,
        LayerMask    groundLayer,
        float        groundRadius,
        float        horizontalForce,
        float        verticalForce
    ) {
        _frog         = frog;
        _player       = player;
        _rb           = rb;
        _groundCheck  = groundCheck;
        _groundLayer  = groundLayer;
        _groundRadius = groundRadius;
        _hForce       = horizontalForce;
        _vForce       = verticalForce;
    }

    public override NodeState Evaluate()
    {
        if (_player == null || _rb == null || _frog == null)
        {
            _state = NodeState.Failure;
            return _state;
        }

        // 1) Check if on the ground
        bool grounded = Physics2D.OverlapCircle(
            _groundCheck.position, _groundRadius, _groundLayer);

        if (grounded && !_hasJumped)
        {
            // 2) Compute direction to player
            float dir = Mathf.Sign(_player.position.x - _frog.position.x);

            // 3) Launch the leap
            _rb.velocity = Vector2.zero;
            _rb.AddForce(new Vector2(_hForce * dir, _vForce),
                         ForceMode2D.Impulse);
            _hasJumped = true;

            // 4) Flip sprite to match chase direction
            Vector3 s = _frog.localScale;
            s.x = Mathf.Abs(s.x) * (dir < 0 ? -1 : 1);
            _frog.localScale = s;
        }
        else if (_hasJumped && grounded)
        {
            // 5) Reset so we can jump again next time
            _hasJumped = false;
        }

        _state = NodeState.Running;
        return _state;
    }
}
