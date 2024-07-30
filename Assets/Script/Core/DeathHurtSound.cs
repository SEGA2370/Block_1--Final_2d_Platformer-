using UnityEngine;

public class DeathHurtSound : MonoBehaviour
{
    [Header("Sound Settings")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    public void PlayDeathSound()
    {
        SoundManager.instance.PlaySound(deathSound);
    }

    public void PlayHurtSound()
    {
        SoundManager.instance.PlaySound(hurtSound);
    }
}
