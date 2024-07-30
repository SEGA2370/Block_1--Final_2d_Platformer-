using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        if (!TryGetComponent<AudioSource>(out soundSource))
        {
            Debug.LogError("AudioSource component not found on this GameObject.");
        }

        if (!transform.GetChild(0).TryGetComponent<AudioSource>(out musicSource))
        {
            Debug.LogError("AudioSource component not found on the child GameObject.");
        }

        // Dont Delete Object if we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Destroy Duplicate gameobjects
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        // Assign Initial Volume
        ChangeSoundVolume(0);
        ChangeMusicVolume(0);
    }

    public void PlaySound(AudioClip _sound)
    {
        soundSource.PlayOneShot(_sound);
    }

    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", _change, soundSource);
    }

    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.4f, "musicVolume", _change, musicSource);
    }

    private void ChangeSourceVolume(float baseVolume, string volumeName, float change, AudioSource source)
    {
        // Get Initial of volume and change it
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        // check the Max And Min Value
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        // Assign Final Value
        float finalVolume = currentVolume * baseVolume;
        source.volume = finalVolume;

        // Save Player VolumeSetting
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}
