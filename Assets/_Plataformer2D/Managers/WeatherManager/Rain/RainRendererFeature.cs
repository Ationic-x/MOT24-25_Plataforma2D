using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;

public class RainRendererFeature : ScriptableRendererFeature
{
	[SerializeField] private Shader shader;
	private RainSettings settings;
	private Material material;
	private RainRenderPass rainRenderPass;


	public override void Create()
	{
		if (shader == null)
		{
			return;
		}
		material = new Material(shader);
		rainRenderPass = new RainRenderPass(material, settings);

		rainRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

	public override void AddRenderPasses(ScriptableRenderer renderer,
		ref RenderingData renderingData)
	{
        if (rainRenderPass == null)
        {
            return;
        }
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(rainRenderPass);
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
public class RainSettings
{
	public Texture dropTex;
	public float dropCount = 0.01f;
	public float dropScale = 0.5f;
	public float fallSpeed = 0.1f;
	public float opacity = 0.1f;
	public Color color = Color.white;
	//public float scale = 1f;
	//public float distortionAmount = 2f;
}