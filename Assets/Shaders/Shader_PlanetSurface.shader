Shader "Custom/Shader_PlanetSurface"
{
    Properties
    {
        _LightDir ("Light Direction", Vector) = (0, 1, 0)
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 localPos : TEXCOORD0; // Local position for mapping
                float3 normal : TEXCOORD1; // Object space normal for lighting
            };

            // Directional light direction
            float4 _LightDir;

            // Hash function for random values
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

            // Fractal Brownian Motion (FBM) for detailed noise
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

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // Use object space position to keep patterns mapped to the object
                o.localPos = v.vertex.xyz;
                o.normal = v.normal; // Object space normal
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Map local position to a spherical shape
                float3 spherePos = normalize(i.localPos);

                // Generate elevation data using FBM
                float elevation = fbm(spherePos * 5.0); // Higher frequency for detailed patterns

                // Define thresholds for different terrain types
                float oceanThreshold = 0.4; // Below this is ocean
                float desertThreshold = 0.6; // Between ocean and land, some regions are deserts

                // Calculate colors for different biomes
                float3 oceanColor = float3(0.0, 0.3, 0.8); // Blue ocean
                float3 landColor = float3(0.2, 0.8, 0.3); // Green land
                float3 desertColor = float3(0.8, 0.7, 0.2); // Yellowish sand

                // Blend between ocean, desert, and land
                float3 color;
                if (elevation < oceanThreshold)
                {
                    color = oceanColor; // Ocean
                }
                else if (elevation < desertThreshold)
                {
                    // Blend between ocean and desert
                    float t = (elevation - oceanThreshold) / (desertThreshold - oceanThreshold);
                    color = lerp(oceanColor, desertColor, t);
                }
                else
                {
                    // Blend between desert and land
                    float t = (elevation - desertThreshold) / (1.0 - desertThreshold);
                    color = lerp(desertColor, landColor, t);
                }

                // Apply simple directional lighting
                float3 lightDir = normalize(_LightDir.xyz);
                float diffuse = max(0.0, dot(normalize(i.normal), lightDir));
                color *= diffuse + 0.2; // Add ambient light

                return fixed4(color, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
