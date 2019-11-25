Shader "NugPen/Prototype Pattern"
{
    Properties
    {
        _ColorA1 ("Color A1", Color) = (0.75, 0.75, 0.75, 1)
        _ColorA2 ("Color A2", Color) = (0.85, 0.85, 0.85, 1)
        _ColorB1 ("Color B1", Color) = (0.70, 0.70, 0.70, 1)
        _ColorB2 ("Color B2", Color) = (0.60, 0.60, 0.60, 1)
        _Scale ("Pattern Size", Range(0,10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                fixed4 vertex : POSITION;
                fixed3 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed4 vertex : SV_POSITION;
                fixed3 worldPos : TEXCOORD0;
            };

            fixed4 _ColorA1;
            fixed4 _ColorA2;
            fixed4 _ColorB1;
            fixed4 _ColorB2;
            fixed _Scale;

            v2f vert (appdata IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);
                return OUT;
            }

            fixed4 frag (v2f IN) : SV_Target
            {
                // Grid
                fixed3 scaledWorld = round((IN.worldPos + fixed3(0.5, 0, -0.5)) / _Scale);
                fixed checker = scaledWorld.x
                    + scaledWorld.y
                    + scaledWorld.z;
                checker = frac(checker * 0.5);
                checker *= 2;
                
                // Color
                fixed3 scaledColorWorld = round(IN.worldPos / 10 / _Scale);
                fixed colorChecker = scaledColorWorld.x
                    + scaledColorWorld.y
                    + scaledColorWorld.z;
                colorChecker = frac(colorChecker * 0.5);
                colorChecker *= 2;
                
                fixed4 colorA = lerp(_ColorA1, _ColorA2, checker);
                fixed4 colorB = lerp(_ColorB1, _ColorB2, checker);
                return lerp(colorA, colorB, colorChecker);
            }
            ENDHLSL
        }
    }
}
