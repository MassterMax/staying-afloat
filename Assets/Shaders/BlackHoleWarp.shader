Shader "Custom/BlackHoleWarp"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Center ("Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius (X,Y)", Vector) = (0.2, 0.3, 0, 0)
        _Strength ("Warp Strength", Range(0, 1)) = 0.5
        _Direction ("Pull Direction", Vector) = (1, 0, 0, 0)
        _GlitchIntensity ("Glitch Intensity", Range(0, 1)) = 0.3
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Center;
            float4 _Radius;
            float _Strength;
            float4 _Direction;
            float _GlitchIntensity;

            // простая детерминированная функция шума
            float random(float2 p)
            {
                return frac(sin(dot(p, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                // --- овальная область ---
                float2 delta = uv - _Center.xy;
                float2 ellipse = delta / _Radius.xy;
                float dist = length(ellipse);
                float mask = saturate(1.0 - dist);

                // --- втягивание ---
                if (mask > 0.0)
                {
                    float pull = mask * _Strength;
                    uv -= _Direction.xy * pull * (1.0 - dist);
                }

                // --- статический глитч ---
                // шум зависит только от позиции пикселя
                float noise = random(float2(floor(uv.y * 100.0), floor(uv.x * 100.0)));
                float glitchOffset = (noise - 0.5) * _GlitchIntensity * mask;

                uv.x += glitchOffset;

                // RGB split
                float2 offset = float2(glitchOffset * 1.5, 0);
                fixed4 col;
                col.r = tex2D(_MainTex, uv + offset).r;
                col.g = tex2D(_MainTex, uv).g;
                col.b = tex2D(_MainTex, uv - offset).b;
                col.a = 1;

                return col;
            }
            ENDCG
        }
    }
}
