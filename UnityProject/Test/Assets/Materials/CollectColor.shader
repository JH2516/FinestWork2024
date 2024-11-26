Shader "CustomRenderTexture/CollectColor"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" { }
        _ScreenTex ("Screen Tex", 2D) = "white" { }
        _BlurRadius ("Blur Radius", Range(0.0, 10.0)) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = v.vertex;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _ScreenTex;
            float _BlurRadius;

            float Gaussian(float x, float deviation)
            {
                return exp(-(x * x) / (2 * deviation * deviation)) / (deviation * sqrt(2 * UNITY_PI));
            }

            float4 frag(v2f i) : SV_Target
            {
                // 뒤에 있는 픽셀 색상 샘플링
                float2 screenPos = i.pos.xy / i.pos.w;
                float3 backColor = tex2D(_ScreenTex, screenPos).rgb;
                float2 texelSize = 1.0 / _ScreenParams.xy;

                return float4(backColor, 1.0);
            }
            ENDCG
        }
    }
}
