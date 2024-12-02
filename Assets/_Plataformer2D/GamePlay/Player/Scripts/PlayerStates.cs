using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStates
{
    // Movement States
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isJumpFalling;
    [SerializeField] private bool _isWallJumping;
    [SerializeField] private bool _isJumpCut;
    [SerializeField] private bool _isSliding;

    // Collision States
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isCollidedLeft;
    [SerializeField] private bool _isCollidedRight;


    // General States
    [SerializeField] private bool _isFacingRight = true; // By default the character is facing right


	// Events
	public event Action<bool> OnFacingRightChanged;
	public event Action OnSlidingChanged;
	public event Action OnGroundedChanged;

	#region Getters And Setters
	// Movement States
	public bool IsRunning { get => _isRunning; set => _isRunning = value; }

    public bool IsDashing { get => _isDashing; set => _isDashing = value; }

    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    public bool IsFalling { get => _isJumpFalling; set => _isJumpFalling = value; }

    public bool IsWallJumping { get => _isWallJumping; set => _isWallJumping = value; }

    public bool IsJumpCut { get => _isJumpCut; set => _isJumpCut = value; }

    public bool IsSliding 
    { 
        get => _isSliding;
        set
        {
            if (_isSliding != value)
            {
                _isSliding = value;
                OnSlidingChanged?.Invoke();
            }
        }
    }

    // Collision States
    public bool IsGrounded
    {
        get => _isGrounded;
        set
        {
            if (_isGrounded != value)
            {
                _isGrounded = value;
                OnGroundedChanged?.Invoke();
            }
        }
    }

    public bool IsCollidedLeft { get => _isCollidedLeft; set => _isCollidedLeft = value; }

    public bool IsCollidedRight { get => _isCollidedRight; set => _isCollidedRight = value; }

    // General States
    public bool IsFacingRight 
    {
        get => _isFacingRight;
        set {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                OnFacingRightChanged?.Invoke(_isFacingRight);
            }
        } 
    }

    #endregion
}
