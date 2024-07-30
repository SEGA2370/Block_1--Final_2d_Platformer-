using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] private bool isPlayer;
    public bool IsPlayer => isPlayer;
    private HealthBar healthBar;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health component not found on this GameObject.");
        }
    }

    private void Start()
    {
        if (isPlayer)
        {
            healthBar = FindObjectOfType<HealthBar>();
            if (healthBar != null && health != null)
            {
                healthBar.SetMaxHealth(health.StartingHealth);
                healthBar.SetHealth(health.currentHealth);
                health.HealthChanged.AddListener(UpdateHealthBar);
            }
        }
    }

    private void UpdateHealthBar(float currentHealth)
    {
        if (isPlayer && healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}
