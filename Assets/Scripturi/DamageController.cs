using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private int FallDamage;
    [SerializeField] private HealthController _healthController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Damage(collision.gameObject);
        }
    }

    void Damage(GameObject player)
    {
        _healthController.playerHealth -= FallDamage;
        _healthController.UpdateHealth();

    }

}
