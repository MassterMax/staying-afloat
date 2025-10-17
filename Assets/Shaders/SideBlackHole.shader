Shader "Custom/SideBlackHole"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Range(0,1)) = 0
        _Collapse ("Collapse", Range(0,1)) = 0
        _BlackHoleEdge ("BlackHoleEdge", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Distortion;
            float _Collapse;
            float _BlackHoleEdge;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                
                // Центр черной дыры — правый край экрана
                float2 holeCenter = float2(1.0, 0.5);
                float2 dir = holeCenter - uv;
                float dist = dir.x; // важна только горизонтальная ось (справа налево)

                // --- Искажение: "засасывает" справа ---
                float distortion = _Distortion * smoothstep(1.0, 0.0, dist);
                uv.x -= distortion * pow(1.0 - dist, 2.0);

                // --- Схлопывание: при Collapse → 1 всё сжимается влево ---
                float collapse = _Collapse;
                uv.x = lerp(uv.x, 0.5, collapse * 0.9); // схлопывает горизонтально

                fixed4 col = tex2D(_MainTex, uv);

                // --- Чёрная область справа ---
                float black = smoothstep(_BlackHoleEdge, _BlackHoleEdge - 0.05, 1.0 - i.uv.x);
                black = saturate(black + collapse * 1.5);
                col.rgb *= (1.0 - black);

                return col;
            }
            ENDCG
        }
    }
}
