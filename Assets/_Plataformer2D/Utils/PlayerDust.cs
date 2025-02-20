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
		particleSystemRenderer = _particleSystemRenderer;
		particlesMaterial = _particleSystemRenderer.material;
		mainPS = _particleSystem.main;
        shapePS = _particleSystem.shape;
		animationPS = _particleSystem.textureSheetAnimation;
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
					currentPD = item;
					UpdateParticleSystem();
                    return;
				}
			} else
			{
                Console.WriteLine($"The {id} wasn't found in the particle list.");
            }
		}
	}
}
