using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGravity : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction gravityAction;

    private PlayerStats playerStats;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        playerStats = GetComponent<PlayerStatsManager>().playerStats;
        playerStats.IsGravityFlipped = false;
        gravityAction = inputActions.FindAction("Gravity");
    }

    void Update()
    {
        if (gravityAction.WasPressedThisFrame())
        {
            ToggleGravity();
        }
    }

    private void ToggleGravity()
    {
        playerStats.IsGravityFlipped = !playerStats.IsGravityFlipped;
        if (playerStats.IsGravityFlipped)
        {
            playerStats.Gravity = 9.81f; // Inverted gravity
        }
        else
        {
            playerStats.Gravity = -9.81f; // Normal gravity
        }
        gameObject.GetComponentInChildren<SpriteRenderer>().flipY = playerStats.IsGravityFlipped;
    }
}

