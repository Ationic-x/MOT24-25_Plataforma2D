using UnityEngine;
using System;

struct Attack
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
	protected ActorController actorController;
	protected IStats stats;
	protected ActorStates<T> actorStates;
	protected ActorAnimation actorAnimation;
	protected AnimationClip currentClip;
	protected string currentSprite;

	protected virtual void Awake()
	{
		stats = actorController.Stats;
	}

	protected virtual void OnEnable()
	{
		actorAnimation.OnClipChange += OnClipChange;
		actorAnimation.OnSpriteChange += OnSpriteChange;
	} 

	protected void OnClipChange(AnimationClip newClip)
	{
		currentClip = newClip;
	}

	protected void OnSpriteChange(string newSprite)
	{
		currentSprite = newSprite;
	}
}
