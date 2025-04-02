Shader "Weather/FogShader"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    sampler2D _NoiseTex; // Texture with the noise (uses as fog)

    float4 _FogColor; // The color of the fog
    float _FogOpacity; // The opacity of the fog
    float _SpeedX; // The movement speed in the X axis
    float _SpeedY; // The movement speed in the Y axis
    float _Scale; // The scale of the fog
    float _YCoverage; // How much of the screen is affected
    float _SmoothnessMin; // The minimum smoothness of the fog (used in smothstep to override black to white steps)
    float _SmoothnessMax; // The maximum smoothness of the fog (used in smothstep to override black to white steps)
    float _RemapMin; // The minimum value to remap the noise
    float _RemapMax; // The maximum value to remap the noise

    float remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return saturate((value - inMin) / (inMax - inMin)) * (outMax - outMin) + outMin;
    }

    float4 FogEffect(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        float2 uv = input.texcoord;

        float2 noiseUV = uv * _Scale;
        noiseUV += float2(_Time.y * _SpeedX, _Time.y * _SpeedY);

        float noise = tex2D(_NoiseTex, noiseUV).r;

        noise = remap(noise, _RemapMin, _RemapMax, 0.0, 1.0);
        float fogAlpha = smoothstep(_SmoothnessMin, _SmoothnessMax, noise);

        float mask = smoothstep(_YCoverage, 0.0, uv.y);
        fogAlpha *= mask * _FogOpacity;

        float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_PointClamp, uv);
        color.rgb = lerp(color.rgb, _FogColor.rgb, fogAlpha);
        return color;
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZWrite Off Cull Off

        Pass
        {
            Name "BlitPass"
            
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment CopyPass

            float4 CopyPass(Varyings input) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_BlitTexture, sampler_PointRepeat, input.texcoord);
            }
            
            ENDHLSL
        }
        Pass
        {
            Name "FogPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment FogEffect
            ENDHLSL
        }
    }

    FallBack Off
}