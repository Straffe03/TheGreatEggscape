using UnityEngine;

/**
 * Script per controlar el moviment del jugador amb AWSD.
 */
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private Vector3 direction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
            if (horizontal != 0 || vertical != 0)
        Debug.Log("👣 Moviment detectat!");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        controller.Move(direction * speed * Time.deltaTime);
    }
}
