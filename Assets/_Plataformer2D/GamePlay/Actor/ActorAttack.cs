using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Attack
{
	private static uint _counter;

	public readonly uint id;
    public readonly int damage;
    public readonly float pushForce;
	// In future we can add status effect

	public Attack(int newDamage, float newPushForce)
	{
		id = ++_counter;
        damage = newDamage;
        pushForce = newPushForce;
	}
}

public abstract class ActorAttack<T> : MonoBehaviour where T : Enum
{
	[SerializeField] protected List<AttackData> listAttacks;
	[SerializeField] protected AttackData currentAttackData;
	
	protected ActorController actorController;
	protected ActorStates<T> actorStates;
	protected IStats stats;
	
	protected ActorAnimation actorAnimation;
	protected AnimationClip currentClip;
	protected string currentSprite;

	protected virtual void Awake()
	{
		stats = actorController.Stats;
		actorAnimation = GetComponentInChildren<ActorAnimation>();
	}

	protected virtual void OnEnable()
	{
		actorAnimation.OnClipChange += OnClipChange;
		actorAnimation.OnSpriteChange += OnSpriteChange;
	} 
	
	protected virtual void OnDisable()
	{
		actorAnimation.OnClipChange -= OnClipChange;
		actorAnimation.OnSpriteChange -= OnSpriteChange;
	} 

	protected void OnClipChange(AnimationClip newClip)
	{
		currentClip = newClip;
		if (currentClip.name.Contains("Attack"))
			StartCoroutine(HandleAttack());
	}

	protected void OnSpriteChange(string newSprite)
	{
		currentSprite = newSprite;
	}

	protected void OnAttack(T attack)
	{
		actorStates.IsAttacking = attack;
		currentAttackData = listAttacks[Convert.ToInt32(attack)];
		actorController.SetAttack(
			new Attack(
				(int)(stats.Magic * currentAttackData.magicalDamageScale + stats.Physic * currentAttackData.physicalDamage),
				stats.Push * currentAttackData.pushForceScale
			)
		);
	}

	IEnumerator HandleAttack()
	{
		// Wait animation duration in frames
		yield return new WaitForSeconds(currentClip.length);

		actorStates.IsAttacking = (T)Enum.ToObject(typeof(T), 0);
	}
}
