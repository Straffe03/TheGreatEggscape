using System.Collections;
using UnityEngine;

/**
 * Moviment relatiu a la càmera, càmera controlada pel ratolí.
 * Inclou animacions i esprint.
 */
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private Coroutine speedBoostCoroutine = null;
    private float originalWalkSpeed;
    private float originalSprintSpeed;
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotationSpeed = 720f;
    public Transform cameraTransform; // Assignar la càmera manualment

    //Cosas para no flotar
    private Vector3 verticalVelocity;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        originalWalkSpeed = walkSpeed;
        originalSprintSpeed = sprintSpeed;
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        // Vectors relatius a la càmera
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;
        moveDir.Normalize();

        // Detectar si es prem la tecla d'esprint
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        if (inputDirection.magnitude >= 0.1f)
        {
            animator.SetFloat("Vert", isSprinting ? 1f : 0.85f);
            animator.speed = isSprinting ? 2f : 2f;

            // Rotació del personatge
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Vert", 0f);
            animator.speed = 1f;
        }
        // Aplica gravetat
        if (controller.isGrounded)
        {
            verticalVelocity.y = -2f; // per mantenir ancorat a terra
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }

        Vector3 totalMove = moveDir * currentSpeed + verticalVelocity;
        // Moviment aplicat
        controller.Move(totalMove * Time.deltaTime);
    }

    public void IncreaseSpeed(float speedBoost, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
            ResetSpeed();
        }

        speedBoostCoroutine = StartCoroutine(SpeedBoostCoroutine(speedBoost, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float speedBoost, float duration)
    {
        walkSpeed += speedBoost;
        sprintSpeed += speedBoost;
        Debug.Log($"Velocitat augmentada: walk={walkSpeed}, sprint={sprintSpeed}");

        yield return new WaitForSeconds(duration);

        ResetSpeed();
        speedBoostCoroutine = null;
    }

    public void ResetSpeed()
    {
        Debug.Log("Speed reset to original values.");
        walkSpeed = originalWalkSpeed;
        sprintSpeed = originalSprintSpeed;
    }
}
