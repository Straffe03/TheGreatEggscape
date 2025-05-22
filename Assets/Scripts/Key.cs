using System;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Action onCollected;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.KeyCollected();
            onCollected?.Invoke();
            Destroy(gameObject);

            //Destroy(gameObject);
        }
    }
}
