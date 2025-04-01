Shader "CustomEffects/HeatShader"
{
    HLSLINCLUDE

    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

    // general heat values
    float _Strength; // distortion intensity
    float _Speed; // speed of the effect (only works in vertical)
    float _YCoverage; // how much of the screen is affected
    
    // voronoi values
    float _CellDensity; // voronoi cell density
    float _AngleSpeed; // speed that rotates the noise
    float _AngleOffset; // angle offset of the noise

    inline float2 voronoi_noise_randomVector(float2 UV, float offset)
    {
        float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
        UV = frac(sin(mul(UV, m)) * 46839.32);
        return float2(sin(UV.y * offset) * 0.5 + 0.5, cos(UV.x * offset) * 0.5 + 0.5);
    }

    void Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out)
    {
        float2 g = floor(UV * CellDensity);
        float2 f = frac(UV * CellDensity);
        float3 res = float3(8.0, 0.0, 0.0);

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                float2 lattice = float2(x, y);
                float2 offset = voronoi_noise_randomVector(lattice + g, AngleOffset);
                float d = distance(lattice + offset, f);
                if (d < res.x)
                {
                    res = float3(d, offset.x, offset.y);
                }
            }
        }

        Out = res.x;
    }

    float4 HeatEffect(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        float2 uv = input.texcoord;
        float mask = smoothstep(_YCoverage, 0.0, uv.y);
        float2 noiseUV = uv;
        noiseUV.y -= _Time.y * _Speed;

        float angle = sin(_Time.y * _AngleSpeed) + _AngleOffset;

        float voronoiValue;
        Voronoi_float(noiseUV, angle, _CellDensity, voronoiValue);

        float offset = voronoiValue * _Strength * mask;
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
