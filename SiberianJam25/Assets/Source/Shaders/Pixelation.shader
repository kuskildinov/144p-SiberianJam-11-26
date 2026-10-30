
Shader "Custom/Pixelation"
{
    Properties
    {
        _PixelSize ("Pixel Size", Float) = 4
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "Pixelation"
            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            float _PixelSize;

            half4 Frag(Varyings input) : SV_Target
            {
                float2 resolution = _ScreenParams.xy;
                float2 pixelCount = max(
                    floor(resolution / max(_PixelSize, 1)),
                    float2(1, 1)
                );

                float2 uv = (floor(input.texcoord * pixelCount) + 0.5)
                            / pixelCount;

                return SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv
                );
            }
            ENDHLSL
        }
    }
}