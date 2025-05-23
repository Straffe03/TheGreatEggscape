using UnityEngine;
using System.Collections;

/**
 * Script que reprodueix un so de mort i després crida a la funció final.
 */
public class DeathSoundPlayer : MonoBehaviour
{
    public static DeathSoundPlayer Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Que sobrevisqui al canvi d’escena
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // Evitar duplicats
        }
    }

    public void PlayDeathSound(AudioClip clip, System.Action onComplete)
    {
        StartCoroutine(PlayAndThen(clip, onComplete));
    }

    private IEnumerator PlayAndThen(AudioClip clip, System.Action onComplete)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(clip.length);
        onComplete?.Invoke();
    }
}
