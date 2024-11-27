using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	[SerializeField] protected PlayerData _data; // Player parameters
	private PlayerInput _pi;

	// Input variables
	[SerializeField] protected Vector2 _moveInput;
	[SerializeField] private bool _jumpPressed;
	[SerializeField] private bool _dashPressed;

	protected PlayerStates _ps;
	protected Rigidbody2D _rb;

    [Header("Actions")]
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference moveAction;

    // Start is called before the first frame update
    void Awake()
	{
        _pi = GetComponent<PlayerInput>();
		_ps = GetComponent<PlayerStates>();
		_rb = GetComponent<Rigidbody2D>();
		SetGravityScale(_data.gravityScale);
	}

	// Update is called once per frame
	void Update()
	{
		float tempX = moveAction.action.ReadValue<Vector2>().x;
        _moveInput.x = tempX > 0 ? 1 : tempX  < 0 ? -1 : 0;

        if (_moveInput.x != 0)
			SwapFacingPosition(_moveInput.x > 0);

		_jumpPressed = jumpAction.action.IsPressed();
		_dashPressed = Input.GetKeyDown(KeyCode.J);

		_ps.IsJumpCut = _ps.IsJumping && !_jumpPressed;

		if ((_ps.IsJumping || _ps.IsJumpFalling) && _ps.IsGrounded)
			_ps.JumpDelayStartTime = Time.time;

		if (_rb.linearVelocity.y < 0 && !_ps.IsSliding && !_ps.IsDashing)
		{
			_ps.IsJumping = false;

			_ps.IsJumpFalling = true;
		}

		if (_ps.IsWallJumping && Time.time - _ps.WallJumpStartTime > _data.wallJumpTime)
			_ps.IsWallJumping = false;

		if (_ps.IsGrounded && !_ps.IsJumping && !_ps.IsWallJumping)
		{
			_ps.IsJumpCut = false;

			_ps.IsJumpFalling = false;
		}

		if (!_ps.IsDashing)
		{
			//Jump
			if (_jumpPressed && CanJump())			
				Jump();

			//WallJump
			else if (_ps.IsSliding && jumpAction.action.triggered)
			{
				_ps.WallJumpStartTime = Time.time;

				WallJump(_ps.IsFacingRight ? -1 : 1);
			}
		}

		if (CanDash() && _dashPressed)
		{
			Vector2 dashDir;

			//Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
			Sleep(_data.dashSleepTime);

			//If not direction pressed, dash forward
			if (_moveInput != Vector2.zero)
				dashDir = _moveInput;
			else
				dashDir = _ps.IsFacingRight ? Vector2.right : Vector2.left;

			// Update when dashing (outside the coroutine this is called once)
			_ps.IsDashing = true;
			_ps.IsJumping = false;
			_ps.IsJumpFalling = false;
			_ps.IsWallJumping = false;
			_ps.IsJumpCut = false;
			_ps.IsGrounded = false;
			_ps.IsCollidedRight = false;
			_ps.IsCollidedLeft = false;

			StartCoroutine(nameof(StartDash), dashDir);
		}

		_ps.IsSliding = CanSlide();

		//Higher gravity if we've released the jump input or are falling
		if (_ps.IsSliding)
		{
			SetGravityScale(0);
		}
		else if (_rb.linearVelocity.y < 0 && _moveInput.y < 0)
		{
			//Much higher gravity if holding down
			SetGravityScale(_data.gravityScale * _data.fastFallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFastFallSpeed));
		}
		else if (_ps.IsJumpCut)
		{
			//Higher gravity if jump button released
			SetGravityScale(_data.gravityScale * _data.jumpCutGravityMult);
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFallSpeed));
		}
		else if ((_ps.IsJumping || _ps.IsWallJumping || _ps.IsJumpFalling) && Mathf.Abs(_rb.linearVelocity.y) < _data.jumpHangTimeThreshold)
		{
			SetGravityScale(_data.gravityScale * _data.jumpHangGravityMult);
		}
		else if (_rb.linearVelocity.y < 0)
		{
			//Higher gravity if falling
			SetGravityScale(_data.gravityScale * _data.fallGravityMult);
			//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
			_rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -_data.maxFallSpeed));
		}
		else
		{
			//Default gravity if standing on a platform or moving upwards
			SetGravityScale(_data.gravityScale);
		}
	}

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

	private void SwapFacingPosition(bool facingRight)
	{
		// Avoid flip before the animation end
		if (_ps.IsSliding || _ps.IsDashing) return;

		if (_ps.IsFacingRight != facingRight)
		{
			_ps.IsFacingRight = facingRight;
			
			Vector3 newScale = transform.localScale;
			newScale.x *= -1;
			transform.localScale = newScale;
		}
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

	public void SetGravityScale(float scale)
	{
		_rb.gravityScale = scale;
	}

	// Interface methods
    public void TakeDamage(float dmg, GameObject src)
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float hl)
    {
        throw new System.NotImplementedException();
    }
}
