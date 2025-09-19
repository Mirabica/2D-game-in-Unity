using UnityEngine;

public class EnemyDamageController : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private HealthController healthController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Verifică dacă jucătorul vine de jos sau lateral (NU de sus)
            Vector2 contactPoint = collision.GetContact(0).point;
            float yDifference = contactPoint.y - transform.position.y;

            if (yDifference < 0.2f) // <–– a atins de jos sau lateral
            {
                healthController.playerHealth -= damageAmount;
                healthController.UpdateHealth();
            }
        }
    }
}
