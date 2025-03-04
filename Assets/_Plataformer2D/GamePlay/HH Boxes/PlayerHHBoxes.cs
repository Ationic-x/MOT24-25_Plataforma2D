using System;
using UnityEngine;

public class PlayerHHBoxes : ActorHHBoxes
{
	protected override void Awake()
	{
		hhBoxesDataInstance = PlayerHHBoxesData.Instance;
		base.Awake();
	}
}
