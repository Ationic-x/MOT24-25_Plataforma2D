using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static DustCollection;

public abstract class ParticleCollection : MonoBehaviour
{
	public enum CollectionID
	{
		PlayerDust = 0,
	}

	protected CollectionID id;
	protected float loopDelay = 1f;
	protected ParticleSystem ps;
	protected List<ParticleData> listPD;
    protected ParticleData currentPD;
    protected ParticleSystem.MainModule mainPS;
	protected ParticleSystem.ShapeModule shapePS;
	protected ParticleSystem.TextureSheetAnimationModule animationPS;
	protected Material particlesMaterial;
	protected ParticleSystemRenderer particleSystemRenderer;


	public abstract void SetParticleEffect<T>(T id) where T : Enum;

	protected virtual void UpdateParticleSystem()
	{
        particlesMaterial.SetTexture("_BaseMap", currentPD.texture);

		particleSystemRenderer.flip = new Vector2(currentPD.flipX ? 1 : 0, currentPD.flipY ? 1 : 0);

		animationPS.numTilesX = currentPD.frames;
		animationPS.numTilesY = currentPD.row;

		shapePS.position = currentPD.offset;

		mainPS.duration = currentPD.lifeTime;
		mainPS.startLifetime = currentPD.lifeTime;
		mainPS.simulationSpace = currentPD.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

		loopDelay = currentPD.delayBetweenParticles;
	}


	#region Flips
	public void FlipXPosition()
    {
        shapePS.position = new Vector2(-shapePS.position.x, shapePS.position.y);
    }

    public void FlipXTexture()
    {
        particleSystemRenderer.flip = new Vector2((int)particleSystemRenderer.flip.x ^ 1 , particleSystemRenderer.flip.y);
    }

    public void HorizontalFlip()
    {
		FlipXPosition();
		FlipXTexture();
    }
	#endregion


	public virtual void Play()
	{
		ps.Play();
	}
	
	public virtual void PlayInLoop()
	{
		StartCoroutine(Loop());
	}

	IEnumerator Loop()
	{
		Play();
		yield return new WaitForSeconds(1f);
	}

	public virtual void Emit(Vector3 position)
	{
		ParticleSystem.EmitParams parameters = new()
		{
			position = position - shapePS.position,
		};
		ps.Emit(parameters, 1);
	}

	public void Stop()
	{
		ps.Stop();
	}
	
	public void Pause()
	{
		ps.Pause();
	}

	public virtual CollectionID ID { get => id; }
}
