using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // ================= CONFIGURACIÓN =================
    [Header("Velocidad")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2.5f;

    [Header("Salto")]
    [SerializeField] private float jumpForce = 5f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float lookSensitivityMouse = 2f;
    [SerializeField] private float lookSensitivityGamepad = 100f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 10f;

    [Header("Grounded")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    // ================= VARIABLES =================
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning;
    private bool isCrouching;

    private float yaw;
    private float pitch = 20f;
    private float distance = 5f;

    private float originalHeight;
    private Vector3 originalCenter;

    private PlayerInput input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        input = GetComponent<PlayerInput>();

        // Suscribir acciones
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMove;
        input.actions["Run"].performed += OnRun;
        input.actions["Run"].canceled += OnRun;
        input.actions["Crouch"].performed += OnCrouch;
        input.actions["Jump"].performed += OnJump;
        input.actions["Look"].performed += OnLook;
        input.actions["Zoom"].performed += OnZoom;

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
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            isCrouching = !isCrouching;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Salto ejecutado");
        }
    }


    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        float zoomInput = context.ReadValue<float>();


        distance -= zoomInput * zoomSpeed * Time.deltaTime;

        // Evita que la cámara se meta dentro del jugador o se aleje demasiado
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    // ================= MOVIMIENTO =================
    private void FixedUpdate()
    {
        float currentSpeed = walkSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }

        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);

        UpdateCrouchCollider();

        animator.SetFloat("Speed", movement.magnitude * (currentSpeed / runSpeed));
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
    }

    private void LateUpdate()
    {
        // Rotación de cámara con mouse o gamepad
        yaw += lookInput.x * lookSensitivityMouse;
        pitch -= lookInput.y * lookSensitivityMouse;
        pitch = Mathf.Clamp(pitch, -40f, 80f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        cameraTransform.position = transform.position + offset;
        cameraTransform.LookAt(transform.position);
    }

    private void UpdateCrouchCollider()
    {
        if (capsule == null) return;

        if (isCrouching)
        {
            capsule.height = originalHeight / 2f;
            capsule.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);
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