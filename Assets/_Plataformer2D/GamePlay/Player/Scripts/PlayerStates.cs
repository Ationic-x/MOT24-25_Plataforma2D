using System;
using UnityEngine;

[System.Serializable]
public class PlayerStates: ActorStates<PlayerAttack.AttackType>
{
    // Movement States
    [SerializeField] private bool _isRunning;
    [SerializeField] private bool _isDashing;
    [SerializeField] private bool _isJumping;
    [SerializeField] private bool _isFalling;
    [SerializeField] private bool _isWallJumping;
    [SerializeField] private bool _isJumpCut;
    [SerializeField] private bool _isSliding;

    // Collision States
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isCollidedBackward;
    [SerializeField] private bool _isCollidedForward;


	// Events
	public event Action<bool> OnSlidingChanged;
	public event Action<bool> OnGroundedChanged;
	public event Action<bool> OnRunningChanged;
	public event Action<bool> OnJumpingChanged;
	public event Action<bool> OnWallJumpingChanged;
	public event Action<bool> OnFallingChanged;
	public event Action<bool> OnDashingChanged;

	#region Getters And Setters
	// Movement States
	public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
			OnRunningChanged?.Invoke(_isRunning);
		}
    }

    public bool IsDashing
    {
        get => _isDashing;
        set
        {
            _isDashing = value;
            OnDashingChanged?.Invoke(_isDashing);
        }
    }

    public bool IsJumping { get => _isJumping; 
        set 
        {
            _isJumping = value;
			OnJumpingChanged?.Invoke(_isJumping);
		}
	}

    public bool IsFalling { get => _isFalling; 
        set 
        {
            _isFalling = value; 
            OnFallingChanged?.Invoke(_isFalling);
		}
	}

    public bool IsWallJumping { get => _isWallJumping; 
        set
        { 
            _isWallJumping = value;
			OnWallJumpingChanged?.Invoke(_isWallJumping);
		}
	}

    public bool IsJumpCut { get => _isJumpCut; set => _isJumpCut = value; }

    public bool IsSliding 
    { 
        get => _isSliding;
        set
        {
            if (_isSliding != value)
            {
                _isSliding = value;
                OnSlidingChanged?.Invoke(_isSliding);
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
                OnGroundedChanged?.Invoke(_isGrounded);
            }
        }
    }

    public bool IsCollidedBackward { get => _isCollidedBackward; set => _isCollidedBackward = value; }

    public bool IsCollidedForward { get => _isCollidedForward; set => _isCollidedForward = value; }
    #endregion
}
