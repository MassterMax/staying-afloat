Shader "Custom/BlackHoleCollapse"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Collapse ("Collapse", Range(0,1)) = 0
        _Distortion ("Distortion", Range(0,1)) = 0
        _BlackHolePos ("Black Hole Pos", Vector) = (0.5,0.5,0,0)
        _EdgeSoftness ("Edge Softness", Range(0.01,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Collapse;
            float _Distortion;
            float2 _BlackHolePos;
            float _EdgeSoftness;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                float2 dir = uv - _BlackHolePos;
                float dist = length(dir);

                // 1) Внешнее искажение — экспоненциально убывает с дистанцией
                float distort = _Distortion * exp(-dist * 8.0);
                uv += dir * distort * 0.25; // 0.25 — базовый масштаб, можно менять

                // 2) Схлопывание — линейное приближение к центру
                float collapse = saturate(_Collapse);
                uv = lerp(uv, _BlackHolePos, collapse);

                // 3) Чёрная область и мягкий край
                // чем ближе к центру, тем более чёрно; используем smoothstep для мягкости края
                float blackMask = smoothstep(0.0, _EdgeSoftness, dist - collapse * 0.5);
                fixed4 col = tex2D(_MainTex, uv);
                col.rgb *= blackMask;

                return col;
            }
            ENDCG
        }
    }
}
