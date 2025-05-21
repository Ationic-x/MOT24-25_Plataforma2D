using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class FogRenderPass : ScriptableRenderPass
{
	private static readonly int fogNoiseTexID = Shader.PropertyToID("_NoiseTex");
	private static readonly int fogColorID = Shader.PropertyToID("_FogColor");
	private static readonly int fogOpacityID = Shader.PropertyToID("_FogOpacity");
	private static readonly int speedXID = Shader.PropertyToID("_SpeedX");
	private static readonly int speedYID = Shader.PropertyToID("_SpeedY");
	private static readonly int scaleID = Shader.PropertyToID("_Scale");
	private static readonly int fogCoverageID = Shader.PropertyToID("_YCoverage");
	private static readonly int fogThresholdMinID = Shader.PropertyToID("_SmoothnessMin");
	private static readonly int fogThresholdMaxID = Shader.PropertyToID("_SmoothnessMax");
	private static readonly int remapMinID = Shader.PropertyToID("_RemapMin");
	private static readonly int remapMaxID = Shader.PropertyToID("_RemapMax");

	private const string k_FogTextureName = "_FogTexture";
    private const string k_FogPassName = "FogRenderPass";
    private const string k_BlitPassName = "BlitRenderPass";

    private FogSettings defaultSettings;
	private Material material;

	private RenderTextureDescriptor fogTextureDescriptor;

    public FogRenderPass(Material material, FogSettings defaultSettings)
	{
		this.material = material;
		this.defaultSettings = defaultSettings;

		fogTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height,
			RenderTextureFormat.Default, 0);
	}

    private void UpdateFogSettings()
	{
		FogVolumeComponent volumeComponent = VolumeManager.instance.stack.GetComponent<FogVolumeComponent>();

		if (material == null || defaultSettings == null || volumeComponent == null) return;

		float fogOpacity = OverrideState(defaultSettings.fogOpacity, volumeComponent.fogOpacity);
		float speedX = OverrideState(defaultSettings.speedX, volumeComponent.speedX);
		float speedY = OverrideState(defaultSettings.speedY, volumeComponent.speedY);
		float scale = OverrideState(defaultSettings.scale, volumeComponent.scale);
		float yCoverage = OverrideState(defaultSettings.yCoverage, volumeComponent.yCoverage);
		float smoothnessMin = OverrideState(defaultSettings.smoothnessMin, volumeComponent.smoothnessMin);
		float smoothnessMax = OverrideState(defaultSettings.smoothnessMax, volumeComponent.smoothnessMax);
		float remapMin = OverrideState(defaultSettings.remapMin, volumeComponent.remapMin);
		float remapMax = OverrideState(defaultSettings.remapMax, volumeComponent.remapMax);
		Color fogColor = OverrideState(defaultSettings.fogColor, volumeComponent.fogColor);
		Texture noiseText = OverrideState(defaultSettings.noiseTex, volumeComponent.noiseTex);

		material.SetFloat(fogOpacityID, fogOpacity);
		material.SetFloat(speedXID, speedX);
		material.SetFloat(speedYID, speedY);
		material.SetFloat(scaleID, scale);
		material.SetFloat(fogCoverageID, yCoverage);
		material.SetFloat(fogThresholdMinID, smoothnessMin);
		material.SetFloat(fogThresholdMaxID, smoothnessMax);
		material.SetFloat(remapMinID, remapMin);
		material.SetFloat(remapMaxID, remapMax);
		material.SetColor(fogColorID, fogColor);
		material.SetTexture(fogNoiseTexID, noiseText);
	}

	private T OverrideState<T, Y>(T defaultValue, Y volumeParameters) where Y : VolumeParameter<T>
	{
		if (volumeParameters.overrideState) 
			return volumeParameters.value;
		
		return defaultValue;
	}

    public override void RecordRenderGraph(RenderGraph renderGraph,
    ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

        if (resourceData.isActiveTargetBackBuffer)
            return;

        fogTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
        fogTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;
        fogTextureDescriptor.depthBufferBits = 0;

        TextureHandle srcCamColor = resourceData.activeColorTexture;
        TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph,
            fogTextureDescriptor, k_FogTextureName, false);

        UpdateFogSettings();

        if (!srcCamColor.IsValid() || !dst.IsValid())
            return;

        RenderGraphUtils.BlitMaterialParameters paraBlit = new(srcCamColor, dst, material, 0);
        renderGraph.AddBlitPass(paraBlit, k_BlitPassName);
        
        RenderGraphUtils.BlitMaterialParameters paraFog = new(dst, srcCamColor, material, 1);
        renderGraph.AddBlitPass(paraFog, k_FogPassName);
    }
}