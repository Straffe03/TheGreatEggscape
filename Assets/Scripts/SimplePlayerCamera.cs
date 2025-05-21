using UnityEngine;

public class SimplePlayerCamera : MonoBehaviour
{
    [Header("Configuració")]
    public Transform player;  // assigna la gallina
    public Vector3 offset = new Vector3(0, 2, -5);
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 1f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    public float distance = 5f;

    private float rotX = 20f;
    private float rotY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloqueja el cursor al centre
        Cursor.visible = false; // Amaga el cursor
    }

    private void LateUpdate()
    {
        if (player == null) return;

        // 🔁 Agafa moviment del ratolí
        rotY += Input.GetAxis("Mouse X") * mouseSensitivityX;
        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivityY;
        rotX = Mathf.Clamp(rotX, minVerticalAngle, maxVerticalAngle);

        // 🌀 Calcula rotació
        Quaternion rotation = Quaternion.Euler(rotX, rotY, 0);
        Vector3 targetPosition = player.position + rotation * offset;

        // 📷 Aplica posició i mirada
        transform.position = targetPosition;
        transform.LookAt(player.position);
    }
}
