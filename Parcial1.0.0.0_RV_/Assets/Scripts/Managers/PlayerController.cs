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
    [Tooltip("Transform de la cámara activa (asignado por CameraManager). Solo se usa para calcular la dirección de movimiento relativa a cámara; el posicionamiento de la cámara en sí lo maneja Cinemachine / SharedCameraController.")]
    [SerializeField] private Transform cameraTransform;

    [Header("Grounded")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    // ================= VARIABLES =================
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private bool isGrounded;

    private Vector2 moveInput;
    private bool isRunning;
    private bool isCrouching;

    private float originalHeight;
    private Vector3 originalCenter;

    private PlayerInput input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // evita que el Rigidbody gire solo
        capsule = GetComponent<CapsuleCollider>();
        input = GetComponent<PlayerInput>();

        // TEMP DEBUG: confirmar que Awake corre y qué mapa/acciones tiene este objeto
        Debug.Log($"[{gameObject.name}] Awake ejecutado. Current Action Map: {input.currentActionMap?.name}");

        // Suscribir acciones
        input.actions["Move"].performed += OnMove;
        input.actions["Move"].canceled += OnMove;
        input.actions["Run"].performed += OnRun;
        input.actions["Run"].canceled += OnRun;
        input.actions["Crouch"].performed += OnCrouch;
        input.actions["Jump"].performed += OnJump;

        // TEMP DEBUG: confirmar que la suscripción a Jump se registró sin lanzar excepción
        Debug.Log($"[{gameObject.name}] Suscripción a Jump completada.");

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
        // TEMP DEBUG: quitar esta línea después de diagnosticar
        Debug.Log($"OnJump llamado. context.performed={context.performed}, isGrounded={isGrounded}, phase={context.phase}");

        if (context.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Salto ejecutado");
        }
    }

    // ================= MOVIMIENTO =================
    private void FixedUpdate()
    {
        float currentSpeed = walkSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;

        // Ground check: usar SOLO la capa de suelo, no "todas las capas" (~0),
        // para evitar detectar el propio collider del jugador o el del otro jugador.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // TEMP DEBUG: comparar contra un CheckSphere sin filtro de capa,
        // y mostrar qué collider(es) hay cerca, para saber si es un problema
        // de capa (Layer) o de posición/radio.
        bool hitsAnything = Physics.CheckSphere(groundCheck.position, groundCheckRadius, ~0);
        if (!isGrounded)
        {
            Collider[] nearby = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, ~0);
            string names = nearby.Length == 0 ? "ninguno" : string.Join(", ", System.Array.ConvertAll(nearby, c => $"{c.name}(layer:{LayerMask.LayerToName(c.gameObject.layer)})"));
            Debug.Log($"[{gameObject.name}] GroundCheck pos={groundCheck.position}, radius={groundCheckRadius}, hitsAnything(sin filtro)={hitsAnything}, colliders cercanos: {names}");
        }

        // Dirección relativa a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 movement = forward * moveInput.y + right * moveInput.x;

        // Velocidad horizontal
        Vector3 horizontalVelocity = movement * currentSpeed;

        // Mantener la velocidad vertical del Rigidbody (gravedad y salto)
        Vector3 velocity = new Vector3(horizontalVelocity.x, rb.linearVelocity.y, horizontalVelocity.z);
        rb.linearVelocity = velocity;

        // Rotación del personaje solo si hay movimiento
        if (movement.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }

        UpdateCrouchCollider();

        // Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.magnitude * (currentSpeed / runSpeed));
            animator.SetBool("IsCrouching", isCrouching);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        }
    }

    // NOTA: se eliminó el LateUpdate() que movía manualmente cameraTransform
    // (yaw, pitch, distance). Ahora Cinemachine (PlayerCameraLook +
    // CinemachineOrbitalFollow) y SharedCameraController son los únicos
    // responsables de posicionar las cámaras. Tener dos sistemas escribiendo
    // sobre el mismo Transform en el mismo frame era lo que causaba el
    // comportamiento errático de cámara.

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