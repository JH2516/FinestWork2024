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

            // �ؽ�ó ���÷�
            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // �ؽ�ó�� �ؼ� ũ�� (UV ��ǥ ���� ũ��)

            // ���� ���̴�
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

            // Fragment ���̴�
            float4 frag(v2f i) : SV_Target
            {
                // �ؽ�ó�� �ؼ� ũ�� (UV �󿡼� �� �ȼ��� ũ��)
                float2 texelSize = float2(0.01, 0.01);

                // 5x5 ������ ���ø��Ͽ� ��� ���� ���ϱ�
                float4 color = float4(0.0, 0.0, 0.0, 0.0);
                int sampleCount = 0;

                for (int x = -2; x <= 2; ++x)
                {
                    for (int y = -2; y <= 2; ++y)
                    {
                        // ���ø��� UV ��ǥ ���
                        float2 sampleUV = i.uv + float2(x, y) * texelSize;
                        color += tex2D(_MainTex, sampleUV);
                        sampleCount++;
                    }
                }

                // ��� ���� ���
                color /= sampleCount;

                return color;
            }
            ENDCG
        }
    }
}
