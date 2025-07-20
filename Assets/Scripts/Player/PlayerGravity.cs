using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGravity : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    private InputAction gravityAction;

    public bool isGravityFlipped;
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
        isGravityFlipped = false;
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
        isGravityFlipped = !isGravityFlipped;
        Physics2D.gravity = isGravityFlipped ? new Vector2(0, -9.81f) : new Vector2(0, 9.81f);
    }
}

