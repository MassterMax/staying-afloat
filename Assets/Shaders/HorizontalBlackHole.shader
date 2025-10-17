Shader "Custom/HorizontalBlackHole"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlackHoleX("Black Hole X", Range(0,1)) = 1.0
        _StrengthX("Horizontal Strength", Range(0,1)) = 0.5
        _StrengthY("Vertical Warp", Range(0,1)) = 0.3
        _SqueezeY("Vertical Squeeze", Range(0,1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _BlackHoleX;
            float _StrengthX;
            float _StrengthY;
            float _SqueezeY;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Горизонтальное всасывание
                float dx = _BlackHoleX - uv.x;
                uv.x += dx * _StrengthX;

                // Вогнутость по вертикали
                float distX = abs(dx);
                uv.y += (0.5 - uv.y) * _StrengthY * (1.0 - distX);

                // Сжатие по вертикали
                float centerY = 0.5;
                uv.y = centerY + (uv.y - centerY) * (1.0 - _SqueezeY * _StrengthX); 
                // Чем больше strengthX, тем сильнее сжатие

                uv = clamp(uv, 0.0, 1.0);
                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
