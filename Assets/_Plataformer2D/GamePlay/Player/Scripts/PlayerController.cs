using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : ActorController
{
	[SerializeField] private PlayerData _data; // Player parameters
	private PlayerInput _pi;

	// Input variables
	[SerializeField] private bool _jumpPressed;
	[SerializeField] private bool _dashPressed;

	[SerializeField] private PlayerStates _ps;
	private Rigidbody2D _rb;

    [Header("Actions")]
    [SerializeField] private InputActionReference _jumpAction;
	[SerializeField] private InputActionReference _moveAction;
	[SerializeField] private InputActionReference _dashAction;

	// Start is called before the first frame update
	void Awake()
	{

		_pi = GetComponent<PlayerInput>();
		_rb = GetComponent<Rigidbody2D>();

		SetGravityScale(_data.gravityScale);
	}

	public void SwapFacingPosition(bool isFacingRight)
	{
		// Avoid flip before the animation end
		if (_ps.IsSliding || _ps.IsDashing) return;

		if (_ps.IsFacingRight != isFacingRight)
			_ps.IsFacingRight = isFacingRight;
	}

	public void Sleep(float duration)
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

	public PlayerStates PS { get { return _ps; } }
	public Rigidbody2D RB { get { return _rb; } }
	public PlayerData Data { get { return _data; } }
	public InputActionReference MoveAction { get { return _moveAction; } }
	public InputActionReference JumpAction { get { return _jumpAction; } }
	public InputActionReference DashAction { get { return _dashAction; } }
}
