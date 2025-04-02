using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeatRendererFeature : ScriptableRendererFeature
{
	[SerializeField] private Shader shader;
	private HeatSettings settings;
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
	public Texture noiseTex;
	public float strength;
	public float yCoverage;
	public float speedX;
	public float speedY;
	public float scale;
	public float rotationSpeed;
	public float rotationOffset;
}