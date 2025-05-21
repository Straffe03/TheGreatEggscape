using UnityEngine;

public class Key : MonoBehaviour
{
    public int scorePerKey = 100;
    public GameManager gameManager;
    public GameObject key;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.keys++;
            gameManager.score += scorePerKey;
            
            Destroy(key);
        }
    }
}
