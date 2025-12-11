Shader "Unlit/Swirl"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Blend One Zero
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _UIE_OUTPUT_LINEAR

            #include "UnityCG.cginc"
            #include "UnityUIEFilter.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                uint rectIndex : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            float _Angle;
            float _Radius;

            v2f vert (FilterVertexInput v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.rectIndex = GetFilterRectIndex(v);
                return o;
            }

            float2 NormalizeUVs(float2 uv, float4 uvRect)
            {
                // Normalize UV coordinates based on the atlas rect
                return float2(
                    (uv.x - uvRect.x) / uvRect.z,
                    (uv.y - uvRect.y) / uvRect.w
                );
            }

            float2 MapToUVRect(float2 uv, float4 uvRect)
            {
                // Map UV coordinates to the atlas rect
                return float2(
                    uv.x * uvRect.z + uvRect.x,
                    uv.y * uvRect.w + uvRect.y
                );
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float effectRadius = _Radius;
                float effectAngle = _Angle;

                float4 uvRect = GetFilterUVRect(i.rectIndex);

                float2 uv = NormalizeUVs(i.uv, uvRect);
                float2 center = float2(0.5, 0.5);
                uv = uv - center;

                float len = length(uv * float2(_MainTex_TexelSize.z * _MainTex_TexelSize.y, 1.));
                float angle = atan2(uv.y, uv.x) + effectAngle * smoothstep(effectRadius, 0., len);
                float radius = length(uv);

                uv = radius * float2(cos(angle), sin(angle)) + center;
                uv = MapToUVRect(uv, uvRect);

                half4 col = tex2D(_MainTex, uv);

                #if _UIE_OUTPUT_LINEAR
                col.rgb = GammaToLinearSpace(col.rgb);
                #endif

                return col;
            }
            ENDCG
        }
    }
}