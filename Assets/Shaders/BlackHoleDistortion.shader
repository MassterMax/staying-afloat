Shader "Custom/BlackHoleDistortion"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _DistortionStrength("Distortion Strength", Float) = 0.5
        _BlackHolePos("Black Hole Pos", Vector) = (0.0,0.5,0,0)
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
            float _DistortionStrength;
            float2 _BlackHolePos;

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                float2 dir = uv - _BlackHolePos;
                float dist = length(dir);

                // простое радиальное смещение
                float strength = _DistortionStrength / max(dist, 0.01);
                uv += dir * strength * 0.1; // 0.1 — коэффициент, регулирует силу эффекта

                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
