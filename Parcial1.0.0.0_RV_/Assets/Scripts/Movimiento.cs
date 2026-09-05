using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayer : MonoBehaviour
{
    private MovePlayer inputActions;

    private Vector2 moveInput;
    private bool isRunning;
    private bool isCrouching;

    [Header("Movement")]
    public float speed = 5f;
    public float runSpeed = 10f;
    public float crouchSpeed = 2.5f;

    [Header("Jump")]
    public float jumpForce = 5f;

    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    private float originalHeight;
    private Vector3 originalCenter;

    private void Awake()
    {
        inputActions = new MovePlayer();
        rb = GetComponent<Rigidbody>();

        // Collider para agacharse (opcional, pero recomendado)
        playerCollider = GetComponent<CapsuleCollider>();

        if (playerCollider != null)
        {
            originalHeight = playerCollider.height;
            originalCenter = playerCollider.center;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Run.performed += OnRun;
        inputActions.Player.Run.canceled += OnRun;

        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Run.performed -= OnRun;
        inputActions.Player.Run.canceled -= OnRun;

        inputActions.Player.Jump.performed -= OnJump;

        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump pressed");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        // Left Ctrl sostenido para agacharse
        isCrouching = Keyboard.current.leftCtrlKey.isPressed;

        // Cambiar tamaño del collider al agacharse
        if (playerCollider != null)
        {
            if (isCrouching)
            {
                playerCollider.height = originalHeight / 2f;
                playerCollider.center = new Vector3(
                    originalCenter.x,
                    originalCenter.y / 2f,
                    originalCenter.z
                );
            }
            else
            {
                playerCollider.height = originalHeight;
                playerCollider.center = originalCenter;
            }
        }

        // Elegir velocidad
        float currentSpeed = speed;

        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isRunning)
            currentSpeed = runSpeed;

        // Movimiento
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        transform.Translate(movement * currentSpeed * Time.fixedDeltaTime, Space.World);
    }
}