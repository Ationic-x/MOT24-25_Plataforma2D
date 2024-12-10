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
	private ParticleSystem.MainModule _psMain;
	private Material _particlesMaterial;
	public bool debug;
	private float _delayBetweenParticles;

	private void Awake()
	{
		SetParticleSystem(_particleSystem);
		_psMain = _particleSystem.main;
		_particlesMaterial = _particleSystemRenderer.material;
	}

	private void Update()
	{
		if (debug)
		{
			debug = false;
			SelectParticleEffect(DustID.WallDust);
		}
	}

	public void SelectParticleEffect(DustID id)
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

	private void UpdateParticleSystem(ParticleData data)
	{
		_particlesMaterial.SetFloat("_Duration", data.lifeTime);
		_particlesMaterial.SetFloat("_StartTime", Time.time); 
		_particlesMaterial.SetFloat("_Frames", data.frames);
		_particlesMaterial.SetTexture("_Texture2D", data.texture);

		_particleSystemRenderer.flip = new Vector2(data.flipX ? 1 : 0, data.flipY ? 1: 0);
		_particleSystemRenderer.pivot = data.position;


		_psMain.startLifetime = data.lifeTime;
		_psMain.simulationSpace = data.isLocal ? ParticleSystemSimulationSpace.Local : ParticleSystemSimulationSpace.World;

		_delayBetweenParticles = data.delayBetweenParticles;

		StartCoroutine("TestLoop");
	}

	IEnumerator TestLoop()
	{
		_particlesMaterial.SetFloat("_StartTime", Time.time);
		_particleSystem.Play();
		yield return new WaitForSeconds(_delayBetweenParticles);
	}

	private void Stopu()
	{
		_particleSystem.Stop();
		Debug.Log("Stop");
	}
}
