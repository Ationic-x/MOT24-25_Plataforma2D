using System;
using UnityEngine.Rendering;

[Serializable]
public sealed class HeatVolumeComponent : VolumeComponent
{
	public ClampedFloatParameter strength =
		new ClampedFloatParameter(0.05f, 0, 1f);
	public ClampedFloatParameter speed =
		new ClampedFloatParameter(0.05f, 0, 100f);
	public ClampedFloatParameter yCoverage =
		new ClampedFloatParameter(0f, 0, 10f);
	public ClampedFloatParameter cellDensity =
		new ClampedFloatParameter(0.05f, 0, 100f);
	public ClampedFloatParameter angleSpeed =
		new ClampedFloatParameter(0.05f, 0, 100f);
	public ClampedFloatParameter angleOffset =
		new ClampedFloatParameter(0.05f, 0, 100f);
}
