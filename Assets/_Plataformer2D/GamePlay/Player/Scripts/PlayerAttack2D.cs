using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack2D : MonoBehaviour
{
	private PlayerController _playerController;

	private PlayerStates _playerStates;
	private Rigidbody2D _rigidbody;
	private PlayerData _playerData;


	// Input Variables
	[SerializeField] private bool _attack1;
	[SerializeField] private AnimationClip _attack1Clip;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
	}

	private void Start()
	{
		_rigidbody = _playerController.Rigidbody;
		_playerData = _playerController.PlayerData;
	}

	private void OnEnable()
	{
		_playerStates = _playerController.PlayerStates;

		// Input Events
		_playerController.AttackAction.action.performed += OnAttackInput;
		_playerController.AttackAction.action.canceled += OnAttackInput;
	}

	private void OnDisable()
	{
		// Input Events
		_playerController.AttackAction.action.performed -= OnAttackInput;
		_playerController.AttackAction.action.canceled -= OnAttackInput;
	}

	private void OnAttackInput(InputAction.CallbackContext context = default)
	{
		_attack1 = context.performed;
		if (_attack1)
			StartCoroutine(HandleAttack());
	}

	IEnumerator HandleAttack()
	{
		_playerStates.IsAttacking = true;
		
		// Wait animation duration in frames
		yield return new WaitForSeconds(_attack1Clip.length);

		_playerStates.IsAttacking = false;
	}
}
