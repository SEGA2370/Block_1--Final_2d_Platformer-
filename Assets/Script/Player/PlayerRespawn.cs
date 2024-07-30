using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkPointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private AnimationManager animationManager;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        animationManager = GetComponent<AnimationManager>();
        uiManager = FindObjectOfType<UIManager>();

        if (playerHealth == null)
        {
            Debug.LogError("Health component not found on this GameObject.");
        }
        if (animationManager == null)
        {
            Debug.LogError("AnimationManager component not found on this GameObject.");
        }
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found in the scene.");
        }
    }

    public void CheckRespawn()
    {
        if (currentCheckpoint == null)
        {
            Debug.Log("No checkpoint set. Triggering game over.");
            uiManager.GameOver();
            return;
        }

        Debug.Log("Checkpoint found. Starting respawn.");
        StartCoroutine(RespawnAfterDelay(1f)); // Adjust delay to match animation length
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        Debug.Log("Starting respawn animation.");
        yield return new WaitForSeconds(delay);

        if (currentCheckpoint != null)
        {
            Debug.Log("Respawning player at checkpoint: " + currentCheckpoint.position);
            transform.position = currentCheckpoint.position;
            playerHealth.Respawn();
            Debug.Log("Respawn complete.");
        }
        else
        {
            Debug.LogError("Current checkpoint is null. Unable to respawn.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if (currentCheckpoint == null || currentCheckpoint != collision.transform)
            {
                currentCheckpoint = collision.transform;
                Debug.Log("Checkpoint set at position: " + currentCheckpoint.position);
                SoundManager.instance.PlaySound(checkPointSound);
                collision.GetComponent<Collider2D>().enabled = false;
                collision.GetComponent<Animator>().SetTrigger("Appear");
            }
        }
    }
}
