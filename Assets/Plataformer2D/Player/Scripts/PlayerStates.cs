using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    // Movement States
    [SerializeField] private bool isRunning;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isJumpFalling;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private bool isJumpCut;
    [SerializeField] private bool isSliding;

    // Collistion State
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isCollidedLeft;
    [SerializeField] private bool isCollidedRight;


    // General States
    [SerializeField] private bool isFacingRight = true; // By default the character is facing right
    [SerializeField] private bool isDashRefilling;

    // Player Counts
    [SerializeField] private int dashesLeft;

    // Player Time
    [SerializeField] private float wallJumpStartTime;
    [SerializeField] private float jumpDelayStartTime;

    #region Getters And Setters
    // Movement States
    public bool IsRunning
    {
        get => isRunning;
        set => isRunning = value;
    }

    public bool IsDashing
    {
        get => isDashing;
        set => isDashing = value;
    }

    public bool IsJumping
    {
        get => isJumping;
        set => isJumping = value;
    }

    public bool IsJumpFalling
    {
        get => isJumpFalling;
        set => isJumpFalling = value;
    }

    public bool IsWallJumping
    {
        get => isWallJumping;
        set => isWallJumping = value;
    }

    public bool IsJumpCut
	{
        get => isJumpCut;
        set => isJumpCut = value;
	}

    public bool IsSliding
	{
        get => isSliding;
        set => isSliding = value;
    }

    // Collision States
    public bool IsGrounded
    {
        get => isGrounded;
        set => isGrounded = value;
    }

    public bool IsCollidedLeft
    {
        get => isCollidedLeft;
        set => isCollidedLeft = value;
    }

    public bool IsCollidedRight
    {
        get => isCollidedRight;
        set => isCollidedRight = value;
    }

    // General States
    public bool IsFacingRight
    {
        get => isFacingRight;
        set => isFacingRight = value;
    }

    public bool IsDashRefilling
	{
        get => isDashRefilling;
        set => isDashRefilling = value;
	}

    // Player Counts
    public int DashesLeft
	{
        get => dashesLeft;
        set => dashesLeft = value;
	}

    // Player Time
    public float WallJumpStartTime
	{
        get => wallJumpStartTime;
        set => wallJumpStartTime = value;
    }

    public float JumpDelayStartTime
	{
        get => jumpDelayStartTime;
        set => jumpDelayStartTime = value;
    }

    #endregion
}
