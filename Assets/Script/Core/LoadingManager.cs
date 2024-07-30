using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private AudioClip finish;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlaySound(finish);
            StartCoroutine(LoadNextScene(2f));
        }
    }

    private IEnumerator LoadNextScene(float delay) // Loads After 2 sec delay
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
