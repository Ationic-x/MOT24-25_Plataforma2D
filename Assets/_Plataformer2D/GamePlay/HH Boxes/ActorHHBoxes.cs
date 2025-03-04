using System;
using UnityEngine;

public abstract class ActorHHBoxes: MonoBehaviour
{
	[SerializeField] protected bool drawGizmo;
	[SerializeField] protected LayerMask collisionLayer;
	[SerializeField] protected ActorAnimation actorAnimation;
	protected string currentClip;
	protected string currentSprite;
	protected HHBoxesController hhBoxesController;
	protected IHHBoxesData hhBoxesDataInstance;

	protected virtual void Awake()
	{
		hhBoxesController = new HHBoxesController(hhBoxesDataInstance, transform, collisionLayer);
	}

	protected virtual void OnEnable()
	{
		actorAnimation.OnSpriteChange += OnSpriteChange;
		actorAnimation.OnClipChange += OnClipChange;
	}

	protected virtual void OnDisable()
	{
		actorAnimation.OnSpriteChange += OnSpriteChange;
		actorAnimation.OnClipChange += OnClipChange;
	}

	protected void OnSpriteChange(string sprite)
	{
		currentSprite = sprite;
		UpdateBoxes();
	}
	
	protected void OnClipChange(AnimationClip clip)
	{
		currentClip = clip.name;
		UpdateBoxes();
	}

	protected void UpdateBoxes()
	{ 
		hhBoxesController.UpdateBoxes(Tuple.Create(currentClip, currentSprite));
	}

	protected void OnDrawGizmosSelected()
	{
		if (!drawGizmo || hhBoxesController == null) return;

		HHBox hitbox = hhBoxesController.CurrentHitbox;
		HHBox[] hurtboxes = hhBoxesController.CurrentHurtboxes;

		Gizmos.color = new Color(0, 1, 0, 0.3f);
		Gizmos.DrawCube(transform.position + hitbox.offset, hitbox.size);

		if (hurtboxes != null)
		{
			Gizmos.color = new Color(1, 0, 0, 0.3f);
			foreach (HHBox hurtbox in hurtboxes)
			{
				Gizmos.DrawCube(transform.position + hurtbox.offset, hurtbox.size);
			}
		}
	}
}