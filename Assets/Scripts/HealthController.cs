using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private HealthIndicator healthIndicator;

    public float maxHealth = 100;
    public float currentHealth = 0;

    private RagdollController ragdoll;

    private void Awake()
    {
        ragdoll = GetComponent<RagdollController>();
    }

    private void Start()
    {
        healthIndicator = FindAnyObjectByType<HealthIndicator>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= maxHealth && currentHealth != 0)
        {
            currentHealth -= damage;
        }

        if (currentHealth ==0)
        {
            if (ragdoll != null)
                ragdoll.ActivateRagdoll();
            Destroy(gameObject, 3);
        }

        if (CompareTag("Player"))
            healthIndicator.setHealth(currentHealth);
    }
}
