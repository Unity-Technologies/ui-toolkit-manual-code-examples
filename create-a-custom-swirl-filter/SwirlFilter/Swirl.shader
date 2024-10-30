Shader "Unlit/Swirl"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.hlsl"

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

            float _Angle;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float effectRadius = _Radius;
                float effectAngle = _Angle;
                
                float2 center = float2(0.5, 0.5);
                
                float2 uv = i.uv - center;
                
                float len = length(uv * float2(_MainTex_TexelSize.z * _MainTex_TexelSize.y, 1.));
                float angle = atan2(uv.y, uv.x) + effectAngle * smoothstep(effectRadius, 0., len);
                float radius = length(uv);

                uv = radius * float2(cos(angle), sin(angle)) + center;
                fixed4 fragColor = _MainTex.Sample(my_linear_repeat_sampler, uv);
                
                return fragColor;
            }
            ENDCG
        }
    }
}
