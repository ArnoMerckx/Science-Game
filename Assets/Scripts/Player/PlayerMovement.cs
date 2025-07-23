using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction downAction;

    [Header("Movement Parameters")]
    private Vector2 moveInput;
    private Rigidbody2D playerRb;


    public PlayerStats playerStats;

    private GameObject currentPlatform;


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
        playerStats = GetComponent<PlayerStatsManager>().playerStats;
        playerStats.IsGrounded = true;
        playerStats.IsGravityFlipped = false;
        playerStats.Gravity = -9.81f; // Default gravity value
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        downAction = InputSystem.actions.FindAction("Down");
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
        if ( downAction.WasPressedThisFrame())
        {
            if (currentPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }
    void FixedUpdate()
    {
        Move();
        HandleSprite();
    }

    public void Jump()
    {
        if (!playerStats.IsGrounded) return;
        if (playerStats.IsGravityFlipped)
        {
            playerRb.AddForceAtPosition(Vector2.down * playerStats.JumpForce, playerRb.position, ForceMode2D.Impulse);
        }
        else
        {
            playerRb.AddForceAtPosition(Vector2.up * playerStats.JumpForce, playerRb.position, ForceMode2D.Impulse);
        } 
        playerStats.IsGrounded = false;
    }
    public void Move()
    {
        Vector2 movement = new Vector2(moveInput.x * playerStats.WalkSpeed, playerRb.linearVelocity.y + playerStats.Gravity * Time.deltaTime);
        playerRb.linearVelocity = movement;
    }

    private void HandleSprite()
    {
        SpriteRenderer spriteRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
        if (playerRb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (playerRb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided object has Ground tag
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            // Check if Ground is below the player when gravity is normal
            if (!playerStats.IsGravityFlipped && collision.contacts[0].point.y < transform.position.y)
            {
                playerStats.IsGrounded = true;
            }
            // Check if Ground is above the player when gravity is flipped
            else if (playerStats.IsGravityFlipped && collision.contacts[0].point.y > transform.position.y)
            {
                playerStats.IsGrounded = true;
            }
        }
        if(collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if exited object has Ground tag
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentPlatform.GetComponent<Collider2D>();
        Collider2D[] playerColliders = GetComponents<Collider2D>();
        foreach (Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
        }
        yield return new WaitForSeconds(1f);
        foreach (Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
    }
}
