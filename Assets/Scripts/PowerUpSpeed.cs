using UnityEngine;

public class PowerUpSpeed : MonoBehaviour
{
    public float speedBoost = 2f; // Factor de multiplicació de la velocitat
    public float duration = 5f; // Durada de l'augment de velocitat
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.pickUps++;
            GameManager.Instance.ActivateTimer(duration);
            Debug.Log("PowerUp Speed recollit!");
            GameManager.score += GameManager.Instance.scorePerPowerUpSpeed;
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.IncreaseSpeed(speedBoost, duration);
            }
            Destroy(gameObject);
        }
    }
}
