Shader "Custom/VaporwaveGrid"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _GridColor("Grid Color", Color) = (1,1,1,1)
        _GridThickness("Grid Thickness", Range(0.001, 0.1)) = 0.01
        _GridSpacing("Grid Spacing", Range(0.1, 10.0)) = 1.0
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _GridColor;
                float _GridThickness;
                float _GridSpacing;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 gridUV = i.uv / _GridSpacing;
                    float2 gridLines = fmod(abs(frac(gridUV) - 0.5), _GridThickness);
                    float gridMask = 1.0 - smoothstep(0.0, _GridThickness, min(gridLines.x, gridLines.y));
                    fixed4 col = tex2D(_MainTex, i.uv) * _GridColor * gridMask;
                    return col;
                }
                ENDCG
            }
        }
}
