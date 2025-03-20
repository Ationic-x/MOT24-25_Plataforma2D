using System;
using UnityEngine;

public class PlayerAnimation : ActorAnimation
{
    private PlayerController _playerController;
    private PlayerStates _playerStates;
    private GhosTrail _ghostTrail;

	private float _ghostTrailDelay;

	private bool _wallJump = false;
	private bool _jump = false;
	// Events

	// Start is called before the first frame update
	protected override void Awake()
    {
		base.Awake();
        _playerController = GetComponentInParent<PlayerController>();
        _ghostTrail = GetComponent<GhosTrail>();
		_playerStates = _playerController.PlayerStates;
	}

	#region Events
	private void OnGroundedChanged(bool isGrounded)
    {
		animator.SetBool("Grounded", isGrounded);
	} 
    
    private void OnSlidingChanged(bool isSliding)
    {
		animator.SetBool("WallSlide", isSliding);
	}
    
    private void OnRunningChanged(bool isRunning)
    {
		animator.SetBool("Run", isRunning);
	} 
    
    private void OnFallingChanged(bool isFalling)
    {
		animator.SetBool("Fall", isFalling);
	}
    
    private void OnJumpingChanged(bool isJumping)
    {
		_jump = isJumping;
		animator.SetBool("Jump", _jump || _wallJump);
	}
	
	private void OnWallJumpingChanged(bool isJumping)
    {
		_wallJump = isJumping;
		animator.SetBool("Jump", _jump || _wallJump);
	}

    private void OnDashingChanged (bool isDashing)
    {
		animator.SetBool("Roll", true);
		_ghostTrail.Available = true;
		_ghostTrailDelay = Time.time + 0.3f;
	}


	private void OnAttackChanged(PlayerAttack.AttackType attack)
    {
		animator.SetBool("Attack", _playerStates.IsAttacking != 0);
		animator.SetBool("Attack1", _playerStates.IsAttacking == PlayerAttack.AttackType.Basic);
		animator.SetBool("Attack2", _playerStates.IsAttacking == PlayerAttack.AttackType.Quick);
		animator.SetBool("Attack3", _playerStates.IsAttacking == PlayerAttack.AttackType.Big);
	}
	#endregion

	protected override void Update()
    {
		base.Update();

        if (Time.time - _ghostTrailDelay > 0)
		{
            _ghostTrail.Available = false;
		}
    }

	protected override void SubscribeToEvents()
	{
		_playerStates.OnGroundedChanged += OnGroundedChanged;
		_playerStates.OnSlidingChanged += OnSlidingChanged;
		_playerStates.OnRunningChanged += OnRunningChanged;
		_playerStates.OnFallingChanged += OnFallingChanged;
		_playerStates.OnJumpingChanged += OnJumpingChanged;
		_playerStates.OnWallJumpingChanged += OnWallJumpingChanged;
		_playerStates.OnDashingChanged += OnDashingChanged;
		_playerStates.OnAttackChanged += OnAttackChanged;
	}

	protected override void UnsubscribeFromEvents()
	{
		_playerStates.OnGroundedChanged -= OnGroundedChanged;
		_playerStates.OnSlidingChanged -= OnSlidingChanged;
		_playerStates.OnRunningChanged -= OnRunningChanged;
		_playerStates.OnFallingChanged -= OnFallingChanged;
		_playerStates.OnJumpingChanged -= OnJumpingChanged;
		_playerStates.OnWallJumpingChanged -= OnWallJumpingChanged;
		_playerStates.OnDashingChanged -= OnDashingChanged;
		_playerStates.OnAttackChanged -= OnAttackChanged;
	}
}

