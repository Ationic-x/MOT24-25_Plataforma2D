using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement2D : MonoBehaviour
{
	private PlayerController _playerController;
	
	private PlayerStates _playerStates;
	private Rigidbody2D _rigidbody;
	private PlayerData _playerData;


	// Input Variables
	[SerializeField] private Vector2 _moveInput;
	[SerializeField] private bool _jumpInput;
	[SerializeField] private bool _dashInput;

	// Movement states
	private float _wallJumpStartTime;
	private float _jumpCoyoteTime;
	private float _wallJumpCoyoteTime;
	private float _dashDelayTime;
	
	private bool _slideIsFacingRight;

	// Movement count
	private int _jumpsLeft;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
	}

	private void Start()
	{
		_rigidbody = _playerController.Rigidbody;
		_playerData = _playerController.PlayerData;

		_jumpsLeft = _playerData.jumpAmount; // TODO: locked max jump amount (require perk)
	}

	#region Events
	private void OnEnable()
    {
		_playerStates = _playerController.PlayerStates;

		// Input Events
		_playerController.MoveAction.action.performed += OnMoveInput;
		_playerController.MoveAction.action.canceled += OnMoveInput;

		_playerController.JumpAction.action.performed += OnJumpInput;
		_playerController.JumpAction.action.canceled += OnJumpInput;
		
		_playerController.DashAction.action.performed += OnDashInput;
		_playerController.DashAction.action.canceled += OnDashInput;

		// State events
		_playerStates.OnSlidingChanged += OnSlidingChanged;
		_playerStates.OnGroundedChanged += OnGroundedChanged;
	}

    private void OnDisable()
    {
		// Input Events
		_playerController.MoveAction.action.performed -= OnMoveInput;
		_playerController.MoveAction.action.canceled -= OnMoveInput;

		_playerController.JumpAction.action.performed -= OnJumpInput;
		_playerController.JumpAction.action.canceled -= OnJumpInput;

		_playerController.DashAction.action.performed -= OnDashInput;
		_playerController.DashAction.action.canceled -= OnDashInput;

		//	State events
		_playerStates.OnSlidingChanged -= OnSlidingChanged;
		_playerStates.OnGroundedChanged -= OnGroundedChanged;
	}

    private void OnMoveInput(InputAction.CallbackContext context = default)
	{
		Vector2 tempVector = context.action.ReadValue<Vector2>();
		_moveInput.x = tempVector.x > 0 ? 1 : tempVector.x < 0 ? -1 : 0;
	}

	private void OnJumpInput(InputAction.CallbackContext context = default)
	{
		_jumpInput = context.performed;

		if (_jumpInput)
			StartCoroutine(HandleJump());
	}

	private void OnDashInput(InputAction.CallbackContext context = default)
	{
		_dashInput = context.performed;  // Currently isn't used

		if (_dashInput)
			StartCoroutine(HandleDash());
	}

	private void OnGroundedChanged(bool isGrounded)
	{
		if (isGrounded)
			_jumpsLeft = _playerData.jumpAmount;
		else
		{
			_jumpCoyoteTime = Time.time;
			_jumpsLeft--;
		}
	}
	
	private void OnSlidingChanged(bool isSliding)
	{
		if (isSliding)
			_jumpsLeft = _playerData.jumpAmount;
		else
		{
			_wallJumpCoyoteTime = Time.time;
			_jumpsLeft--;
		}
	}
	#endregion

	// All the behvaidour managed per update of game
	// Currently States and Gravity
	private void Update()
	{
		if (CanMove() && _moveInput.x != 0)
			_playerController.SwapFacingPosition(_moveInput.x > 0);

		HandleStates();
		HandleGravity();
	}

	// All the behaviour managed per update of rb
	private void FixedUpdate()
	{
		//Handle Run
		if (!_playerStates.IsDashing)
		{
			if (_playerStates.IsWallJumping)
				Run(_playerData.wallJumpRunLerp);
			else
				Run(1);
		}
		//else if (_isDashAttacking)
		//{
		//	Run(Data.dashEndRunLerp);
		//}

		//Handle Slide
		if (_playerStates.IsSliding)
			Slide();
	}
	

	#region Handle Behaviour (A bridge between actions and general behaviour)
	private void HandleStates()
	{
		_playerStates.IsSliding = CanSlide();

		_playerStates.IsJumpCut = _playerStates.IsJumping && (!_jumpInput || !CanMove());

		if (_rigidbody.linearVelocity.y < 0 && !_playerStates.IsSliding && !_playerStates.IsDashing)
		{
			_playerStates.IsJumping = false;

			_playerStates.IsFalling = true;
		}

		if (_playerStates.IsWallJumping && Time.time - _wallJumpStartTime > _playerData.wallJumpTime)
			_playerStates.IsWallJumping = false;

		if (_playerStates.IsGrounded && !_playerStates.IsJumping && !_playerStates.IsWallJumping)
		{
			_playerStates.IsJumpCut = false;

			_playerStates.IsFalling = false;
		}
	}

	private void HandleGravity()
	{
		//Higher gravity if we've released the jump input or are falling
		if (_playerStates.IsSliding)
		{
			_playerController.SetGravityScale(0);
		}
		else if (_rigidbody.linearVelocity.y < 0 && _moveInput.y < 0)
		{
			//Much higher gravity if holding down
			_playerController.SetGravityScale(_playerData.gravityScale * _playerData.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, -_playerData.maxFastFallSpeed));
		}
		else if (_playerStates.IsJumpCut)
		{
			//Higher gravity if jump button released
			_playerController.SetGravityScale(_playerData.gravityScale * _playerData.jumpCutGravityMult);
			_rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, -_playerData.maxFallSpeed));
		}
		else if ((_playerStates.IsJumping || _playerStates.IsWallJumping || _playerStates.IsFalling) && Mathf.Abs(_rigidbody.linearVelocity.y) < _playerData.jumpHangTimeThreshold)
		{
			_playerController.SetGravityScale(_playerData.gravityScale * _playerData.jumpHangGravityMult);
		}
		else if (_rigidbody.linearVelocity.y < 0)
		{
			//Higher gravity if falling
			_playerController.SetGravityScale(_playerData.gravityScale * _playerData.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, Mathf.Max(_rigidbody.linearVelocity.y, -_playerData.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			_playerController.SetGravityScale(_playerData.gravityScale);
		}
	}

	IEnumerator HandleJump()
	{
		if (CanMove())
		{
			//Jump
			if (CanJump())
				Jump();

			//WallJump
			else if (CanWallJump())
			{
				_wallJumpStartTime = Time.time;

				WallJump();
			}
		}

		yield return new WaitForFixedUpdate();
	}

	IEnumerator HandleDash()
	{
		if (CanDash())
		{
			Vector2 dashDir;

			// NOTE: Currently I dont like this effect, prob will be changed for something more interesting like slowmo || need to be reviewed
			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			// _pc.Sleep(_data.dashDelay);

			//If not direction pressed, dash forward
			if (_moveInput != Vector2.zero)
				dashDir = _moveInput;
			else
				dashDir = _playerStates.IsFacingRight ? Vector2.right : Vector2.left;

			StartCoroutine(Dash(dashDir));
		}
		
		yield return new WaitForFixedUpdate();
	}
	#endregion

	#region Checks (Is able to)
	// Return if the player can do a Movement action
	private bool CanMove()
	{
		// Unable to move when is attacking
		return _playerStates.IsAttacking == 0;
	}


	// Return if the player can slide in a wall right now
	private bool CanSlide()
	{
		if (!CanMove())
			return false;

		// Player cant be in another states
		if (_playerStates.IsWallJumping || _playerStates.IsDashing || _playerStates.IsGrounded)
			return false;

		// Player movement input must be in the same direction at forwardcollider
		if (!(_playerStates.IsCollidedForward && (_playerStates.IsFacingRight ? _moveInput.x > 0 : _moveInput.x < 0)))
			return false;

		// If you are not moving the X (close wall or stopped cases), without consider sliding
		if (!_playerStates.IsSliding && _rigidbody.linearVelocity.x == 0)
		// Can slide only when reach max height or press again a move input
		if (_rigidbody.linearVelocity.y > 0 && !(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
			return false;

		return true;
	}

	private bool CanJump()
	{
		// Avoid perform a normal jump when sliding
		if (_playerStates.IsSliding) return false;

		// Normal jump removes the subtraction on the fall
		if (_playerStates.IsGrounded || Time.time - _jumpCoyoteTime < _playerData.coyoteTime)
			_jumpsLeft++; 

		// If have jumps is able to jump even in air
		return _jumpsLeft > 0;
	}

	private bool CanWallJump()
	{
		// Normal wall jump removes the subtraction on the fall
		if (_playerStates.IsSliding || Time.time - _wallJumpCoyoteTime < _playerData.coyoteTime)
		{
			_jumpsLeft++;
			return true;
		}

		return false;
	}

	private bool CanDash()
	{
		return CanMove() && !_playerStates.IsDashing && Time.time - _dashDelayTime > _playerData.dashDelay;
	}
	#endregion

	#region Actions (All the specific logic)
	private void Jump()
	{
		// Update states when jump
		_jumpCoyoteTime = 0;
		_jumpsLeft--;

		_playerStates.IsJumping = true;

		_playerStates.IsWallJumping = false;
		_playerStates.IsJumpCut = false;
		_playerStates.IsFalling = false;
		_playerStates.IsGrounded = false;

		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = _playerData.jumpForce;
		if (_rigidbody.linearVelocity.y < 0)
			force -= _rigidbody.linearVelocity.y;

		_rigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
	}

	private void WallJump()
	{
		// Update states when start walljumping
		_wallJumpCoyoteTime = 0;
		_jumpsLeft--;

		_playerStates.IsWallJumping = true;

		_playerStates.IsJumping = false;
		_playerStates.IsJumpCut = false;
		_playerStates.IsFalling = false;
		_playerStates.IsSliding = false;

		Vector2 force = new (_playerData.wallJumpForce.x, _playerData.wallJumpForce.y);
		force.x *= _slideIsFacingRight ? -1 : 1; //apply force in opposite direction of wall

		if (Mathf.Sign(_rigidbody.linearVelocity.x) != Mathf.Sign(force.x))
			force.x -= _rigidbody.linearVelocity.x;

		if (_rigidbody.linearVelocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= _rigidbody.linearVelocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		_rigidbody.AddForce(force, ForceMode2D.Impulse);
	}

	private void Slide()
	{
		// Update states when start sliding
		_playerStates.IsSliding = true;

		_playerStates.IsJumping = false;
		_playerStates.IsFalling = false;
		_playerStates.IsWallJumping = false;

		_slideIsFacingRight = _playerStates.IsFacingRight;

		//We remove the remaining upwards Impulse to prevent upwards sliding
		if (_rigidbody.linearVelocity.y > 0)
		{
			_rigidbody.AddForce(-_rigidbody.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);
		}

		//Works the same as the Run but only in the y-axis
		float speedDif = _playerData.slideSpeed - _rigidbody.linearVelocity.y;
		float movement = speedDif * _playerData.slideAccel;

		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		_rigidbody.AddForce(movement * Vector2.up);

	}

	// Method to make run the player in the X axis
	private void Run(float lerpAmount)
	{
		// Update States when start running
		_playerStates.IsRunning = _moveInput.x != 0 && _playerStates.IsGrounded && CanMove(); // Only Run when is grounded


		float targetSpeed = CanMove() ? _moveInput.x * _playerData.runMaxSpeed : 0; // -1~1 * Max Speed
		targetSpeed = Mathf.Lerp(_rigidbody.linearVelocity.x, targetSpeed, lerpAmount); // Get values betwen current.s and target.s (smothness)

		float accelRate;

		// Calculate acceleration and decceleration
		if (_playerStates.IsGrounded)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _playerData.runAccelAmount : _playerData.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _playerData.runAccelAmount * _playerData.accelInAir : _playerData.runDeccelAmount * _playerData.deccelInAir;

		if ((_playerStates.IsJumping || _playerStates.IsWallJumping || _playerStates.IsFalling) && Mathf.Abs(_rigidbody.linearVelocity.y) < _playerData.jumpHangTimeThreshold)
		{
			accelRate *= _playerData.jumpHangAccelerationMult;
			targetSpeed *= _playerData.jumpHangMaxSpeedMult;
		}

		// Preserve the speed of the character, remove any decceleration if have a greater speed than max speed (jump wall case)
		if (_playerData.doConserveMomentum
			&& Mathf.Abs(_rigidbody.linearVelocity.x) > Mathf.Abs(targetSpeed)
			&& Mathf.Sign(_rigidbody.linearVelocity.x) == Mathf.Sign(targetSpeed)
			&& Mathf.Abs(targetSpeed) > 0.01f
			&& !_playerStates.IsGrounded)
			accelRate = 0;

		float speedDif = targetSpeed - _rigidbody.linearVelocity.x; // Get distance between target.s and current.s

		float movement = speedDif * accelRate; // Multiply speedDif by accelRate factor

		_rigidbody.AddForce(movement * Vector2.right, ForceMode2D.Force); // Add movement force into X axis

		/* 
		 * Basic explanation: We detect a number between target speed and current speed (defined lerp amount), by default 1 so is max speed i.e 10 max speed
		 * We get the Delta between taret speed and ours 10 - 10 (our) = 0, 10 - 7 = 3.
		 * This difference is multiplied by our acceleration i.e, so we have higher greater force at early 10, 9, 8, 7 ... 0
		 * All this speed values ared added as force that works like rb.velocity in the following way
		 * rb.velocity.x + SpeedValue * Time.DeltaTime / rb.mass (Mass is 1, doesn't matter)
		 * Is like a log function with a limit in the max speed
		 * So we are incrementing a value (also reduced by her mass), till reach the value 0 (max speed)
		 */
	}

	//Dash Coroutines
	private IEnumerator Dash(Vector2 dir)
	{
		//Overall this method of dashing aims to mimic Celeste, if you're looking for
		// a more physics-based approach try a method similar to that used in the jump

		// Update States when start dashing
		_playerStates.IsDashing = true;

		_playerStates.IsJumping = false;
		_playerStates.IsFalling = false;
		_playerStates.IsWallJumping = false;
		_playerStates.IsJumpCut = false;
		_playerStates.IsGrounded = false;
		_playerStates.IsCollidedForward = false;
		_playerStates.IsCollidedBackward = false;

		float startTime = Time.time;

		//_isDashAttacking = true;

		_playerController.SetGravityScale(0);

		//We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
		while (CanMove() && Time.time - startTime <= _playerData.dashAttackTime)
		{
			_rigidbody.linearVelocity = dir.normalized * _playerData.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 
			//This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
			yield return null;
		}

		startTime = Time.time;

		//_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		_playerController.SetGravityScale(_playerData.gravityScale);
		_rigidbody.linearVelocity = _playerData.dashEndSpeed * dir.normalized;

		while (CanMove() && Time.time - startTime <= _playerData.dashEndTime)
		{
			yield return null;
		}

		//Dash over
		_playerStates.IsDashing = false;
		_dashDelayTime = Time.time;
	}
	#endregion
}
