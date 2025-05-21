using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
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
	public float strength = 0.01f;
	public float yCoverage = 0.5f;
	public float speedX = 0.1f;
	public float speedY = 0.1f;
	public float scale = 1f;
	public float rotationSpeed = 2f;
	public float rotationOffset = 2f;
}