Shader "Shallow Water/Fields"
{
    Properties
    {
        _Pasture ("Pasture", Color) = (0.3, 0.41, 0.17, 1)
        _PastureWorn ("Worn pasture", Color) = (0.4, 0.43, 0.2, 1)
        _Stubble ("Stubble", Color) = (0.68, 0.6, 0.38, 1)
        _Plough ("Ploughland", Color) = (0.34, 0.26, 0.19, 1)
        _Hedgerow ("Hedgerow", Color) = (0.16, 0.22, 0.09, 1)
        _FieldMetres ("Field size in metres", Float) = 140
        _HedgerowMetres ("Hedgerow width in metres", Float) = 3
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Relief.cginc"

        #define FIELD_WARP 0.35
        #define FURROW_METRES 0.35
        #define TURF_METRES 9.0
        #define GRAIN_METRES 0.25

        fixed4 _Pasture;
        fixed4 _PastureWorn;
        fixed4 _Stubble;
        fixed4 _Plough;
        fixed4 _Hedgerow;
        float _FieldMetres;
        float _HedgerowMetres;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
            INTERNAL_DATA
        };

        float FieldRelief(float kind, float2 ground, float isHedgerow)
        {
            float2 furrowDirection = normalize(float2(cos(kind * 40.0), sin(kind * 40.0)));
            float furrowPlace = dot(ground, furrowDirection) / FURROW_METRES;
            float furrowDetail = PatternDetail(float2(furrowPlace, 0.0));
            float furrows = sin(furrowPlace * 6.2831853) * 0.04 * step(0.62, kind) * furrowDetail;
            float2 turfPlace = ground / GRAIN_METRES;
            float turf = (ValueNoise(turfPlace) - 0.5) * 0.02 * PatternDetail(turfPlace);
            return furrows + turf + isHedgerow * 0.4 * ValueNoise(ground * 0.8);
        }

        float3 FieldCrop(float kind, float2 ground)
        {
            float3 pasture = lerp(_Pasture.rgb, _PastureWorn.rgb, Fbm(ground / TURF_METRES));
            float2 furrowDirection = normalize(float2(cos(kind * 40.0), sin(kind * 40.0)));
            float furrowPlace = dot(ground, furrowDirection) / FURROW_METRES;
            float furrows = lerp(0.5, 0.5 + 0.5 * sin(furrowPlace * 6.2831853), PatternDetail(float2(furrowPlace, 0.0)));
            float3 stubble = _Stubble.rgb * lerp(0.85, 1.05, furrows);
            float3 plough = _Plough.rgb * lerp(0.75, 1.1, furrows);
            float3 crop = lerp(pasture, stubble, step(0.62, kind));
            return lerp(crop, plough, step(0.84, kind));
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 ground = IN.worldPos.xz;
            float2 place = ground / _FieldMetres;
            place += (float2(Fbm(place * 0.7), Fbm(place * 0.7 + 9.1)) - 0.5) * FIELD_WARP;
            float3 cells = Cells(place);
            float3 field = FieldCrop(cells.y, ground);
            float fromTheHedge = cells.x * _FieldMetres;
            float isHedgerow = 1.0 - smoothstep(_HedgerowMetres * 0.5, _HedgerowMetres, fromTheHedge);
            float3 colour = lerp(field, _Hedgerow.rgb, isHedgerow);
            float2 grainPlace = ground / GRAIN_METRES;
            float grain = lerp(0.5, ValueNoise(grainPlace), PatternDetail(grainPlace));
            o.Albedo = colour * lerp(0.85, 1.1, grain);
            o.Smoothness = 0.06;
            float relief = FieldRelief(cells.y, ground, isHedgerow);
            o.Normal = WORLD_TO_TANGENT(IN, Raised(IN.worldPos, VERTEX_NORMAL(IN), relief));
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
