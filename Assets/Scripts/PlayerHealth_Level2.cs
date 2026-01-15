using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    private CharacterController controller;

    void Start()
    {
        // Start at full health.
        currentHealth = maxHealth;

        // Set up the health bar UI if assigned.
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        controller = GetComponent<CharacterController>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died. Respawning at the beginning of the maze.");

        // Reset health
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Use the ParkourRespawn script to respawn at the StartPoint.
        ParkourRespawn respawn = GetComponent<ParkourRespawn>();
        if (respawn != null)
        {
            respawn.RespawnToStart();
        }
        else
        {
            // Fallback: directly reposition the player.
            transform.position = ParkourRespawn.StartPoint;
            transform.rotation = Quaternion.identity;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }
}