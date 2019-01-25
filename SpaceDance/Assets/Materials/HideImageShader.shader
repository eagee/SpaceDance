Shader "Sprites/HideImageShader" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Base (RGB)", 2D) = "white" {}
        _BottomLimit("Bottom Limit: World Pos Y", Float) = 10.0
        _TopLimit("Top Limit: World Pos Y", Float) = -10.0
        _SinOffset("Top Limit: World Pos Y", Float) = 0.0
    }
        SubShader{
        Lighting Off
        AlphaTest Greater 0.5

        Tags
    {
        "Queue" = "Transparent"
        "IgnoreProjector" = "True"
        "RenderType" = "Transparent"
        "PreviewType" = "Plane"
        "CanUseSpriteAtlas" = "True"
    }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog{ Mode Off }
        Blend One OneMinusSrcAlpha
        LOD 200

        CGPROGRAM
#pragma surface surf NoLighting alpha
#include "UnityCG.cginc"

        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) {
        fixed4 c;
        c.rgb = s.Albedo;
        c.a = s.Alpha;
        return c;
    }

    sampler2D _MainTex;
    float _BottomLimit;
    float _TopLimit;
    float _SinOffset;
    fixed4 _Color;

    struct Input {
        float2 uv_MainTex;
        float3 worldPos;
    };

    void surf(Input IN, inout SurfaceOutput o) {
        float bottomLimit = sin(IN.worldPos.x + _SinOffset) * 0.5f + _BottomLimit;
        float topLimit = sin(IN.worldPos.x + _SinOffset) * 0.5f + _TopLimit;
        float changeAlpha = 0.0f;
        if (IN.worldPos.y < bottomLimit - 0.75)
        {
            clip(-1);
        }
        if (IN.worldPos.y < bottomLimit)
        {
            changeAlpha = 1.0f - abs(bottomLimit - 0.75 - IN.worldPos.y);
        }
        if (IN.worldPos.y > topLimit)
        {
            clip(-1);
        }
        // Albedo comes from a texture tinted by color
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
        o.Albedo = c.rgb;
        o.Alpha = c.a - changeAlpha;
        //half4 c = tex2D(_MainTex, IN.uv_MainTex);
        //o.Albedo = c.rgb;
        //o.Alpha = c.a - changeAlpha;
    }
    ENDCG
        }
            FallBack "Diffuse"
}