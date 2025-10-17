Shader "Custom/SideBlackHole"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Distortion ("Distortion", Range(0,1)) = 0
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

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                // Центр черной дыры справа
                float2 holeCenter = float2(1.0, 0.5);
                float2 dir = holeCenter - uv;
                float dist = dir.x; // горизонтальная ось (справа налево)

                // --- Только геометрическое искажение ---
                float distortion = _Distortion * smoothstep(1.0, 0.0, dist);
                uv.x -= distortion * pow(1.0 - dist, 2.0);

                fixed4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
