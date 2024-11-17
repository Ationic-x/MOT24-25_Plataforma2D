using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private PlayerData data; // Player parameters

	// Input variables
	[SerializeField] private Vector2 moveInput;
	[SerializeField] private bool jumpButton;
	[SerializeField] private bool dashButton;

	private PlayerStates ps;
	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start()
	{
		ps = GetComponent<PlayerStates>();
		rb = GetComponent<Rigidbody2D>();
		SetGravityScale(data.gravityScale);
	}

	// Update is called once per frame
	void Update()
	{
		moveInput.x = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;

		if (moveInput.x != 0)
			SwapFacingPosition(moveInput.x > 0);

		jumpButton = Input.GetKey(KeyCode.J);
		dashButton = Input.GetKeyDown(KeyCode.K);

		ps.IsJumpCut = ps.IsJumping && !jumpButton;

		if ((ps.IsJumping || ps.IsJumpFalling) && ps.IsGrounded)
			ps.JumpDelayStartTime = Time.time;

		if (rb.velocity.y < 0 && !ps.IsSliding && !ps.IsDashing)
		{
			ps.IsJumping = false;

			ps.IsJumpFalling = true;
		}

		if (ps.IsWallJumping && Time.time - ps.WallJumpStartTime > data.wallJumpTime)
			ps.IsWallJumping = false;

		if (ps.IsGrounded && !ps.IsJumping && !ps.IsWallJumping)
		{
			ps.IsJumpCut = false;

			ps.IsJumpFalling = false;
		}

		if (!ps.IsDashing)
		{
			//Jump
			if (jumpButton && CanJump())			
				Jump();

			//WallJump
			else if (ps.IsSliding && Input.GetKeyDown(KeyCode.J))
			{
				ps.WallJumpStartTime = Time.time;

				WallJump(ps.IsFacingRight ? -1 : 1);
			}
		}

		if (CanDash() && dashButton)
		{
			Vector2 dashDir;

			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			Sleep(data.dashSleepTime);

			//If not direction pressed, dash forward
			if (moveInput != Vector2.zero)
				dashDir = moveInput;
			else
				dashDir = ps.IsFacingRight ? Vector2.right : Vector2.left;

			// Update when dashing (outside the coroutine this is called once)
			ps.IsDashing = true;
			ps.IsJumping = false;
			ps.IsJumpFalling = false;
			ps.IsWallJumping = false;
			ps.IsJumpCut = false;
			ps.IsGrounded = false;
			ps.IsCollidedRight = false;
			ps.IsCollidedLeft = false;

			StartCoroutine(nameof(StartDash), dashDir);
		}

		ps.IsSliding = CanSlide();

		//Higher gravity if we've released the jump input or are falling
		if (ps.IsSliding)
		{
			SetGravityScale(0);
		}
		else if (rb.velocity.y < 0 && moveInput.y < 0)
		{
			//Much higher gravity if holding down
			SetGravityScale(data.gravityScale * data.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFastFallSpeed));
		}
		else if (ps.IsJumpCut)
		{
			//Higher gravity if jump button released
			SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
		}
		else if ((ps.IsJumping || ps.IsWallJumping || ps.IsJumpFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
		{
			SetGravityScale(data.gravityScale * data.jumpHangGravityMult);
		}
		else if (rb.velocity.y < 0)
		{
			//Higher gravity if falling
			SetGravityScale(data.gravityScale * data.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -data.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			SetGravityScale(data.gravityScale);
		}
	}

	private void FixedUpdate()
	{
		//Handle Run
		if (!ps.IsDashing)
		{
			if (ps.IsWallJumping)
				Run(data.wallJumpRunLerp);
			else
				Run(1);
		}
		//else if (_isDashAttacking)
		//{
		//	Run(Data.dashEndRunLerp);
		//}

		//Handle Slide
		if (ps.IsSliding)
			Slide();
	}

	// Return if the player can slide in a wall right now
	private bool CanSlide()
	{

		// Player cant be in another states
		if (ps.IsWallJumping || ps.IsDashing || ps.IsGrounded)
			return false;
		 
		// Player movement input must be in the same at collider
		if (!(ps.IsCollidedLeft && moveInput.x < 0) && !(ps.IsCollidedRight && moveInput.x > 0))
			return false;

		// If you are not moving the X (close wall or stopped cases), without consider sliding
		if (!ps.IsSliding && rb.velocity.x == 0)
			// Can slide only when reach max height or press again a move inputn
			if (rb.velocity.y > 0 && !(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
				return false;

		return true;
	}

	private bool CanJump()
	{
		// Player must be in the floor
		if (!ps.IsGrounded || ps.IsJumping || ps.IsSliding)
			return false;

		// Player must be in the floor a minimum of time
		if (ps.JumpDelayStartTime - Time.time > -data.delayJump)
			return false;

		return true;
	}

	private bool CanDash()
	{
		if (!ps.IsDashing && ps.DashesLeft < data.dashAmount && ps.IsGrounded && !ps.IsDashRefilling)
		{
			StartCoroutine(nameof(RefillDash), 1);
		}

		return ps.DashesLeft > 0;
	}

	private void SwapFacingPosition(bool facingRight)
	{
		// Avoid flip before the animation end
		if (ps.IsSliding || ps.IsDashing) return;

		if (ps.IsFacingRight != facingRight)
		{
			ps.IsFacingRight = facingRight;
			
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
	}

	private void Jump()
	{
		// Update states when jump
		ps.IsJumping = true;
		ps.IsWallJumping = false;
		ps.IsJumpCut = false;
		ps.IsJumpFalling = false;
		ps.IsGrounded = false;

		//We increase the force applied if we are falling
		//This means we'll always feel like we jump the same amount 
		//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
		float force = data.jumpForce;
		if (rb.velocity.y < 0)
			force -= rb.velocity.y;

		rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
	}

	private void Slide()
	{
		//We remove the remaining upwards Impulse to prevent upwards sliding
		if (rb.velocity.y > 0)
		{
			rb.AddForce(-rb.velocity.y * Vector2.up, ForceMode2D.Impulse);
		}

		//Works the same as the Run but only in the y-axis
		float speedDif = data.slideSpeed - rb.velocity.y;
		float movement = speedDif * data.slideAccel;
		
		//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
		//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		rb.AddForce(movement * Vector2.up);

		// Remove jumping state and jump falling
		ps.IsJumping = false;
		ps.IsJumpFalling = false;
		ps.IsWallJumping = false;
	}

	private void Sleep(float duration)
	{
		//Method used so we don't need to call StartCoroutine everywhere
		//nameof() notation means we don't need to input a string directly.
		//Removes chance of spelling mistakes and will improve error messages if any
		StartCoroutine(nameof(PerformSleep), duration);
	}

	private IEnumerator PerformSleep(float duration)
	{
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime(duration); //Must be Realtime since timeScale with be 0 
		Time.timeScale = 1;
	}

	// Method to make run the player in the X axis
	private void Run(float lerpAmount)
	{
		float targetSpeed = moveInput.x * data.runMaxSpeed; // -1~1 * Max Speed
		targetSpeed = Mathf.Lerp(rb.velocity.x, targetSpeed, lerpAmount); // Get values betwen current.s and target.s (smothness)

		float accelRate;

		// Calculate acceleration and decceleration
		if (ps.IsGrounded)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;

		if ((ps.IsJumping || ps.IsWallJumping || ps.IsJumpFalling) && Mathf.Abs(rb.velocity.y) < data.jumpHangTimeThreshold)
		{
			accelRate *= data.jumpHangAccelerationMult;
			targetSpeed *= data.jumpHangMaxSpeedMult;
		}

		// Preserve the speed of the character, remove any decceleration if have a greater speed than max speed (jump wall case)
		if (data.doConserveMomentum 
			&& Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) 
			&& Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) 
			&& Mathf.Abs(targetSpeed) > 0.01f 
			&& !ps.IsGrounded)
			accelRate = 0;

		float speedDif = targetSpeed - rb.velocity.x; // Get distance between target.s and current.s

		float movement = speedDif * accelRate; // Multiply speedDif by accelRate factor

		rb.AddForce(movement * Vector2.right, ForceMode2D.Force); // Add movement force into X axis

		ps.IsRunning = moveInput.x != 0 && ps.IsGrounded; // Change State

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

	private void WallJump(int dir)
	{
		// Update states when walljump
		ps.IsWallJumping = true;
		ps.IsJumping = false;
		ps.IsJumpCut = false;
		ps.IsJumpFalling = false;
		ps.IsSliding = false;

		Vector2 force = new Vector2(data.wallJumpForce.x, data.wallJumpForce.y);
		force.x *= dir; //apply force in opposite direction of wall

		if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(force.x))
			force.x -= rb.velocity.x;

		if (rb.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
			force.y -= rb.velocity.y;

		//Unlike in the run we want to use the Impulse mode.
		//The default mode will apply are force instantly ignoring masss
		rb.AddForce(force, ForceMode2D.Impulse);
	}

	public void SetGravityScale(float scale)
	{
		rb.gravityScale = scale;
	}

	//Dash Coroutine
	private IEnumerator StartDash(Vector2 dir)
	{
		//Overall this method of dashing aims to mimic Celeste, if you're looking for
		// a more physics-based approach try a method similar to that used in the jump

		ps.IsGrounded = false;
		ps.IsDashing = true;

		float startTime = Time.time;

		ps.DashesLeft--;
		//_isDashAttacking = true;

		SetGravityScale(0);

		//We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
		while (Time.time - startTime <= data.dashAttackTime)
		{
			rb.velocity = dir.normalized * data.dashSpeed;
			//Pauses the loop until the next frame, creating something of a Update loop. 
			//This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
			yield return null;
		}

		startTime = Time.time;

		//_isDashAttacking = false;

		//Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
		//SetGravityScale(Data.gravityScale);
		rb.velocity = data.dashEndSpeed * dir.normalized;

		while (Time.time - startTime <= data.dashEndTime)
		{
			yield return null;
		}

		//Dash over
		ps.IsDashing = false;
	}

	//Short period before the player is able to dash again
	private IEnumerator RefillDash(int amount)
	{
		//SHoet cooldown, so we can't constantly dash along the ground, again this is the implementation in Celeste, feel free to change it up
		ps.IsDashRefilling = true;
		yield return new WaitForSeconds(data.dashRefillTime);
		ps.IsDashRefilling = false;
		ps.DashesLeft = Mathf.Min(data.dashAmount, ps.DashesLeft + amount);
	}
}
