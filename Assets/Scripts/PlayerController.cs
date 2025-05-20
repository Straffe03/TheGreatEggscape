using UnityEngine;

/**
 * Moviment relatiu a la càmera, càmera controlada pel ratolí.
 * Inclou animacions i esprint.
 */
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float rotationSpeed = 720f;
    public Transform cameraTransform; // Assignar la càmera manualment

    private CharacterController controller;
    private Animator animator;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        // Si no es mou, aturem animació i sortim
        if (inputDirection.magnitude < 0.1f)
        {
            animator.SetFloat("Vert", 0f);
            animator.speed = 1f; // Restaurem la velocitat per seguretat
            return;
        }

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

        // Ajustem l'animació i velocitat segons estat
        animator.SetFloat("Vert", isSprinting ? 1f : 0.85f);
        animator.speed = isSprinting ? 2f : 2f; // Fa que caminar sigui més viu

        // Rotació del personatge
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Moviment aplicat
        controller.Move(moveDir * currentSpeed * Time.deltaTime);
    }
}
