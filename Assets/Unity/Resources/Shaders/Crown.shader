Shader "Shallow Water/Crown"
{
    Properties
    {
        _Colour ("Summer green", Color) = (0.25, 0.33, 0.12, 1)
        _AutumnGold ("Autumn gold", Color) = (0.72, 0.58, 0.16, 1)
        _AutumnRust ("Autumn rust", Color) = (0.62, 0.3, 0.1, 1)
        _AutumnRed ("Autumn red", Color) = (0.55, 0.14, 0.08, 1)
        _Gaps ("Shade inside the crown", Color) = (0.05, 0.06, 0.03, 1)
        _Sway ("Sway in the wind", Float) = 0.012
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow
        #pragma multi_compile_instancing
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"

        #define CLUMPS_ACROSS_THE_CROWN 7.0
        #define LEAVES_ACROSS_THE_CROWN 22.0
        #define GUST_SPEED 1.3
        #define FULL_TURN 6.2831853

        fixed4 _Colour;
        fixed4 _AutumnGold;
        fixed4 _AutumnRust;
        fixed4 _AutumnRed;
        fixed4 _Gaps;
        float _Sway;

        struct Input
        {
            float3 crownPlace;
            float seed;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            float3 root = mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;
            float seed = Hash21(root.xz);
            float lift = saturate(v.vertex.y + 0.5);
            float phase = seed * FULL_TURN + _Time.y * GUST_SPEED;
            v.vertex.x += sin(phase) * _Sway * lift;
            v.vertex.z += cos(phase * 0.7) * _Sway * 0.6 * lift;
            o.crownPlace = v.vertex.xyz;
            o.seed = seed;
        }

        float3 AutumnTint(float seed)
        {
            float3 turning = lerp(_Colour.rgb, _AutumnGold.rgb, smoothstep(0.15, 0.45, seed));
            float3 turned = lerp(turning, _AutumnRust.rgb, smoothstep(0.5, 0.75, seed));
            return lerp(turned, _AutumnRed.rgb, smoothstep(0.82, 0.97, seed));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 place = IN.crownPlace;
            float detail = PatternDetail(place.xz * LEAVES_ACROSS_THE_CROWN + place.y * LEAVES_ACROSS_THE_CROWN);
            float3 leaf = AutumnTint(IN.seed);
            float stillGreen = smoothstep(0.55, 0.85, ValueNoise3(place * 4.0 + IN.seed * 31.0));
            leaf = lerp(leaf, _Colour.rgb, stillGreen * 0.45);
            float clumps = ValueNoise3(place * CLUMPS_ACROSS_THE_CROWN + IN.seed * 17.0);
            float leaves = lerp(0.5, ValueNoise3(place * LEAVES_ACROSS_THE_CROWN), detail);
            float isLeafy = smoothstep(0.22, 0.62, clumps);
            float3 colour = lerp(_Gaps.rgb, leaf * (0.75 + 0.5 * leaves), lerp(0.8, isLeafy, detail));
            float outer = saturate(place.y + 0.65);
            o.Albedo = colour * lerp(0.6, 1.0, outer);
            o.Occlusion = lerp(0.5, 1.0, isLeafy * outer);
            o.Smoothness = 0.1;
            float relief = (clumps - 0.5) * 0.3 + (leaves - 0.5) * 0.05 * detail;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
