using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement2D : MonoBehaviour
{
	private PlayerController _pc;
	
	private PlayerStates _ps;
	private Rigidbody2D _rb;
	private PlayerData _data;


	// Input Variables
	[SerializeField] private Vector2 _moveInput;
	[SerializeField] private bool _jumpInput;
	[SerializeField] private bool _dashInput;

	// Movement states
	private float _wallJumpStartTime;
	[SerializeField] private float _jumpCoyoteTime;
	[SerializeField] private float _wallJumpCoyoteTime;
	
	private bool _slideIsFacingRight;

	// Movement count
	private int _jumpsLeft;

	private void Awake()
	{
		_pc = GetComponent<PlayerController>();
	}

	private void Start()
	{
		_rb = _pc.RB;
		_data = _pc.Data;

		_jumpsLeft = _data.jumpAmount; // TODO: locked max jump amount (require perk)
	}

	#region Events
	private void OnEnable()
    {
		_ps = _pc.PS;

		// Input Events
		_pc.MoveAction.action.performed += OnMoveInput;
		_pc.MoveAction.action.canceled += OnMoveInput;

		_pc.JumpAction.action.performed += OnJumpInput;
		_pc.JumpAction.action.canceled += OnJumpInput;
		
		_pc.DashAction.action.performed += OnDashInput;
		_pc.DashAction.action.canceled += OnDashInput;

		// State events
		_ps.OnSlidingChanged += OnSlidingChanged;
		_ps.OnGroundedChanged += OnGroundedChanged;
	}

    private void OnDisable()
    {
		// Input Events
		_pc.MoveAction.action.performed -= OnMoveInput;
		_pc.MoveAction.action.canceled -= OnMoveInput;

		_pc.JumpAction.action.performed -= OnJumpInput;
		_pc.JumpAction.action.canceled -= OnJumpInput;

		_pc.DashAction.action.performed -= OnDashInput;
		_pc.DashAction.action.canceled -= OnDashInput;

		//	State events
		_ps.OnSlidingChanged -= OnSlidingChanged;
		_ps.OnGroundedChanged -= OnGroundedChanged;
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
			_jumpsLeft = _data.jumpAmount;
		else
		{
			_jumpCoyoteTime = Time.time;
			_jumpsLeft--;
		}
	}
	
	private void OnSlidingChanged(bool isSliding)
	{
		if (isSliding)
			_jumpsLeft = _data.jumpAmount;
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
		if (_moveInput.x != 0)
			_pc.SwapFacingPosition(_moveInput.x > 0);

		HandleStates();
		HandleGravity();
	}

	// All the behaviour managed per update of rb
	// Currently Run and Walking
	private void FixedUpdate()
	{
		//Handle Run
		if (!_ps.IsDashing)
		{
			if (_ps.IsWallJumping)
				Run(_data.wallJumpRunLerp);
			else
				Run(1);
		}
		//else if (_isDashAttacking)
		//{
		//	Run(Data.dashEndRunLerp);
		//}

		//Handle Slide
		if (_ps.IsSliding)
			Slide();
	}
	

	#region Handle Behaviour (A bridge between actions and general behaviour)
	private void HandleStates()
	{
		_ps.IsSliding = CanSlide();

		_ps.IsJumpCut = _ps.IsJumping && !_jumpInput;

		if (_rb.linearVelocity.y < 0 && !_ps.IsSliding && !_ps.IsDashing)
		{
			_ps.IsJumping = false;

			_ps.IsFalling = true;
		}

		if (_ps.IsWallJumping && Time.time - _wallJumpStartTime > _data.wallJumpTime)
			_ps.IsWallJumping = false;

		if (_ps.IsGrounded && !_ps.IsJumping && !_ps.IsWallJumping)
		{
			_ps.IsJumpCut = false;

			_ps.IsFalling = false;
		}
	}

	private void HandleGravity()
	{
		//Higher gravity if we've released the jump input or are falling
		if (_ps.IsSliding)
		{
			_pc.SetGravityScale(0);
		}
		else if (_rb.linearVelocity.y < 0 && _moveInput.y < 0)
		{
			//Much higher gravity if holding down
			_pc.SetGravityScale(_data.gravityScale * _data.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFastFallSpeed));
		}
		else if (_ps.IsJumpCut)
		{
			//Higher gravity if jump button released
			_pc.SetGravityScale(_data.gravityScale * _data.jumpCutGravityMult);
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFallSpeed));
		}
		else if ((_ps.IsJumping || _ps.IsWallJumping || _ps.IsFalling) && Mathf.Abs(_rb.linearVelocity.y) < _data.jumpHangTimeThreshold)
		{
			_pc.SetGravityScale(_data.gravityScale * _data.jumpHangGravityMult);
		}
		else if (_rb.linearVelocity.y < 0)
		{
			//Higher gravity if falling
			_pc.SetGravityScale(_data.gravityScale * _data.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			_pc.SetGravityScale(_data.gravityScale);
		}
	}

	IEnumerator HandleJump()
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

		yield return new WaitForFixedUpdate();
	}

	IEnumerator HandleDash()
	{
		if (CanDash())
		{
			Vector2 dashDir;

			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			_pc.Sleep(_data.dashSleepTime);

			//If not direction pressed, dash forward
			if (_moveInput != Vector2.zero)
				dashDir = _moveInput;
			else
				dashDir = _ps.IsFacingRight ? Vector2.right : Vector2.left;

			StartCoroutine(nameof(Dash), dashDir);
		}
		
		yield return new WaitForFixedUpdate();
	}
	#endregion

	#region Checks (Is able to)
	// Return if the player can slide in a wall right now
	private bool CanSlide()
	{
		// Player cant be in another states
		if (_ps.IsWallJumping || _ps.IsDashing || _ps.IsGrounded)
			return false;

		// Player movement input must be in the same at collider
		if (!(_ps.IsCollidedLeft && _moveInput.x < 0) && !(_ps.IsCollidedRight && _moveInput.x > 0))
			return false;

		// If you are not moving the X (close wall or stopped cases), without consider sliding
		if (!_ps.IsSliding && _rb.linearVelocity.x == 0)
			// Can slide only when reach max height or press again a move inputn
			if (_rb.linearVelocity.y > 0 && !(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
				return false;

		return true;
	}

	private bool CanJump()
	{
		// Avoid perform a normal jump when sliding
		if (_ps.IsSliding) return false;

		// Normal jump removes the subtraction on the fall
		if (_ps.IsGrounded || Time.time - _jumpCoyoteTime < _data.coyoteTime)
			_jumpsLeft++; 

		// If have jumps is able to jump even in air
		return _jumpsLeft > 0;
	}

	private bool CanWallJump()
	{
		// Normal wall jump removes the subtraction on the fall
		if (_ps.IsSliding || Time.time - _wallJumpCoyoteTime < _data.coyoteTime)
		{
			_jumpsLeft++;
			return true;
		}

		return false;
	}

	private bool CanDash()
	{
		// FIXME: Add more conditions between 'dashable'
		return !_ps.IsDashing;
	}
	#endregion

	#region Actions (All the specific logic)
	private void Jump()
	{
		// Update states when jump
		_jumpCoyoteTime = 0;
		_jumpsLeft--;

		_ps.IsJumping = true;

		_ps.IsWallJumping = false;
		_ps.IsJumpCut = false;
		_ps.IsFalling = false;
		_ps.IsGrounded = false;

		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = _data.jumpForce;
		if (_rb.linearVelocity.y < 0)
			force -= _rb.linearVelocity.y;

		_rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
	}

	private void WallJump()
	{
		// Update states when start walljumping
		_wallJumpCoyoteTime = 0;
		_jumpsLeft--;

		_ps.IsWallJumping = true;

		_ps.IsJumping = false;
		_ps.IsJumpCut = false;
		_ps.IsFalling = false;
		_ps.IsSliding = false;

		Vector2 force = new Vector2(_data.wallJumpForce.x, _data.wallJumpForce.y);
		force.x *= _slideIsFacingRight ? -1 : 1; //apply force in opposite direction of wall

		if (Mathf.Sign(_rb.linearVelocity.x) != Mathf.Sign(force.x))
			force.x -= _rb.linearVelocity.x;

		if (_rb.linearVelocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= _rb.linearVelocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		_rb.AddForce(force, ForceMode2D.Impulse);
	}

	private void Slide()
	{
		// Update states when start sliding
		_ps.IsSliding = true;

		_ps.IsJumping = false;
		_ps.IsFalling = false;
		_ps.IsWallJumping = false;

		_slideIsFacingRight = _ps.IsFacingRight;

		//We remove the remaining upwards Impulse to prevent upwards sliding
		if (_rb.linearVelocity.y > 0)
		{
			_rb.AddForce(-_rb.linearVelocity.y * Vector2.up, ForceMode2D.Impulse);
		}

		//Works the same as the Run but only in the y-axis
		float speedDif = _data.slideSpeed - _rb.linearVelocity.y;
		float movement = speedDif * _data.slideAccel;

		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		_rb.AddForce(movement * Vector2.up);

	}

	// Method to make run the player in the X axis
	private void Run(float lerpAmount)
	{
		// Update States when start running
		_ps.IsRunning = _moveInput.x != 0 && _ps.IsGrounded; // Only Run when is grounded


		float targetSpeed = _moveInput.x * _data.runMaxSpeed; // -1~1 * Max Speed
		targetSpeed = Mathf.Lerp(_rb.linearVelocity.x, targetSpeed, lerpAmount); // Get values betwen current.s and target.s (smothness)

		float accelRate;

		// Calculate acceleration and decceleration
		if (_ps.IsGrounded)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount : _data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data.runAccelAmount * _data.accelInAir : _data.runDeccelAmount * _data.deccelInAir;

		if ((_ps.IsJumping || _ps.IsWallJumping || _ps.IsFalling) && Mathf.Abs(_rb.linearVelocity.y) < _data.jumpHangTimeThreshold)
		{
			accelRate *= _data.jumpHangAccelerationMult;
			targetSpeed *= _data.jumpHangMaxSpeedMult;
		}

		// Preserve the speed of the character, remove any decceleration if have a greater speed than max speed (jump wall case)
		if (_data.doConserveMomentum
			&& Mathf.Abs(_rb.linearVelocity.x) > Mathf.Abs(targetSpeed)
			&& Mathf.Sign(_rb.linearVelocity.x) == Mathf.Sign(targetSpeed)
			&& Mathf.Abs(targetSpeed) > 0.01f
			&& !_ps.IsGrounded)
			accelRate = 0;

		float speedDif = targetSpeed - _rb.linearVelocity.x; // Get distance between target.s and current.s

		float movement = speedDif * accelRate; // Multiply speedDif by accelRate factor

		_rb.AddForce(movement * Vector2.right, ForceMode2D.Force); // Add movement force into X axis

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
		_ps.IsDashing = true;

		_ps.IsJumping = false;
		_ps.IsFalling = false;
		_ps.IsWallJumping = false;
		_ps.IsJumpCut = false;
		_ps.IsGrounded = false;
		_ps.IsCollidedRight = false;
		_ps.IsCollidedLeft = false;

		float startTime = Time.time;

		//_isDashAttacking = true;

		_pc.SetGravityScale(0);

		//We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
		while (Time.time - startTime <= _data.dashAttackTime)
		{
			_rb.linearVelocity = dir.normalized * _data.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 
			//This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
			yield return null;
		}

		startTime = Time.time;

		//_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		_pc.SetGravityScale(_data.gravityScale);
		_rb.linearVelocity = _data.dashEndSpeed * dir.normalized;

		while (Time.time - startTime <= _data.dashEndTime)
		{
			yield return null;
		}

		//Dash over
		_ps.IsDashing = false;
	}
	#endregion
}
