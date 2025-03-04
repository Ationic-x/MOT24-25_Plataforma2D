using System;
using UnityEngine;

public class LightBanditHHBoxes : ActorHHBoxes
{
	protected override void Awake()
	{
		hhBoxesDataInstance = LightBanditHHBoxesData.Instance;
		base.Awake();
	}
}
