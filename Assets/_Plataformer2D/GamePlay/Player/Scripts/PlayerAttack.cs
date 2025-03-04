using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
	public enum AttackType
	{
		None = 0,
		Basic,
		Quick,
		Big,
		Ranged,
	}


	private PlayerController _playerController;

	private PlayerStates _playerStates;
	private PlayerData _playerData;
	[SerializeField] private List<AttackData> _listAttacks;


	// Input Variables
	[SerializeField] private bool _attack1;
	[SerializeField] private AnimationClip _attack1Clip;

	private void Awake()
	{
		_playerController = GetComponent<PlayerController>();
	}

	private void Start()
	{
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
			StartCoroutine(HandleAttack(_attack1Clip.length, AttackType.Big));
	}

	IEnumerator HandleAttack(float duration, AttackType attack)
	{
		_playerStates.IsAttacking = attack;
		
		// Wait animation duration in frames
		yield return new WaitForSeconds(duration);

		_playerStates.IsAttacking = AttackType.None;
	}
}
