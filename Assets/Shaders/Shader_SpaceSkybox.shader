Shader "Custom/Shader_SpaceSkybox"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0, 0, 0, 1)
        _StarColor ("Star Color", Color) = (1, 1, 1, 1)
        _NebulaColor ("Nebula Color", Color) = (0.5, 0.2, 1, 1)
        _GalaxyColor ("Galaxy Color", Color) = (1, 0.5, 0.2, 1)
        _StarDensity ("Star Density", Range(0.0, 1.0)) = 0.2
        _NebulaIntensity ("Nebula Intensity", Range(0.0, 2.0)) = 0.8
        _GalaxyIntensity ("Galaxy Intensity", Range(0.0, 2.0)) = 1.0
        _NebulaMovementSpeed ("Nebula Movement Speed", Range(0.0, 0.1)) = 0.02
        _GalaxyRotationSpeed ("Galaxy Rotation Speed", Range(0.0, 1.0)) = 0.05
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 direction : TEXCOORD0; // View direction
            };

            // Properties
            float4 _BackgroundColor;
            float4 _StarColor;
            float4 _NebulaColor;
            float4 _GalaxyColor;
            float _StarDensity;
            float _NebulaIntensity;
            float _GalaxyIntensity;
            float _NebulaMovementSpeed;
            float _GalaxyRotationSpeed;

            // Hash function for pseudo-random values
            float hash(float3 p)
            {
                p = frac(p * 0.3183099 + float3(0.1, 0.2, 0.3));
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            // Smooth noise function
            float noise(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);

                float n = lerp(
                    lerp(
                        lerp(hash(i + float3(0, 0, 0)), hash(i + float3(1, 0, 0)), f.x),
                        lerp(hash(i + float3(0, 1, 0)), hash(i + float3(1, 1, 0)), f.x), f.y),
                    lerp(
                        lerp(hash(i + float3(0, 0, 1)), hash(i + float3(1, 0, 1)), f.x),
                        lerp(hash(i + float3(0, 1, 1)), hash(i + float3(1, 1, 1)), f.x), f.y),
                    f.z);
                return n;
            }

            // Fractal Brownian Motion (FBM) for complex noise patterns
            float fbm(float3 p)
            {
                float total = 0.0;
                float amplitude = 1.0;
                float frequency = 1.0;
                for (int i = 0; i < 5; i++)
                {
                    total += noise(p * frequency) * amplitude;
                    frequency *= 2.0;
                    amplitude *= 0.5;
                }
                return total;
            }

            // Galaxy pattern using spiral noise
            float galaxyPattern(float3 dir, float rotationAngle)
            {
                // Apply rotation to the galaxy pattern
                float angle = atan2(dir.y, dir.x) + rotationAngle; // Rotate the galaxy around the center
                float radius = length(dir.xy); // Distance from the center
                float spiral = sin(10.0 * angle - radius * 5.0); // Spiral arms
                return smoothstep(0.3, 1.0, spiral);
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Convert vertex to direction (normalized)
                o.direction = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.vertex.xyz));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 dir = i.direction;

                // Time-based rotation angle for the galaxy
                float rotationAngle = _Time.y * _GalaxyRotationSpeed * 6.283185307179586; // 2 * PI

                // Star field (scattered points)
                float starNoise = fbm(dir * 20.0);
                float stars = step(1.0 - _StarDensity, starNoise);

                // Nebula (smooth cloud-like formations) with slight movement
                float nebula = fbm(dir * 8.0 + _Time.y * _NebulaMovementSpeed) * (_NebulaIntensity + 0.1 * sin(_Time.y));

                // Galaxy (spiral structures) with rotation
                float galaxy = galaxyPattern(dir, rotationAngle) * (_GalaxyIntensity + 0.05 * cos(_Time.y));

                // Combine colors
                float3 color = _BackgroundColor.rgb;
                color += stars * _StarColor.rgb; // Add stars
                color += nebula * _NebulaColor.rgb; // Add nebula
                color += galaxy * _GalaxyColor.rgb; // Add galaxy

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}