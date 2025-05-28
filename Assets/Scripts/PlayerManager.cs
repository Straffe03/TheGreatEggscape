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

    public GameObject bloodEffectPrefab;

    private void Start()
    {
        GameManager.Instance.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        GameManager.Instance.SetHealth(currentHealth);
        Debug.Log("Has rebut " + amount + " de dany. Vida restant: " + currentHealth);

        if (bloodEffectPrefab != null)
        {
            GameObject blood = Instantiate(bloodEffectPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            Destroy(blood, 1.5f);
        }
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
