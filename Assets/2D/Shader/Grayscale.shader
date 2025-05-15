Shader "PostProcessing/Grayscale"
{
    Properties
    {
        // _GrabTexture ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        ZWrite Off
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha

        GrabPass {}

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
            
            sampler2D _GrabTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 color = tex2D(_GrabTexture, i.uv);
                float gray = dot(color.rgb, float3(0.299, 0.587, 0.114));
                float4 graycolor = float4(gray, gray, gray, color.a);
                
                return graycolor;
            }
            ENDCG
        }
    }
}
