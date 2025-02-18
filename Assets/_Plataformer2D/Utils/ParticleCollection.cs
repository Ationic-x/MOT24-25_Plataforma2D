using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerDust;

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
	protected Material particlesMaterial;
	protected ParticleSystemRenderer particleSystemRenderer;


	public abstract void SetParticleEffect<T>(T id) where T : Enum;

	protected virtual void UpdateParticleSystem()
	{
        particlesMaterial.SetFloat("_Duration", currentPD.lifeTime);
		particlesMaterial.SetFloat("_StartTime", Time.time);
		particlesMaterial.SetFloat("_Frames", currentPD.frames);
		particlesMaterial.SetTexture("_Texture2D", currentPD.texture);

		particleSystemRenderer.flip = new Vector2(currentPD.flipX ? 1 : 0, currentPD.flipY ? 1 : 0);

		shapePS.position = currentPD.position;


		mainPS.startLifetime = currentPD.lifeTime;
		mainPS.simulationSpace = currentPD.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

		loopDelay = currentPD.delayBetweenParticles;
	}


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
