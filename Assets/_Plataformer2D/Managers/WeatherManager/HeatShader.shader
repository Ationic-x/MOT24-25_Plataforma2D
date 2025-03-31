Shader "CustomEffects/HeatShader"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    float _Strength;
    float _Speed;
    float _CellDensity;
    float _AngleSpeed;
    float _AngleOffset;

    //Función Voronoi simulada
    float voronoi(float2 uv, float angleOffset, float cellDensity)
    {
        uv *= cellDensity;

        float2 gv = frac(uv) - 0.5;
        float2 id = floor(uv);

        float minDist = 1.0;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                float2 offset = float2(x, y);
                float2 neighbor = id + offset;
                float2 rand = frac(sin(dot(neighbor, float2(127.1, 311.7))) * 43758.5453);
                float2 diff = offset + rand - gv;
                float dist = dot(diff, diff);
                minDist = min(minDist, dist);
            }
        }

        return minDist;
    }

    float4 Dummy(Varyings input) : SV_Target
    {
        const float BLUR_SAMPLES = 64;
        const float BLUR_SAMPLES_RANGE = BLUR_SAMPLES / 2;
            
        float3 color = 0;
            
        for(float i = -BLUR_SAMPLES_RANGE; i <= BLUR_SAMPLES_RANGE; i++)
        {
            float2 sampleOffset = float2 (0, (0 / _BlitTexture_TexelSize.w) * (i / BLUR_SAMPLES_RANGE));
            color += SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, input.texcoord + sampleOffset).rgb;
        }
            
        return float4(color.rgb / (BLUR_SAMPLES + 1), 1);
    }

    float4 HeatEffect(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;

        // === Noise UV ===
        float2 noiseUV = uv;
        noiseUV.y -= _Time.y * _Speed;

        // === Angle animado ===
        float angle = sin(_Time.y * _AngleSpeed) + _AngleOffset;

        // Voronoi y desplazamiento
        float v = voronoi(noiseUV, angle, _CellDensity);
        float offset = v * _Strength;

        // Aplicamos desplazamiento en X
        uv.x += offset;

        float4 color = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, uv);
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
            Name "HeatPass"
            
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment Dummy
            
            ENDHLSL
        }
        Pass
        {
            Name "HeatPass2"
            
            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment HeatEffect
            
            ENDHLSL
        }
    }
}
