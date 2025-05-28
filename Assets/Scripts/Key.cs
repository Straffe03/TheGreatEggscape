using System;
using UnityEngine;


public class Key : MonoBehaviour
{
    private ParticleSystem collectParticles;
    public Action onCollected;
    private AudioSource audioSource;
    void Start()
    {
        collectParticles = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponentInChildren<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (collectParticles != null)
            {
                collectParticles.transform.parent = null; // lo soltamos para que no muera con la llave
                collectParticles.Play();
                Destroy(collectParticles.gameObject, collectParticles.main.duration);
            }
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.transform.parent = null;
                audioSource.PlayOneShot(audioSource.clip);
                Destroy(audioSource.gameObject, collectParticles.main.duration);
            }
            GameManager.Instance.KeyCollected();
            onCollected?.Invoke();
            Destroy(gameObject);
            
        }
    }
}
