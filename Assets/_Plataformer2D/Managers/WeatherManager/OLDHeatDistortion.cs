using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[System.Serializable]
[PostProcess(typeof(HeatDistortionRenderer), PostProcessEvent.AfterStack, "Custom/HeatDistortion")]
public sealed class HeatDistortion : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Strength of the heat distortion effect.")]
    public FloatParameter distortionStrength = new FloatParameter { value = 0.1f };

    [Range(0f, 1f), Tooltip("Frequency of the heat distortion.")]
    public FloatParameter distortionFrequency = new FloatParameter { value = 0.5f };
}

public sealed class HeatDistortionRenderer : PostProcessEffectRenderer<HeatDistortion>
{
    private Material material;

    public override void Init()
    {
        Shader heatDistortionShader = Shader.Find("Hidden/Custom/HeatDistortionShader");
        material = new Material(heatDistortionShader);
    }

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(material.shader);
        sheet.properties.SetFloat("_DistortionStrength", settings.distortionStrength);
        sheet.properties.SetFloat("_DistortionFrequency", settings.distortionFrequency);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
