using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private Vector2 moveInput;
    private bool isRunning;
    private bool isCrouching;

    private float originalHeight;
    private Vector3 originalCenter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        if (capsule != null)
        {
            originalHeight = capsule.height;
            originalCenter = capsule.center;
        }
    }

    // ================= INPUT =================

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"{gameObject.name}  {moveInput}");
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.ReadValueAsButton();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // ================= MOVIMIENTO =================

    private void FixedUpdate()
    {
        float currentSpeed = walkSpeed;

        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isRunning)
            currentSpeed = runSpeed;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Quitamos la inclinación de la cámara.
        forward.y = 0f;
        right.y = 0f;

        // Normalizamos para que siempre midan 1.
        forward.Normalize();
        right.Normalize();

        // Movimiento relativo a la cámara.
        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.fixedDeltaTime
            );
        }

        rb.MovePosition(
            rb.position + movement * currentSpeed * Time.fixedDeltaTime
        );

        UpdateCrouchCollider();
    }

    private void UpdateCrouchCollider()
    {
        if (capsule == null) return;

        if (isCrouching)
        {
            capsule.height = originalHeight / 2f;
            capsule.center = new Vector3(
                originalCenter.x,
                originalCenter.y / 2f,
                originalCenter.z
            );
        }
        else
        {
            capsule.height = originalHeight;
            capsule.center = originalCenter;
        }
    }
    public void SetCameraTransform(Transform newCamera)
    {
        cameraTransform = newCamera;
    }
}