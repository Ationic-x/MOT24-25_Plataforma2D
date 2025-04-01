using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class HeatRenderPass : ScriptableRenderPass
{
	private static readonly int strengthID = Shader.PropertyToID("_Strength");
	private static readonly int speedID = Shader.PropertyToID("_Speed");
	private static readonly int yCoverageID = Shader.PropertyToID("_YCoverage");
	private static readonly int cellDensityID = Shader.PropertyToID("_CellDensity");
	private static readonly int angleSpeedID = Shader.PropertyToID("_AngleSpeed");
	private static readonly int angleOffsetID = Shader.PropertyToID("_AngleOffset");

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
		if (material == null) return;

		var volumeComponent = VolumeManager.instance.stack.GetComponent<HeatVolumeComponent>();

		float strength = OverrideState(defaultSettings.strength, volumeComponent.strength);
		float speed = OverrideState(defaultSettings.speed, volumeComponent.speed);
        float yCoverage = OverrideState(defaultSettings.yCoverage, volumeComponent.yCoverage);
        float cellDensity = OverrideState(defaultSettings.cellDensity, volumeComponent.cellDensity);
        float angleSpeed = OverrideState(defaultSettings.angleSpeed, volumeComponent.angleSpeed);
        float angleOffset = OverrideState(defaultSettings.angleOffset, volumeComponent.angleOffset);

        material.SetFloat(strengthID, strength);
		material.SetFloat(speedID, speed);
		material.SetFloat(yCoverageID, yCoverage);
		material.SetFloat(cellDensityID, cellDensity);
		material.SetFloat(angleSpeedID, angleSpeed);
		material.SetFloat(angleOffsetID, angleOffset);
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

        RenderGraphUtils.BlitMaterialParameters paraHeat = new(srcCamColor, dst, material, 0);
        renderGraph.AddBlitPass(paraHeat, k_BlitPassName);
        
        RenderGraphUtils.BlitMaterialParameters paraHorizontal = new(dst, srcCamColor, material, 1);
        renderGraph.AddBlitPass(paraHorizontal, k_HeatPassName);
    }
}