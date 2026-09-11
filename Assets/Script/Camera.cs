using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Camera Settings")]
    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 200f;

    [Header("Collision")]
    public float cameraRadius = 0.3f;
    public LayerMask collisionMask;

    private float mouseX;
    private float mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Mouse input
        mouseX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Prevent camera from rotating too far up/down
        mouseY = Mathf.Clamp(mouseY, -30f, 60f);

        // Camera rotation
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // Position where we WANT the camera to be
        Vector3 targetPosition = player.position + Vector3.up * height;

        Vector3 desiredPosition =
            targetPosition - rotation * Vector3.forward * distance;

        // Direction from player towards camera
        Vector3 direction = desiredPosition - targetPosition;

        // Check if wall is between player and camera
        if (Physics.SphereCast(
            targetPosition,
            cameraRadius,
            direction.normalized,
            out RaycastHit hit,
            distance,
            collisionMask))
        {
            // Move camera closer if wall is hit
            desiredPosition =
                targetPosition +
                direction.normalized * (hit.distance - cameraRadius);
        }

        // Apply camera position and rotation
        transform.position = desiredPosition;
        transform.rotation = rotation;
    }
}