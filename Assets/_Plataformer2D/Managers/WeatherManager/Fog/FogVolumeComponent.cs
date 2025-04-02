using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public sealed class FogVolumeComponent : VolumeComponent
{
	[Header("Fog Settings")]
	public Texture2DParameter noiseTex =
		new Texture2DParameter(null);

	public ClampedFloatParameter scale =
		new ClampedFloatParameter(2f, 0.01f, 5f);

	public ClampedFloatParameter yCoverage =
		new ClampedFloatParameter(0.5f, 0f, 10f);

	[Header("Noise Color")]
	public ColorParameter fogColor =
		new ColorParameter(new Color(0.8f, 0.8f, 0.8f, 1f), true);

	public ClampedFloatParameter fogOpacity =
		new ClampedFloatParameter(0.3f, 0f, 1f);
	

	[Header("Noise Speed")]
	public ClampedFloatParameter speedX =
		new ClampedFloatParameter(0.05f, -2f, 2f);

	public ClampedFloatParameter speedY =
		new ClampedFloatParameter(0.01f, -2f, 2f);

	[Header("Noise Mask")]
	public ClampedFloatParameter smoothnessMin =
		new ClampedFloatParameter(0.4f, 0f, 1f);

	public ClampedFloatParameter smoothnessMax =
		new ClampedFloatParameter(0.7f, 0f, 1f);

	public ClampedFloatParameter remapMin =
		new ClampedFloatParameter(0.3f, 0f, 1f);

	public ClampedFloatParameter remapMax =
		new ClampedFloatParameter(0.8f, 0f, 1f);
}
