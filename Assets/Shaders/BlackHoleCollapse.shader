Shader "Custom/BlackHoleCollapse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Collapse ("Collapse", Range(0,1)) = 0
        _Distortion ("Distortion", Range(0,1)) = 0
        _BlackHolePos ("Black Hole Pos", Vector) = (0.5,0.5,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ZTest Always Cull Off ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Collapse;
            float _Distortion;
            float2 _BlackHolePos;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dir = uv - _BlackHolePos;
                float dist = length(dir);

                // --- Этап 1: гравитационное искажение ---
                float distort = _Distortion * exp(-dist * 8); // экспоненциальное искривление у центра
                uv += dir * distort;

                // --- Этап 2: схлопывание пространства ---
                float collapse = smoothstep(0.0, 1.0, _Collapse);
                uv = lerp(uv, _BlackHolePos, collapse);

                fixed4 col = tex2D(_MainTex, uv);

                // --- Этап 3: чернота по радиусу ---
                float black = smoothstep(0.5, 0.1, dist - collapse * 0.5);
                col.rgb *= black;

                return col;
            }
            ENDCG
        }
    }
}
