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
	private ParticleData _currentPD;
	public bool debug;

	private void Awake()
	{
		ps = _particleSystem;
		listPD = _particleDataList;
		mainPS = _particleSystem.main;
		particleSystemRenderer = _particleSystemRenderer;
		particlesMaterial = _particleSystemRenderer.material;
        shapePS = _particleSystem.shape;
		id = CollectionID.PlayerDust;

		// We use the first one to avoid null pointers
		currentPD = _particleDataList[0];
        UpdateParticleSystem();
    }

	private void Update()
	{
		if (debug)
		{
			debug = false;
			SetParticleEffect(DustID.WallDust);
		}
	}

	public override void SetParticleEffect<DustID>(DustID id)
    {
        DustID tmpID = (DustID)Enum.ToObject(typeof(DustID), currentPD.id);
        if (tmpID.Equals(id)) return;
        foreach (ParticleData item in _particleDataList)
		{
			if (Enum.IsDefined(typeof(DustID), item.id))
			{
                if (id.Equals(item.id))
				{
					_currentPD = item;
					UpdateParticleSystem();
                    return;
				}
			} else
			{
                Console.WriteLine($"The {id} wasn't found in the particle list.");
            }
		}
	}

	public override void Play()
	{
		particlesMaterial.SetFloat("_StartTime", Time.deltaTime);
		ps.Play();
	}

	public override void PlayInLoop()
	{
		StartCoroutine(Loop());
	}

	IEnumerator Loop()
	{
		Play();
		yield return new WaitForSeconds(loopDelay);
	}
}
