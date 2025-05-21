using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class HeatRenderPass : ScriptableRenderPass
{
	private static readonly int noiseTexID = Shader.PropertyToID("_NoiseTex");
	private static readonly int strengthID = Shader.PropertyToID("_Strength");
	private static readonly int yCoverageID = Shader.PropertyToID("_YCoverage");
	private static readonly int speedXID = Shader.PropertyToID("_SpeedX");
	private static readonly int speedYID = Shader.PropertyToID("_SpeedY");
	private static readonly int scaleID = Shader.PropertyToID("_Scale");
	private static readonly int rotationSpeedID = Shader.PropertyToID("_RotationSpeed");
	private static readonly int rotationOffsetID = Shader.PropertyToID("_RotationOffset");

    private const string k_HeatTextureName = "_HeatTexture";
    private const string k_HeatPassName = "HeatRenderPass";
    private const string k_BlitPassName = "BlitRenderPass";

    private HeatSettings defaultSettings;
	private Material material;

	private RenderTextureDescriptor heatTextureDescriptor;

    public HeatRenderPass(Material material, HeatSettings defaultSettings)
	{
		this.material = material;
		this.defaultSettings = defaultSettings;

		heatTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height,
			RenderTextureFormat.Default, 0);
	}

    private void UpdateHeatSettings()
	{
		HeatVolumeComponent volumeComponent = VolumeManager.instance.stack.GetComponent<HeatVolumeComponent>();

		if (material == null || defaultSettings == null || volumeComponent == null) return;

		Texture noiseTex = OverrideState(defaultSettings.noiseTex, volumeComponent.noiseTex);
		float strength = OverrideState(defaultSettings.strength, volumeComponent.strength);
        float yCoverage = OverrideState(defaultSettings.yCoverage, volumeComponent.yCoverage);
		float speedX = OverrideState(defaultSettings.speedX, volumeComponent.speedX);
		float speedY = OverrideState(defaultSettings.speedX, volumeComponent.speedY);
        float scale = OverrideState(defaultSettings.scale, volumeComponent.scale);
        float rotationSpeed = OverrideState(defaultSettings.rotationSpeed, volumeComponent.rotationSpeed);
        float rotationOffset = OverrideState(defaultSettings.rotationOffset, volumeComponent.rotationOffset);

        material.SetTexture(noiseTexID, noiseTex);
		material.SetFloat(strengthID, strength);
        material.SetFloat(yCoverageID, yCoverage);
        material.SetFloat(speedXID, speedX);
        material.SetFloat(speedYID, speedY);
        material.SetFloat(scaleID, scale);
        material.SetFloat(rotationSpeedID, rotationSpeed);
        material.SetFloat(rotationOffsetID, rotationOffset);
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

        heatTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
        heatTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;
        heatTextureDescriptor.depthBufferBits = 0;

        TextureHandle srcCamColor = resourceData.activeColorTexture;
        TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph,
            heatTextureDescriptor, k_HeatTextureName, false);

        UpdateHeatSettings();

        if (!srcCamColor.IsValid() || !dst.IsValid())
            return;

        RenderGraphUtils.BlitMaterialParameters paraBlit = new(srcCamColor, dst, material, 0);
        renderGraph.AddBlitPass(paraBlit, k_BlitPassName);
        
        RenderGraphUtils.BlitMaterialParameters paraHeat = new(dst, srcCamColor, material, 1);
        renderGraph.AddBlitPass(paraHeat, k_HeatPassName);
    }
}