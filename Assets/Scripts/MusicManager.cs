using UnityEngine;

/**
 * Script que reprodueix música persistent entre escenes.
 * Assegura que només hi ha una instància i no es destrueix entre canvis d’escena.
 */

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMute(bool mute)
    {
        audioSource.mute = mute;
    }

    public bool IsMuted()
    {
        return audioSource.mute;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
}
