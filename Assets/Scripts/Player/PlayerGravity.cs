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
        Physics2D.gravity = playerStats.IsGravityFlipped ? new Vector2(0, 9.81f) : new Vector2(0, -9.81f);
        gameObject.GetComponentInChildren<SpriteRenderer>().flipY = playerStats.IsGravityFlipped;
    }
}

