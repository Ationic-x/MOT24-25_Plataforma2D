using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public sealed class HeatVolumeComponent : VolumeComponent
{
	[Header("Heat Settings")]
	public Texture2DParameter noiseTex =
		new Texture2DParameter(null);
	
	public ClampedFloatParameter strength = 
		new ClampedFloatParameter(0.05f, 0, 1f);

	public ClampedFloatParameter yCoverage = 
		new ClampedFloatParameter(0f, 0, 10f);
	
	public ClampedFloatParameter scale = 
		new ClampedFloatParameter(0f, 0, 10f);
	

	[Header("Noise Speed")]
	public ClampedFloatParameter speedX = 
		new ClampedFloatParameter(0.05f, 0, 2f);
	
	public ClampedFloatParameter speedY = 
		new ClampedFloatParameter(0.05f, 0, 2f);

	[Header("Noise Rotation")]
	public ClampedFloatParameter rotationSpeed =
		new ClampedFloatParameter(0.05f, 0, 2f);

	public ClampedFloatParameter rotationOffset = 
		new ClampedFloatParameter(0.05f, 0, 100f);
}
