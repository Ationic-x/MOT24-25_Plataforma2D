Shader "Weather/HeatShader"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    
    sampler2D _NoiseTex; // Texture heat

    // General parameters
    float _Strength;
    float _YCoverage;

    // Scale and speed
    float _Scale;
    float _SpeedX;
    float _SpeedY;
    
    // Rotation
    float _RotationSpeed;
    float _RotationOffset;

    float4 RotateUV(float2 uv, float angle, float2 pivot)
    {
        float s = sin(angle);
        float c = cos(angle);
        uv -= pivot;
        float2 rotated = float2(uv.x * c - uv.y * s, uv.x * s + uv.y * c);
        return float4(rotated + pivot, 0, 0);
    }

    float4 HeatEffect(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;

        float mask = smoothstep(_YCoverage, 0.0, uv.y);

        float2 noiseUV = uv * _Scale;
        noiseUV += float2(_Time.y * _SpeedX, _Time.y * _SpeedY);

        float rotation = _Time.y * _RotationSpeed + _RotationOffset;
        float2 rotatedUV = RotateUV(noiseUV, rotation, float2(0.5, 0.5)).xy;

        float noise = tex2D(_NoiseTex, rotatedUV).r;

        float offset = noise * _Strength * mask;
        uv.x += offset;

        float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_PointClamp, uv);
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
                return SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord);
            }
            
            ENDHLSL
        }
        Pass
        {
            Name "HeatPass"
            
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment HeatEffect
            
            ENDHLSL
        }
    }
}
