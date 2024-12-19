using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDust : ParticleCollection
{
	public enum DustID
	{
		WallDust = 0,
	}


	[SerializeField] private List<ParticleData> _particleDataList;
	[SerializeField] private ParticleSystem _particleSystem;
	[SerializeField] private ParticleSystemRenderer _particleSystemRenderer;
	public bool debug;

	private void Awake()
	{
		ps = _particleSystem;
		listPD = _particleDataList;
		psMain = _particleSystem.main;
		particleSystemRenderer = _particleSystemRenderer;
		particlesMaterial = _particleSystemRenderer.material;
		id = CollectionID.PlayerDust;
	}

	private void Update()
	{
		if (debug)
		{
			debug = false;
			SelectParticleEffect(DustID.WallDust);
		}
	}

	public override void SelectParticleEffect<DustID>(DustID id)
	{
		foreach (ParticleData item in _particleDataList)
		{
			if (Enum.IsDefined(typeof(DustID), item.id))
			{
				UpdateParticleSystem(item);
				return;
			}
		}
		
		Console.WriteLine($"The {id} wasn't found in the particle list.");
		
	}

	//protected override void UpdateParticleSystem(ParticleData data)
	//{
	//	particlesMaterial.SetFloat("_Duration", data.lifeTime);
	//	particlesMaterial.SetFloat("_StartTime", Time.time); 
	//	particlesMaterial.SetFloat("_Frames", data.frames);
	//	particlesMaterial.SetTexture("_Texture2D", data.texture);

	//	particleSystemRenderer.flip = new Vector2(data.flipX ? 1 : 0, data.flipY ? 1: 0);
	//	particleSystemRenderer.pivot = data.position;


	//	psMain.startLifetime = data.lifeTime;
	//	psMain.simulationSpace = data.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

	//	loopDelay = data.delayBetweenParticles;

	//	StartCoroutine("TestLoop");
	//}

	public override void Play()
	{
		particlesMaterial.SetFloat("_StartTime", Time.time);
		ps.Play();
	}

	public override void PlayInLoop()
	{
		StartCoroutine("Loop");
	}

	IEnumerator Loop()
	{
		particlesMaterial.SetFloat("_StartTime", Time.time);
		ps.Play();
		yield return new WaitForSeconds(loopDelay);
	}
}
