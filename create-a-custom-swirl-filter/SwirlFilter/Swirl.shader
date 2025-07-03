Shader "Unlit/Swirl"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend One OneMinusSrcAlpha
        ZWrite Off
        ZTest Always 
        Cull Off

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

            Texture2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            SamplerState my_linear_repeat_sampler;

            float4 _UVRect; // UV rectangle for the texture (x, y, width, height)

            float _Angle;
            float _Radius;

            float2 NormalizeUVs(float2 uv)
            {
                // Normalize UV coordinates based on the atlas rect
                return float2(
                    (uv.x - _UVRect.x) / _UVRect.z,
                    (uv.y - _UVRect.y) / _UVRect.w
                );
            }

            float2 MapToUVRect(float2 uv)
            {
                // Map UV coordinates to the atlas rect
                return float2(
                    uv.x * _UVRect.z + _UVRect.x,
                    uv.y * _UVRect.w + _UVRect.y
                );
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Convert the atlas UVs to the 0-1 range
                float2 uv = NormalizeUVs(i.uv);

                float effectRadius = _Radius;
                float effectAngle = _Angle;

                float2 center = float2(0.5, 0.5);

                uv = uv - center;
                float len = length(uv * float2(_MainTex_TexelSize.z * _MainTex_TexelSize.y, 1.));
                float angle = atan2(uv.y, uv.x) + effectAngle * smoothstep(effectRadius, 0., len);
                float radius = length(uv);

                uv = radius * float2(cos(angle), sin(angle)) + center;

                // Map the UVs back to the atlas rect
                uv = MapToUVRect(uv);

                return _MainTex.Sample(my_linear_repeat_sampler, uv);
            }
            ENDCG
        }
    }
}
