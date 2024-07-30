using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health healthComponent = collision.GetComponent<Health>();
        PlayerSettings playerSettings = collision.GetComponent<PlayerSettings>();

        if (healthComponent != null && playerSettings != null && playerSettings.IsPlayer)
        {
            SoundManager.instance.PlaySound(pickupSound);
            healthComponent.AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
