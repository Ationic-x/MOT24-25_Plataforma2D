using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
using UnityEngine.Rendering.Universal;

public class HeatRenderPass : ScriptableRenderPass
{
	private static readonly int strengthID = Shader.PropertyToID("_Strength");
	private static readonly int speedID = Shader.PropertyToID("_Speed");
	private static readonly int cellDensityID = Shader.PropertyToID("_CellDensity");
	private static readonly int angleSpeedID = Shader.PropertyToID("_AngleSpeed");
	private static readonly int angleOffsetID = Shader.PropertyToID("_AngleOffset");

	private const string k_BlurTextureName = "_BlurTexture";
	private const string k_HeatPassName = "HeatPass";

	private HeatSettings defaultSettings;
	private Material material;

	private RenderTextureDescriptor blurTextureDescriptor;

	public HeatRenderPass(Material material, HeatSettings defaultSettings)
	{
		this.material = material;
		this.defaultSettings = defaultSettings;

		blurTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height,
			RenderTextureFormat.Default, 0);
	}

	private void UpdateHeatSettings()
	{
		if (material == null) return;

		// Use the Volume settings or the default settings if no Volume is set.
		var volumeComponent =
			VolumeManager.instance.stack.GetComponent<HeatVolumeComponent>();
		float strength = volumeComponent.angleSpeed.overrideState ?
			volumeComponent.strength.value : defaultSettings.strength;
		float speed = volumeComponent.speed.overrideState ?
			volumeComponent.speed.value : defaultSettings.speed;
		float cellDensity = volumeComponent.cellDensity.overrideState ?
			volumeComponent.cellDensity.value : defaultSettings.cellDensity;
		float angleSpeed = volumeComponent.angleSpeed.overrideState ?
			volumeComponent.angleSpeed.value : defaultSettings.angleSpeed;
		float angleOffset = volumeComponent.angleOffset.overrideState ?
			volumeComponent.angleOffset.value : defaultSettings.angleOffset;
		material.SetFloat(strengthID, strength);
		material.SetFloat(speedID, speed);
		material.SetFloat(cellDensityID, cellDensity);
		material.SetFloat(angleSpeedID, angleSpeed);
		material.SetFloat(angleOffsetID, angleOffset);
	}

	public override void RecordRenderGraph(RenderGraph renderGraph,
	ContextContainer frameData)
	{
		UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();

		UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

		// The following line ensures that the render pass doesn't blit
		// from the back buffer.
		if (resourceData.isActiveTargetBackBuffer)
			return;

		// Set the blur texture size to be the same as the camera target size.
		blurTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
		blurTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;
		blurTextureDescriptor.depthBufferBits = 0;

		TextureHandle srcCamColor = resourceData.activeColorTexture;
		TextureHandle dst = UniversalRenderer.CreateRenderGraphTexture(renderGraph,
			blurTextureDescriptor, k_BlurTextureName, false);

		// Update the blur settings in the material
		UpdateHeatSettings();

		// This check is to avoid an error from the material preview in the scene
		if (!srcCamColor.IsValid() || !dst.IsValid())
			return;

		// The AddBlitPass method adds a vertical blur render graph pass that blits from the source texture (camera color in this case) to the destination texture using the first shader pass (the shader pass is defined in the last parameter).
		RenderGraphUtils.BlitMaterialParameters paraVertical = new(srcCamColor, dst, material, 0);
		renderGraph.AddBlitPass(paraVertical, k_HeatPassName);
	}
}