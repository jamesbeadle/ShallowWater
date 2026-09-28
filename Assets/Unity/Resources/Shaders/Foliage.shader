Shader "Shallow Water/Foliage"
{
    Properties
    {
        _Colour ("Leaves still green", Color) = (0.22, 0.27, 0.1, 1)
        _Turning ("Leaves turning", Color) = (0.52, 0.45, 0.16, 1)
        _Turned ("Leaves turned", Color) = (0.4, 0.25, 0.1, 1)
        _Autumn ("How far autumn has come", Range(0, 1)) = 0.35
        _AutumnSpread ("How much trees differ", Range(0, 1)) = 0.5
        _LeafShape ("Leaf: 0 oak, 1 ash, 2 birch, 3 needles, 4 hazel", Float) = 0
        _LeafMetres ("Length of a leaf", Float) = 0.16
        _Translucency ("Light through the leaves", Float) = 0.6
        _Sway ("Sway in the wind", Float) = 0.12
        _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType" = "TransparentCutout" "Queue" = "AlphaTest" }
        LOD 200
        Cull Off

        CGPROGRAM
        #pragma surface surf Foliage vertex:vert alphatest:_Cutoff addshadow fullforwardshadows
        #pragma multi_compile_instancing
        #pragma target 3.5
        #include "UnityPBSLighting.cginc"
        #include "Noise.cginc"
        #include "Wind.cginc"
        #include "Leaves.cginc"

        #define OUTWARD_SHARE 0.5
        #define CARD_BULGE 0.2
        #define FLUTTER_SPEED 2.3
        #define FLUTTER_SPREAD 40.0
        #define FLUTTER_RADIANS 0.06
        #define TRANSLUCENCY_FOCUS 3.0
        #define DEEPEST_SHADE 0.78
        #define DEEPEST_OCCLUSION 0.45
        #define ORTHOGRAPHIC 0.5
        #define SHADOW_PUSH 0.6

        fixed4 _Colour;
        fixed4 _Turning;
        fixed4 _Turned;
        float _Autumn;
        float _AutumnSpread;
        float _LeafShape;
        float _LeafMetres;
        float _Translucency;
        float _Sway;

        struct Input
        {
            float4 card;
            float treeSeed;
            float halfLeavesAcross;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float treeSeed = TreeSeed();
            float4 card = v.texcoord1;
            float spin = card.w + sin(_Time.y * FLUTTER_SPEED + card.y * FLUTTER_SPREAD) * FLUTTER_RADIANS;
            float2 corner = Turned(v.texcoord.xy, spin);
            float3 centre = v.vertex.xyz + Swayed(v.vertex.xyz, treeSeed, _Sway);
            #if defined(UNITY_PASS_SHADOWCASTER)
            bool isTheSunsShadow = UNITY_MATRIX_P[3][3] > ORTHOGRAPHIC;
            float3 awayFromTheSun = -normalize(mul((float3x3)unity_WorldToObject, _WorldSpaceLightPos0.xyz));
            centre += isTheSunsShadow ? awayFromTheSun * card.x * SHADOW_PUSH : float3(0, 0, 0);
            #endif
            float3 across = normalize(mul((float3x3)unity_WorldToObject, UNITY_MATRIX_V[0].xyz));
            float3 upward = normalize(mul((float3x3)unity_WorldToObject, UNITY_MATRIX_V[1].xyz));
            float3 reach = across * corner.x + upward * corner.y;
            v.vertex.xyz = centre + reach * card.x * 0.5;
            float3 towardTheEye = normalize(ObjSpaceViewDir(float4(centre, 1)));
            v.normal = normalize(lerp(towardTheEye, v.normal, OUTWARD_SHARE) + reach * CARD_BULGE);
            o.card = float4(v.texcoord.xy, card.y, card.z);
            o.halfLeavesAcross = card.x * 0.5 / _LeafMetres;
            o.treeSeed = treeSeed;
        }

        float3 LeafColour(float treeSeed, float tone)
        {
            float turning = saturate(_Autumn + (treeSeed - 0.5) * _AutumnSpread);
            float hasTurned = smoothstep(tone - 0.12, tone + 0.12, turning);
            float3 turned = lerp(_Turning.rgb, _Turned.rgb, frac(tone * 7.31));
            float3 green = _Colour.rgb * (0.85 + 0.3 * frac(tone * 3.7));
            return lerp(green, turned, hasTurned) * (0.7 + 0.45 * frac(tone * 13.7));
        }

        inline half4 LightingFoliage (SurfaceOutputStandard s, half3 viewDir, UnityGI gi)
        {
            half4 lit = LightingStandard(s, viewDir, gi);
            half through = pow(saturate(dot(viewDir, -gi.light.dir)), TRANSLUCENCY_FOCUS);
            lit.rgb += s.Albedo * gi.light.color * through * _Translucency * s.Occlusion;
            return lit;
        }

        inline void LightingFoliage_GI (SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
        {
            LightingStandard_GI(s, data, gi);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            #if defined(UNITY_PASS_SHADOWCASTER)
            bool isTheSunsShadow = UNITY_MATRIX_P[3][3] > ORTHOGRAPHIC;
            o.Alpha = LeafShadow(IN.card.xy, IN.card.z, IN.halfLeavesAcross);
            if (isTheSunsShadow) return;
            #endif
            LeafCover cover = LeavesOnACard(IN.card.xy, IN.card.z, _LeafShape, IN.halfLeavesAcross);
            float depth = IN.card.w;
            float3 leaf = LeafColour(IN.treeSeed, cover.tone) * lerp(0.65, 1.05, cover.rib);
            o.Albedo = leaf * lerp(1.0, DEEPEST_SHADE, depth);
            o.Occlusion = lerp(1.0, DEEPEST_OCCLUSION, depth);
            o.Smoothness = 0.22;
            o.Alpha = cover.amount;
        }
        ENDCG
    }
    FallBack "Legacy Shaders/Transparent/Cutout/VertexLit"
}
