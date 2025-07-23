using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction downAction;
    private InputAction enterDoorAction;
    private InputAction portalAction;

    [Header("Movement Parameters")]
    private Vector2 moveInput;
    private Rigidbody2D playerRb;


    public PlayerStats playerStats;

    [Header("Upgrade Parameters")]
    private UpgradeHelper upgradeHelper;
    private GameObject currentPlatform;
    public GameObject currentPortal;
    public List<GameObject> spawnedInPortals;


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
        //Get Components
        playerStats = GetComponent<PlayerStatsManager>().playerStats;
        playerRb = GetComponent<Rigidbody2D>();
        upgradeHelper = GameObject.Find("UpgradeHelper").GetComponent<UpgradeHelper>();

        //Reset Parameters
        playerStats.IsGrounded = true;
        playerStats.IsGravityFlipped = false;
        playerStats.Gravity = -9.81f; // Default gravity value
        spawnedInPortals = new List<GameObject>();

        //Setup Input
        SetupInputActions();
        
    }

    private void SetupInputActions()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        downAction = InputSystem.actions.FindAction("Down");
        enterDoorAction = InputSystem.actions.FindAction("Enter");
        portalAction = InputSystem.actions.FindAction("Portal");
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
        if (enterDoorAction.WasPressedThisFrame())
        {
            if (currentPortal != null)
            {
                currentPortal.GetComponent<Portal>().TelePortToDestination();
            }
        }
        if (portalAction.WasPressedThisFrame())
        {
            HandlePortalSpawn();
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

    private void HandlePortalSpawn()
    {
        if (currentPortal != null)
        {
            
            int index = spawnedInPortals.IndexOf(currentPortal);
            Destroy(spawnedInPortals[index]);
            spawnedInPortals.RemoveAt(index);
            currentPortal = null;
        }
        else
        {
            if (spawnedInPortals.Count == 0)
            {
                GameObject newPortal = Instantiate(upgradeHelper.portals[0], transform.position, Quaternion.identity);
                newPortal.GetComponent<Portal>().portalColor = PortalColor.Blue;
                spawnedInPortals.Add(newPortal);
            }
            else if (spawnedInPortals.Count == 1)
            {
                if (spawnedInPortals[0].GetComponent<Portal>().portalColor == PortalColor.Blue)
                {
                    GameObject newPortal = Instantiate(upgradeHelper.portals[1], transform.position, Quaternion.identity);
                    newPortal.GetComponent<Portal>().portalColor = PortalColor.Orange;
                    spawnedInPortals.Add(newPortal);
                }
                else
                {
                    GameObject newPortal = Instantiate(upgradeHelper.portals[0], transform.position, Quaternion.identity);
                    newPortal.GetComponent<Portal>().portalColor = PortalColor.Blue;
                    spawnedInPortals.Add(newPortal);
                }
                foreach (var portal in spawnedInPortals)
                {
                    portal.GetComponent<Portal>().ResetDestinationPortal();
                }
            }
            else
            {
                if (spawnedInPortals[1].GetComponent<Portal>().portalColor == PortalColor.Blue)
                {
                    GameObject newPortal = Instantiate(upgradeHelper.portals[1], transform.position, Quaternion.identity);
                    newPortal.GetComponent<Portal>().portalColor = PortalColor.Orange;
                    spawnedInPortals.Add(newPortal);
                }
                else
                {
                    GameObject newPortal = Instantiate(upgradeHelper.portals[0], transform.position, Quaternion.identity);
                    newPortal.GetComponent<Portal>().portalColor = PortalColor.Blue;
                    spawnedInPortals.Add(newPortal);
                }

            }
            if (spawnedInPortals.Count > 2)
            {
                Destroy(spawnedInPortals[0]);
                spawnedInPortals.RemoveAt(0);
            }
            foreach (var portal in spawnedInPortals)
            {
                portal.GetComponent<Portal>().ResetDestinationPortal();
            }
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
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentPlatform = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            currentPortal = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal") && other.gameObject == currentPortal)
        {
            currentPortal = null;
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
