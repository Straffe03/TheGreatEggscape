using UnityEngine;

/**
 * Script que reprodueix música persistent entre escenes.
 * Assegura que només hi ha una instància i no es destrueix entre canvis d’escena.
 */
public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private void Awake()
    {
        // Evita duplicats
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
