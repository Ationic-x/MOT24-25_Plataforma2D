using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : ActorController
{
	[SerializeField] private PlayerData _playerData; // Player parameters

	// Input variables
	[SerializeField] private bool _jumpPressed;
	[SerializeField] private bool _dashPressed;

	[SerializeField] private PlayerStates _playerStates;
	private Rigidbody2D _rigidbody;

    [Header("Actions")]
    [SerializeField] private InputActionReference _jumpAction;
	[SerializeField] private InputActionReference _moveAction;
	[SerializeField] private InputActionReference _dashAction;
	[SerializeField] private InputActionReference _attackAction;

	// Start is called before the first frame update
	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();

		SetGravityScale(_playerData.gravityScale);
	}

	public void SwapFacingPosition(bool isFacingRight)
	{
		// Avoid flip before the animation end
		if (_playerStates.IsSliding || _playerStates.IsDashing) return;

		if (_playerStates.IsFacingRight != isFacingRight)
		{
			_playerStates.IsFacingRight = isFacingRight;
			transform.localScale = new (-transform.localScale.x, transform.localScale.y);
		}
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
		_rigidbody.gravityScale = scale;
	}

	public PlayerStates PlayerStates { get => _playerStates; }
	public Rigidbody2D Rigidbody { get => _rigidbody; }
	public PlayerData PlayerData { get => _playerData; }
	public InputActionReference MoveAction { get => _moveAction; }
	public InputActionReference JumpAction { get => _jumpAction; }
	public InputActionReference DashAction { get => _dashAction; }
	public InputActionReference AttackAction { get => _attackAction; }
}
