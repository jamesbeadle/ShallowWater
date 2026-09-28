Shader "Shallow Water/Brick"
{
    Properties
    {
        _Colour ("Brick colour", Color) = (0.55, 0.3, 0.22, 1)
        _Variation ("Second brick colour", Color) = (0.42, 0.22, 0.17, 1)
        _Mortar ("Mortar colour", Color) = (0.62, 0.58, 0.52, 1)
        _Frame ("Window frame paint", Color) = (0.88, 0.86, 0.8, 1)
        _Glass ("Glass", Color) = (0.1, 0.12, 0.14, 1)
        _Curtain ("Curtains", Color) = (0.55, 0.35, 0.3, 1)
        _Door ("Door paint", Color) = (0.12, 0.25, 0.18, 1)
        _Sill ("Sills and steps", Color) = (0.7, 0.68, 0.62, 1)
        _HasWindows ("Has windows", Float) = 0
        _WindowOffset ("Windows either side of the middle, in metres", Float) = 0
        _HasDoor ("Has a door in the middle", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert
        #pragma target 3.5
        #include "Noise.cginc"
        #include "Openings.cginc"

        #define BRICK_LENGTH 0.225
        #define COURSE_HEIGHT 0.075
        #define JOINT_ALONG 0.045
        #define JOINT_UP 0.13

        fixed4 _Colour;
        fixed4 _Variation;
        fixed4 _Mortar;
        float _HasWindows;
        float _WindowOffset;
        float _HasDoor;

        struct Input
        {
            float3 worldPos;
            float3 worldNormal;
            float2 surfacePlace;
        };

        void vert (inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.surfacePlace = v.texcoord.xy;
        }

        float3 Bricks(float3 position, float3 normal)
        {
            bool isLevel = abs(normal.y) > 0.5;
            float2 wall = isLevel ? position.xz : float2(AlongTheFace(position, normal), position.y);
            float course = floor(wall.y / COURSE_HEIGHT);
            float2 place = float2(wall.x / BRICK_LENGTH + frac(course * 0.5), wall.y / COURSE_HEIGHT);
            float2 inside = frac(place);
            float3 brick = lerp(_Colour.rgb, _Variation.rgb, Hash21(floor(place)));
            brick *= 0.88 + 0.24 * ValueNoise(place * float2(5.0, 2.0));
            bool isJoint = inside.x < JOINT_ALONG || inside.y < JOINT_UP;
            float3 coursed = isJoint ? _Mortar.rgb : brick;
            float3 average = lerp(lerp(_Colour.rgb, _Variation.rgb, 0.5), _Mortar.rgb, 0.18);
            return lerp(average, coursed, PatternDetail(place));
        }

        Opening OpeningAt(float2 wallPlace)
        {
            Opening none = (Opening)0;
            float2 inWindowColumn = float2(abs(abs(wallPlace.x) - _WindowOffset), wallPlace.y);
            Opening ground = Window(inWindowColumn, GROUND_FLOOR_WINDOW);
            Opening upper = Window(inWindowColumn, UPPER_FLOOR_WINDOW);
            Opening door = Door(wallPlace);
            door.coverage *= _HasDoor;
            Opening opening = Covering(Covering(Covering(none, ground), upper), door);
            opening.coverage *= _HasWindows;
            return opening;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float3 wall = Bricks(IN.worldPos, IN.worldNormal);
            float weathering = Fbm(IN.worldPos.xz * 0.8 + IN.worldPos.y * 0.3);
            wall *= lerp(0.82, 1.05, weathering);
            Opening opening = OpeningAt(IN.surfacePlace);
            o.Albedo = opening.coverage > 0.0 ? opening.colour : wall;
            o.Smoothness = opening.coverage > 0.0 ? opening.smoothness : 0.12;
            o.Alpha = 1;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
