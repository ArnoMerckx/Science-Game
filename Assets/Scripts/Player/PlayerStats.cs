using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Player System/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Movement")]
    public float WalkSpeed = 5f;
    public float JumpForce = 5f;
    public bool IsGrounded = true;  
    public float Gravity = -9.81f;

    [Header("Player Gravity")]
    public bool IsGravityFlipped = false;
}
