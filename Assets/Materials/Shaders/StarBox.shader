Shader "Custom/SkyboxWithStarsLargeGapsAndColor"
{
    Properties
    {
        _RotationSpeed("Rotation Speed", Float) = 1
        _StarDensity("Star Density", Range(0.1, 1.0)) = 0.5
        _StarSize("Star Size", Range(0.1, 1.0)) = 0.5
        _Brightness("Brightness", Range(0.1, 2.0)) = 1.0
        _GapThreshold("Gap Threshold", Range(0.1, 1.0)) = 0.1
        _SkyColor("Sky Color", Color) = (1, 1, 1, 1)
    }

        SubShader
    {
        Tags { "RenderType" = "Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 tex : TEXCOORD0;
            };

            float _RotationSpeed;
            float _StarDensity;
            float _StarSize;
            float _Brightness;
            float _GapThreshold;
            fixed4 _SkyColor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Rotate vertex
                float angle = _Time.y * _RotationSpeed * 0.1;
                float s = sin(angle);
                float c = cos(angle);
                float3x3 rotationMatrix = float3x3(c, 0, s, 0, 1, 0, -s, 0, c);
                o.tex = mul(rotationMatrix, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 skyColor = _SkyColor.rgb;

                // Generate stars procedurally based on fragment position
                float2 starPosition = i.tex.xy * _StarDensity;
                float starValue = frac(sin(dot(starPosition, float2(12.9898,78.233))) * 43758.5453);
                float starIntensity = step(starValue, _StarSize);

                // Apply gap threshold to increase gap between stars
                if (starValue < _GapThreshold)
                {
                    starIntensity = 0.0;
                }

                // Add star brightness
                skyColor += starIntensity * _Brightness;

                return fixed4(skyColor, 1.0);
            }
            ENDCG
        }
    }

        FallBack "Skybox/Cubemap"
}
