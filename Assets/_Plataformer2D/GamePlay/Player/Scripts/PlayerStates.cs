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

    // Player Time
    [SerializeField] private float _wallJumpStartTime;
    [SerializeField] private float _jumpFinishTime;


	// Events
	public event Action<bool> OnFacingRightChanged;

	#region Getters And Setters
	// Movement States
	public bool IsRunning { get => _isRunning; set => _isRunning = value; }

    public bool IsDashing { get => _isDashing; set => _isDashing = value; }

    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    public bool IsJumpFalling { get => _isJumpFalling; set => _isJumpFalling = value; }

    public bool IsWallJumping { get => _isWallJumping; set => _isWallJumping = value; }

    public bool IsJumpCut { get => _isJumpCut; set => _isJumpCut = value; }

    public bool IsSliding { get => _isSliding; set => _isSliding = value; }

    // Collision States
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    public bool IsCollidedLeft { get => _isCollidedLeft; set => _isCollidedLeft = value; }

    public bool IsCollidedRight { get => _isCollidedRight; set => _isCollidedRight = value; }

    // General States
    public bool IsFacingRight {
        get => _isFacingRight;
        set {
            if (_isFacingRight != value)
            {
                _isFacingRight = value;
                OnFacingRightChanged?.Invoke(_isFacingRight);
            }
        } 
    }

    // Player Time
    public float WallJumpStartTime { get => _wallJumpStartTime; set => _wallJumpStartTime = value; }

    public float JumpFinishTime { get => _jumpFinishTime; set => _jumpFinishTime = value; }

    #endregion
}
