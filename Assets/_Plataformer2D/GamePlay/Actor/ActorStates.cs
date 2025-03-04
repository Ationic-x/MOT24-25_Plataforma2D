using System;
using UnityEngine;

public abstract class ActorStates<T> where T: Enum
{
	// In future more states will be added,
	// depending on how complex the enemy state become and how many traits share with the player state. 

	[SerializeField] protected T isAttacking;

	// General States
	[SerializeField] protected bool isFacingRight = true; // By default the character is facing right

	// Events
	public event Action<bool> OnFacingRightChanged;
	public event Action<T> OnAttackChanged;

	#region Getters And Setters
	// General States
	public bool IsFacingRight
	{
		get => isFacingRight;
		set
		{
			if (isFacingRight != value)
			{
				isFacingRight = value;
				OnFacingRightChanged?.Invoke(isFacingRight);
			}
		}
	}

	public T IsAttacking
	{
		get => isAttacking;
		set
		{
			isAttacking = value;
			OnAttackChanged?.Invoke(isAttacking);
		}
	}
	#endregion
}
