using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]

    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;

    private Animator animator;
    private SpriteRenderer SpriteRenderer;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; //When the trap gets triggered
    private bool active; //When the trap is active and can hurt player

    private Health playerHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered)
            {
                //trigger firetrap
                StartCoroutine(ActivateFiretrap());
            }
            if (active)
            {
                collision.GetComponent<Health>().TakeDamage(damage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerHealth = null;
        }
    }

    private IEnumerator ActivateFiretrap()
    {
        // changes sprite color to RED to notify the player and the trigger trap
        triggered = true;
        SpriteRenderer.color = Color.red; 

        // Wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(firetrapSound);
        SpriteRenderer.color = Color.white; // changes sprite color back to normal for better understanding
        active = true;
        animator.SetBool("Activated", true);

        // Wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        animator.SetBool("Activated", false);
    }
}
