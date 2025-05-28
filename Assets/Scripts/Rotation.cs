using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 25f; // graus per segon
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
