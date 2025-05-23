using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    [Header("So de dany")]
    public AudioClip hurtSound;
    [Header("So de mort (opcional)")]
    public AudioClip deathSound;

    private AudioSource audioSource;

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Has rebut " + amount + " de dany. Vida restant: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }else if (hurtSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }

    }

    private void Die()
    {
        if (deathSound != null && DeathSoundPlayer.Instance != null)
        {
            DeathSoundPlayer.Instance.PlayDeathSound(deathSound, () => {
                GameManager.Instance.PlayerLoss();
            });
        }
        else
        {
            GameManager.Instance.PlayerLoss();
        }
        Debug.Log("El jugador ha mort!");
        GameManager.Instance.PlayerLoss();
    }

}
