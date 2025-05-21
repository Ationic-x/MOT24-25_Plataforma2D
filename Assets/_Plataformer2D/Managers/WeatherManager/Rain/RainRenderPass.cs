using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class RainRenderPass : ScriptableRenderPass
{
	private static readonly int dropTexID = Shader.PropertyToID("_DropTex");
    private static readonly int dropCountID = Shader.PropertyToID("_DropCount");
    private static readonly int dropScaleID = Shader.PropertyToID("_DropScale");
	private static readonly int fallSpeedID = Shader.PropertyToID("_FallSpeed");
	private static readonly int opacityID = Shader.PropertyToID("_Opacity");
	private static readonly int colorID = Shader.PropertyToID("_Color");
	//private static readonly int scaleID = Shader.PropertyToID("_Scale");
	//private static readonly int rotationSpeedID = Shader.PropertyToID("_DistortionAmount");

    private const string k_RainTextureName = "_RainTexture";
    private const string k_RainPassName = "RainRenderPass";
    private const string k_BlitPassName = "BlitRenderPass";

    private RainSettings defaultSettings;
	private Material material;

	private RenderTextureDescriptor heatTextureDescriptor;

    public RainRenderPass(Material material, RainSettings defaultSettings)
	{
		this.material = material;
		this.defaultSettings = defaultSettings;

		heatTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height,
			RenderTextureFormat.Default, 0);
	}

    private void UpdateRainSettings()
	{
		RainVolumeComponent volumeComponent = VolumeManager.instance.stack.GetComponent<RainVolumeComponent>();

		if (material == null || defaultSettings == null || volumeComponent == null) return;

		Texture dropTex = OverrideState(defaultSettings.dropTex, volumeComponent.dropTex);
		float dropCount = OverrideState(defaultSettings.dropCount, volumeComponent.dropCount);
		float dropScale = OverrideState(defaultSettings.dropScale, volumeComponent.dropScale);
		float fallSpeed = OverrideState(defaultSettings.fallSpeed, volumeComponent.fallSpeed);
		float opacity = OverrideState(defaultSettings.opacity, volumeComponent.Opacity);
		Color color = OverrideState(defaultSettings.color, volumeComponent.color);
		//float scale = OverrideState(defaultSettings.scale, volumeComponent.scale);
		//float distortionAmount = OverrideState(defaultSettings.distortionAmount, volumeComponent.distortionAmount);

		material.SetTexture(dropTexID, dropTex);
		material.SetFloat(dropCountID, dropCount);
		material.SetFloat(dropScaleID, dropScale);
		material.SetFloat(fallSpeedID, fallSpeed);
		material.SetFloat(opacityID, opacity);
		material.SetColor(colorID, color);
		//material.SetFloat(scaleID, scale);
		//material.SetFloat(rotationSpeedID, distortionAmount);
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
            heatTextureDescriptor, k_RainTextureName, false);

        UpdateRainSettings();

        if (!srcCamColor.IsValid() || !dst.IsValid())
            return;

        RenderGraphUtils.BlitMaterialParameters paraBlit = new(srcCamColor, dst, material, 0);
        renderGraph.AddBlitPass(paraBlit, k_BlitPassName);
        
        RenderGraphUtils.BlitMaterialParameters paraHeat = new(dst, srcCamColor, material, 1);
        renderGraph.AddBlitPass(paraHeat, k_RainPassName);
    }
}