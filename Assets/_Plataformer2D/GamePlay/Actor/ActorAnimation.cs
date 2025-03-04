using System;
using UnityEngine;

public abstract class ActorAnimation : MonoBehaviour
{

	protected Animator animator;
	protected SpriteRenderer spriteRenderer;
	[SerializeField] protected string currentSprite;
	[SerializeField] protected AnimationClip currentClip;

	public event Action<string> OnSpriteChange;
	public event Action<AnimationClip> OnClipChange;

	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected virtual void OnEnable()
	{
		currentSprite = spriteRenderer.sprite.name;
		currentClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
		SubscribeToEvents();
	}

	protected virtual void OnDisable()
	{
		UnsubscribeFromEvents();
	}

	protected abstract void SubscribeToEvents();
	protected abstract void UnsubscribeFromEvents();

	protected virtual void Update()
	{
		UpdateClipAndSprite();
	}

	private void UpdateClipAndSprite()
	{
		string spriteName = spriteRenderer.sprite.name;
		AnimationClip clipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip;

		if (!currentClip.Equals(clipName))
		{
			currentClip = clipName;
			OnClipChange?.Invoke(currentClip);
		}

		if (!currentSprite.Equals(spriteName))
		{
			currentSprite = spriteName;
			OnSpriteChange?.Invoke(currentSprite);
		}
	}
}
