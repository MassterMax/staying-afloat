Shader "Custom/BlackHoleWarp"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Warp ("Warp", Range(0, 1)) = 0
        _BlackHolePos ("Black Hole Pos", Vector) = (0.5, 0.5, 0, 0)
        _Falloff ("Falloff", Range(0.1, 10)) = 3
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
            float _Warp;
            float2 _BlackHolePos;
            float _Falloff;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dir = uv - _BlackHolePos;
                float dist = length(dir);

                // --- Эффект гравитационного искажения ---
                // Приближение: чем ближе к центру, тем сильнее растягивает края
                float warpFactor = 1.0 / (1.0 + _Warp * pow(dist * _Falloff + 1.0, 2.0));

                // Сдвигаем UV в направлении от центра, усиливая ощущение туннеля
                uv = _BlackHolePos + dir * warpFactor;

                // --- Черная область ---
                float black = smoothstep(0.5, 0.0, dist * (1.0 + _Warp * 3.0));

                fixed4 col = tex2D(_MainTex, uv);
                col.rgb *= black;

                return col;
            }
            ENDCG
        }
    }
}
