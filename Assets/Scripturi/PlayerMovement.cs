using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity = 9.81f; 

    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;

    public GhindaManager gm;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.2484116f, 0.2406476f, 1f);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.2484116f, 0.2406476f, 1f);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            Jump();

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        float jumpVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        body.velocity = new Vector2(body.velocity.x, jumpVelocity);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ghinda"))
        {
         Destroy(other.gameObject);
         gm.ghindaCount++;
        }
         
    }
    
}
