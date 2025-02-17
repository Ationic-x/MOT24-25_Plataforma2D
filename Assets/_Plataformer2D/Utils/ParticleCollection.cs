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


	public virtual void SetParticleEffect<T>(T id) where T : Enum
	{

        T tmpID = (T)Enum.ToObject(typeof(T), currentPD.id);
        if (tmpID.Equals(id)) return;
        foreach (ParticleData item in listPD)
        {
            if (Enum.IsDefined(typeof(T), item.id))
            {
                if (tmpID.Equals(item.id))
                {
                    currentPD = item;
                    UpdateParticleSystem();
                    return;
                }
            }
            else
            {
                Console.WriteLine($"The {id} wasn't found in the particle list.");
            }
        }
	}

	protected virtual void UpdateParticleSystem()
	{
		particlesMaterial.SetFloat("_Duration", currentPD.lifeTime);
		particlesMaterial.SetFloat("_StartTime", Time.time);
		particlesMaterial.SetFloat("_Frames", currentPD.frames);
		particlesMaterial.SetTexture("_Texture2D", currentPD.texture);

		particleSystemRenderer.flip = new Vector2(currentPD.flipX ? 1 : 0, currentPD.flipY ? 1 : 0);
		particleSystemRenderer.pivot = currentPD.position;


		mainPS.startLifetime = currentPD.lifeTime;
		mainPS.simulationSpace = currentPD.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

		loopDelay = currentPD.delayBetweenParticles;
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
