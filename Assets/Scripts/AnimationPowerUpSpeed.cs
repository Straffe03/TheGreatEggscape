using UnityEngine;

public class AnimationPowerUpSpeed : MonoBehaviour
{
    public float rotationSpeed = 90f; // graus per segon
    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
