using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeatRendererFeature : ScriptableRendererFeature
{
	[SerializeField] private HeatSettings settings;
	[SerializeField] private Shader shader;
	private Material material;
	private HeatRenderPass heatRenderPass;

	public override void Create()
	{
		if (shader == null)
		{
			return;
		}
		material = new Material(shader);
		heatRenderPass = new HeatRenderPass(material, settings);

		heatRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

	public override void AddRenderPasses(ScriptableRenderer renderer,
		ref RenderingData renderingData)
	{
        if (heatRenderPass == null)
        {
            return;
        }
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(heatRenderPass);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (Application.isPlaying)
        {
            Destroy(material);
        }
        else
        {
            DestroyImmediate(material);
        }
    }
}

[Serializable]
public class HeatSettings
{
	[Range(0, 10f)] public float strength;
	[Range(0, 10f)] public float speed;
	[Range(0, 10f)] public float cellDensity;
	[Range(0, 10f)] public float angleSpeed;
	[Range(0, 10f)] public float angleOffset;
}