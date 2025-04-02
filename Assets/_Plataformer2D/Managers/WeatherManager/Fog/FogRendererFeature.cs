using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FogRendererFeature : ScriptableRendererFeature
{
	private FogSettings settings;
	[SerializeField] private Shader shader;
	private Material material;
	private FogRenderPass fogRenderPass;

	public override void Create()
	{
		if (shader == null)
		{
			return;
		}
		material = new Material(shader);
		fogRenderPass = new FogRenderPass(material, settings);

		fogRenderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

	public override void AddRenderPasses(ScriptableRenderer renderer,
		ref RenderingData renderingData)
	{
        if (fogRenderPass == null)
        {
            return;
        }
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(fogRenderPass);
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
public class FogSettings
{
	public Texture noiseTex;
	public Color fogColor = new Color(0.8f, 0.8f, 0.8f, 1f);
	public float fogOpacity = 0.3f;
	public float speedX = 0.05f;
	public float speedY = 0.01f;
	public float scale = 2f;
	public float yCoverage = 0.5f;
	public float smoothnessMin = 0.4f;
	public float smoothnessMax = 0.7f;
	public float remapMin = 0.3f;
	public float remapMax = 0.8f;
}