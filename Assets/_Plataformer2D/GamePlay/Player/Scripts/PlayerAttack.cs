using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : ActorAttack<PlayerAttack.AttackType>
{
	public enum AttackType
	{
		None = 0,
		Basic,
		Quick,
		Big,
		Ranged,
	}


	// Input Variables
	[SerializeField] private bool _attack1;
	[SerializeField] private AnimationClip _attack1Clip;
	private PlayerController _playerController;

	protected override void Awake()
	{
		actorController = GetComponent<PlayerController>();
		_playerController = (PlayerController)actorController;
		actorStates = _playerController.PlayerStates;
		base.Awake();
	}

	protected override void OnEnable()
	{
		base.OnEnable();

		// Input Events
		_playerController.AttackAction.action.performed += OnAttackInput;
		_playerController.AttackAction.action.canceled += OnAttackInput;
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		// Input Events
		_playerController.AttackAction.action.performed -= OnAttackInput;
		_playerController.AttackAction.action.canceled -= OnAttackInput;
	}

	private void OnAttackInput(InputAction.CallbackContext context = default)
	{
		if (actorStates.IsAttacking != 0) return;

		_attack1 = context.performed;
		if (_attack1)
				OnAttack(AttackType.Basic);
	}
}
