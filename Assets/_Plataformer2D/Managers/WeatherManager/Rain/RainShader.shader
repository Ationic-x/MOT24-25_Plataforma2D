Shader "Weather/RainShader"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    TEXTURE2D(_DropTex);
    SAMPLER(sampler_DropTex);

    float _DropCount;
    float _DropScale;
    float _FallSpeed;
    float4 _Color;
    float _Opacity;

    float2 Hash2(uint i)
    {
        i = (i ^ 61u) ^ (i >> 16);
        i *= 9u;
        i = i ^ (i >> 4);
        i *= 0x27d4eb2du;
        i = i ^ (i >> 15);
    
        float fx = frac(i * 0.0000001);
        float fy = frac(i * 0.0000017);
        return float2(fx, fy);
    }

    float4 RainEffect(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float4 baseColor = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);
        float4 rainColor = float4(0, 0, 0, 0);

        for (int i = 0; i < (int)_DropCount; i++)
        {
            float2 rand = Hash2(i);
            float lifetime = 1.3;
            float time = _Time.x * _FallSpeed;

            float dropTime = time + rand.y * lifetime;

            float currentCycle = floor(dropTime / lifetime);
            float cycleT = frac(dropTime / lifetime);

            float margin = _DropScale * 0.5;
            float hashX1 = frac(sin((i + currentCycle) * 17.13) * 43758.5453) - margin;
            float hashX2 = frac(sin((i + currentCycle + 1.0) * 17.13) * 43758.5453) - margin;

            float x = lerp(hashX1, hashX2, smoothstep(1.0, 1.0, cycleT));

            float y = 1.1 - cycleT * lifetime;
            float2 pos = float2(x, y);

            float2 localUV = float2((uv.x - pos.x) / _DropScale, (uv.y - pos.y) / _DropScale);

            if (all(localUV >= 0.0) && all(localUV <= 1.0))
            {
                float4 drop = SAMPLE_TEXTURE2D(_DropTex, sampler_LinearClamp, localUV);
                float3 tint = drop.rgb * _Color.rgb;
                float a = drop.a * _Color.a;

                if (a > rainColor.a)
                {
                    rainColor.rgb = tint;
                    rainColor.a = a;
                }
            }
        }

        rainColor.a = saturate(rainColor.a);
        float4 final = lerp(baseColor, float4(rainColor.rgb, baseColor.a), rainColor.a);
        return final;
    }

    ENDHLSL

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
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
            Name "RainPass"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment RainEffect
            ENDHLSL
        }
    }
}