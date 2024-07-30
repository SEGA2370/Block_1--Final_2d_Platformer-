using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float StartingHealth => startingHealth; // Expose startingHealth through a property
    public float currentHealth { get; private set; }
    public UnityEvent<float> HealthChanged; // Event to notify when health changes

    private AnimationManager animationManager;
    private bool dead;

    [SerializeField] private HealthBar healthBar; // Reference to HealthBar

    private void Awake()
    {
        currentHealth = startingHealth;

        if (!TryGetComponent<AnimationManager>(out animationManager))
        {
            Debug.LogError("AnimationManager component not found on this GameObject.");
        }

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(startingHealth);
        }
    }

    private void Start()
    {
        HealthChanged.AddListener(UpdateHealthBar);
    }

    private void UpdateHealthBar(float health)
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
    }

    public void TakeDamage(float damage)
    {
        if (animationManager == null || animationManager.IsInvulnerable || dead)
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        HealthChanged?.Invoke(currentHealth); // Notify subscribers of health change

        if (currentHealth > 0)
        {
            animationManager.PlayHurtAnimation();
            animationManager.StartInvulnerability();
            // Optionally call sound manager directly here
        }
        else
        {
            if (!dead)
            {
                dead = true;
                animationManager.PlayDeathAnimation(OnDeathAnimationComplete);
                // Optionally call sound manager and deactivate components here
            }
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        HealthChanged?.Invoke(currentHealth); // Notify subscribers of health change
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        animationManager.ResetDeathAnimation();
        // Optionally call component activation here
    }

    private void OnDeathAnimationComplete()
    {
        // Logic to execute after the death animation
    }
}
