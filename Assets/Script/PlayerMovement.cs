using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public InputAction MoveAction;
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        MoveAction.Enable();

        // Automatically get Main Camera if not assigned
        if (cameraTransform == null)
        {
            cameraTransform = UnityEngine.Camera.main.transform;
        }
    }

    void FixedUpdate()
    {
        // Get WASD input
        Vector2 input = MoveAction.ReadValue<Vector2>();

        // Walking animation
        bool isWalking = input != Vector2.zero;
        anim.SetBool("IsWalking", isWalking);

        // Camera directions
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Ignore camera up/down angle
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Movement relative to camera
        Vector3 movement =
            cameraForward * input.y +
            cameraRight * input.x;

        // Prevent faster diagonal movement
        movement.Normalize();

        // Move player
        rb.MovePosition(
            rb.position +
            movement * walkSpeed * Time.fixedDeltaTime
        );

        // Rotate player towards movement direction
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(movement);

            rb.MoveRotation(
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
        }
    }

    void OnEnable()
    {
        MoveAction.Enable();
    }

    void OnDisable()
    {
        MoveAction.Disable();
    }
}