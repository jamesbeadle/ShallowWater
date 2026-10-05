Shader "Shallow Water/Crops"
{
    Properties
    {
        _Ground ("Ground", Color) = (0.33, 0.4, 0.2, 1)
        _GroundWorn ("Worn ground", Color) = (0.42, 0.42, 0.24, 1)
        _Growth ("Growth", Color) = (0.24, 0.31, 0.14, 1)
        _Accent ("Accent", Color) = (0.58, 0.54, 0.38, 1)
        _Pattern ("Pattern", Float) = 0
        _RowMetres ("Row spacing in metres", Float) = 1
        _Cover ("Cover", Range(0, 1)) = 0.2
        _Relief ("Relief in metres", Float) = 0
        _Lifted ("Share lifted", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"

        fixed4 _Ground;
        fixed4 _GroundWorn;
        fixed4 _Growth;
        fixed4 _Accent;
        float _Pattern;
        float _RowMetres;
        float _Cover;
        float _Relief;
        float _Lifted;

        #include "Crops.cginc"
        #include "Tillage.cginc"
        #include "RootRows.cginc"

        struct Input
        {
            float2 surfacePlace;
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
        }

        CropSurface CropAt(float2 place, float2 ground)
        {
            if (_Pattern < 0.5) return Grass(place, ground);
            if (_Pattern < 1.5) return Stubble(place, ground);
            if (_Pattern < 2.5) return Plough(place, ground);
            if (_Pattern < 3.5) return Drilled(place, ground);
            if (_Pattern < 4.5) return RootRows(place, ground);
            if (_Pattern < 5.5) return PotatoRidges(place, ground);
            if (_Pattern < 6.5) return Footpath(place, ground);
            return HedgeBottom(place, ground);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            CropSurface crop = CropAt(IN.surfacePlace, IN.worldPos.xz);
            o.Albedo = crop.colour;
            o.Smoothness = crop.smoothness;
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), crop.height));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
