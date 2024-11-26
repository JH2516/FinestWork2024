Shader "CustomRenderTexture/TextBlur"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // ÅØ½ºÃ³ »ùÇÃ·¯
            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // ÅØ½ºÃ³ÀÇ ÅØ¼¿ Å©±â (UV ÁÂÇ¥ »óÀÇ Å©±â)

            // Á¤Á¡ ½¦ÀÌ´õ
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment ½¦ÀÌ´õ
            float4 frag(v2f i) : SV_Target
            {
                // ÅØ½ºÃ³ÀÇ ÅØ¼¿ Å©±â (UV »ó¿¡¼­ ÇÑ ÇÈ¼¿ÀÇ Å©±â)
                float2 texelSize = float2(0.01, 0.01);

                // 5x5 ¿µ¿ªÀ» »ùÇÃ¸µÇÏ¿© Æò±Õ »ö»ó ±¸ÇÏ±â
                float4 color = float4(0.0, 0.0, 0.0, 0.0);
                int sampleCount = 0;

                for (int x = -2; x <= 2; ++x)
                {
                    for (int y = -2; y <= 2; ++y)
                    {
                        // »ùÇÃ¸µÇÒ UV ÁÂÇ¥ °è»ê
                        float2 sampleUV = i.uv + float2(x, y) * texelSize;
                        color += tex2D(_MainTex, sampleUV);
                        sampleCount++;
                    }
                }

                // Æò±Õ »ö»ó °è»ê
                color /= sampleCount;

                return color;
            }
            ENDCG
        }
    }
}
