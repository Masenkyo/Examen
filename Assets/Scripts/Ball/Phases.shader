Shader "BALLS/Phases"
{
    Properties
    {
        _Color ("Color", color) = (1, 1, 1, 1)
        _Phase1 ("Phase1Texture", 2D) = "white" {}
        _Phase2 ("Phase2Texture", 2D) = "white" {}
        _DamageAmount("DamageAmount", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            sampler2D _Phase1;
            sampler2D _Phase2;
            float _DamageAmount;

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
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 phase1 = tex2D(_Phase1, i.uv);
                fixed4 phase2 = tex2D(_Phase2, i.uv);

                return lerp(phase2, phase1, _DamageAmount) * _Color;
            }
            ENDCG
        }
    }
}
