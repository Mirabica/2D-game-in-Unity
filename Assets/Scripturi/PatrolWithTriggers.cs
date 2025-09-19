using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrolWithTriggers : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 1.5f;
    public float jumpForce = 8f;

    [HideInInspector] public int direction = -1;  // start moving left

    Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Always move horizontally at patrol speed
        _rb.velocity = new Vector2(direction * speed, _rb.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1) TURNPOINT: reverse direction
        if (other.CompareTag("TurnPoint"))
        {
            Flip();
            return;
        }

        // 2) JUMPTRIGGER: only if I’m moving *toward* this trigger
        if (other.CompareTag("JumpTrigger"))
        {
            // assume the JumpTrigger collider sits *in front* of the obstacle
            // so only jump if I'm actually moving into it
            if ((direction < 0 && other.transform.position.x < transform.position.x) ||
                (direction > 0 && other.transform.position.x > transform.position.x))
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }
        }
    }

    void Flip()
    {
        direction *= -1;
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
