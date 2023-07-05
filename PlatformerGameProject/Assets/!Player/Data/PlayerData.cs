using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State Vars")]
    public float movementVelocity = 0.8f;

    [Header("Jump State Vars")]
    public float jumpVelocity = 3;
    public int amountOfJumps = 1;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 3;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Dash State")]
    public float dashVelocity = 3;

    [Header("In Air State Vars")]
    public float coyoteTime = 0.2f;

    [Header("Wall Slide State")]
    public float wallSlideVelocity = 0.1f;

    [Header("Check Vars")]
    public float groundCheckRadius = 0.057f;
    public LayerMask whatIsGround;
    public float wallCheckDistance = 0.5f;
}
