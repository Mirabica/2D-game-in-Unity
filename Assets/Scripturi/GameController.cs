using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{   
    private Vector2 startPos;
    private bool isDead = false;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    public float deathJumpForce = 5f;
    public float respawnDelay = 1.5f;

    private void Start()
    {   

        startPos = transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>(); // Replace with your player's movement script name
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && collision.CompareTag("Obstacle"))
        {
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
{
    isDead = true;

    // Disable player's controls
    if (playerMovement != null)
        playerMovement.enabled = false;

    // Disable the collider temporarily
    Collider2D playerCollider = GetComponent<Collider2D>();
    if (playerCollider != null)
        playerCollider.enabled = false;

    // Trigger death animation
    if (animator != null)
        animator.SetTrigger("death");

    // Mario-like jump (increase this value as needed)
    if (rb != null)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * deathJumpForce, ForceMode2D.Impulse);
    }

    // Wait for the animation and jump to complete
    yield return new WaitForSeconds(respawnDelay);

    Respawn();

    // Re-enable the collider after respawn
    if (playerCollider != null)
        playerCollider.enabled = true;
}


    void Respawn()
    {
        transform.position = startPos;

        // Re-enable player's controls
        if (playerMovement != null)
            playerMovement.enabled = true;

        isDead = false;

        // Optionally, reset animation state
        if (animator != null)
            animator.SetTrigger("respawn");
    }
}
