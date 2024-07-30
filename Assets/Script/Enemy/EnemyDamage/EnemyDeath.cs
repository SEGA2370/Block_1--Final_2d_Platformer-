using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private Health health; // Reference to the Health component
    private AnimationManager animationManager;
    private bool dead;

    [Header("Death Delay")]
    [SerializeField] private float deathDelay = 1f; // Adjust this value as needed

    private void Awake()
    {
        if (health == null)
        {
            return;
        }

        animationManager = GetComponent<AnimationManager>();
        if (animationManager == null)
        {
            Debug.LogError("AnimationManager component not found on this GameObject.");
        }

        // Subscribe to the Health component's HealthChanged event
        health.HealthChanged.AddListener(OnHealthChanged);
    }

    private void OnHealthChanged(float currentHealth)
    {
        if (currentHealth <= 0)
        {
            if (!dead)
            {
                dead = true;
                animationManager.PlayDeathAnimation(() =>
                {
                    DestroyObjectAndParentWithDelay(deathDelay);
                }, deathDelay);
            }
        }
        else
        {
            animationManager.PlayHurtAnimation();
        }
    }

    private void DestroyObjectAndParentWithDelay(float delay)
    {
        Invoke("DestroyObjectAndParent", delay);
    }

    private void DestroyObjectAndParent()
    {
        // Destroy the object and its parent
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject); // Destroy the parent object
        }
        Destroy(gameObject); // Destroy the current object
    }
}
