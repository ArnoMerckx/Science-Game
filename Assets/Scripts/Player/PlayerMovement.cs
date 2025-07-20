using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;

    [Header("Movement Settings")]
    private Vector2 moveInput;
    private Rigidbody2D playerRb;
    public float WalkSpeed = 5f;

    [Header("Jump Settings")]
    public float JumpForce = 10f;
    public bool isGrounded;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        isGrounded = true;
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    public void Jump()
    {
        if (!isGrounded) return;
        playerRb.AddForceAtPosition(Vector2.up * JumpForce, playerRb.position, ForceMode2D.Impulse);
        isGrounded = false;
    }
    public void Move()
    {
        Vector2 movement = new Vector2(moveInput.x * WalkSpeed, playerRb.linearVelocity.y);
        playerRb.linearVelocity = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
