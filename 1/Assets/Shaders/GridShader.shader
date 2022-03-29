Shader "Unlit/GridShader"
{
    Properties
    {
        _BackgroundColor("BackgroundColor", Color) = (0, 0, 0, 1)
        _BorderColor("BorderColor", Color) = (1, 1, 1, 1)
        _ScaleX("Scale x", Float) = 1
        _ScaleY("Scale y", Float) = 1
        _Sigma("Sigma", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 _BackgroundColor;
            half4 _BorderColor;
            half _ScaleX;
            half _ScaleY;
            half _Sigma;

            fixed4 frag(v2f i) : SV_Target
            {
                half sigma = 0.01;

                half2 p = (i.uv * half2(_ScaleX, _ScaleY)) % 1.0;

                half padding = _Sigma * 3.0;

                half4 x = half4(p - half2(padding, padding), p - half2(1.0 - padding, 1.0 - padding)) * (sqrt(0.5) / sigma);

                half4 s = sign(x), a = abs(x);
                x = 1.0 + (0.278393 + (0.230389 + 0.078108 * (a * a)) * a) * a;
                x *= x;

                half4 integral = 0.5 + 0.5 * (s - s / (x * x));
                half t = 1.0 - (integral.z - integral.x) * (integral.w - integral.y);
                return lerp(_BackgroundColor, _BorderColor, pow(t, 8.0));
            }
            ENDCG
        }
    }
}
