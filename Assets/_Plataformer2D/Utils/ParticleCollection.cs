using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	protected ParticleSystem.MainModule psMain;
	protected Material particlesMaterial;
	protected ParticleSystemRenderer particleSystemRenderer;


	public virtual void SelectParticleEffect<T>(T id) where T : Enum
	{
		foreach (ParticleData item in listPD)
		{
			if (Enum.IsDefined(typeof(T), item.id))
			{
				UpdateParticleSystem(item);
				return;
			}
		}

		Console.WriteLine($"The {id} wasn't found in the particle list.");
	}

	protected virtual void UpdateParticleSystem(ParticleData data)
	{
		particlesMaterial.SetFloat("_Duration", data.lifeTime);
		particlesMaterial.SetFloat("_StartTime", Time.time);
		particlesMaterial.SetFloat("_Frames", data.frames);
		particlesMaterial.SetTexture("_Texture2D", data.texture);

		particleSystemRenderer.flip = new Vector2(data.flipX ? 1 : 0, data.flipY ? 1 : 0);
		particleSystemRenderer.pivot = data.position;


		psMain.startLifetime = data.lifeTime;
		psMain.simulationSpace = data.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

		loopDelay = data.delayBetweenParticles;
	}

	public virtual void Play()
	{
		ps.Play();
	}
	
	public virtual void PlayInLoop()
	{
		StartCoroutine("Loop");
	}

	IEnumerator Loop()
	{
		ps.Play();
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
