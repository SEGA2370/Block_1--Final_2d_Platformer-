using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool invulnerable;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    public bool IsInvulnerable => invulnerable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject.");
        }
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on this GameObject.");
        }
    }

    public void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayDeathAnimation(System.Action onDeathAnimationComplete, float deathDelay = 0f)
    {
        if (!dead)
        {
            dead = true;
            if (gameObject.CompareTag("Player"))
            {
                animator.SetBool("Grounded", true);
            }
            animator.SetTrigger("Die");
            StartCoroutine(WaitForDeathAnimation(onDeathAnimationComplete, deathDelay));
        }
    }

    private IEnumerator WaitForDeathAnimation(System.Action onDeathAnimationComplete, float deathDelay)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(deathDelay);
        onDeathAnimationComplete?.Invoke();
    }

    public void ResetDeathAnimation()
    {
        dead = false;
        animator.ResetTrigger("Die");
        animator.Play("Idle_Player");
    }

    public void StartInvulnerability()
    {
        if (!invulnerable)
        {
            StartCoroutine(Invulnerability());
        }
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        // Assume layer indices for player and enemy are 10 and 11
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
