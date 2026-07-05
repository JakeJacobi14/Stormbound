Shader "Custom/OceanCycloneSwirl_v2" {
    Properties {
        [Header(Ocean)]
        _OceanColor ("Ocean Color", Color) = (0.1, 0.3, 0.5, 1.0) // Deep Blue/Green ocean base
        _WaveSpeed ("Ocean Wave Speed", Float) = 1.0
        _WaveFrequency ("Ocean Wave Frequency", Float) = 10.0
        _WaveAmplitude ("Ocean Wave Amplitude", Float) = 0.01 // Gentle displacement

        [Header(Cyclones)]
        _CycloneColorRed ("Cyclone Color Red", Color) = (1.0, 0.2, 0.1, 1.0) // Vibrant Red
        _CycloneColorBlue ("Cyclone Color Blue", Color) = (0.1, 0.2, 1.0, 1.0) // Vibrant Blue

        // Cyclone 1 Properties (Red)
        _Cyclone1_Center ("Cyclone 1 Center (Screen Pos)", Vector) = (0.5, 0.5, 0, 0) // Normalized screen coordinates (0-1)
        _Cyclone1_Strength ("Cyclone 1 Strength", Float) = 0.5 // How strong the swirl is
        _Cyclone1_Radius ("Cyclone 1 Radius", Float) = 0.3 // How far the swirl extends

        // Cyclone 2 Properties (Blue)
        _Cyclone2_Center ("Cyclone 2 Center (Screen Pos)", Vector) = (0.25, 0.75, 0, 0)
        _Cyclone2_Strength ("Cyclone 2 Strength", Float) = 0.4
        _Cyclone2_Radius ("Cyclone 2 Radius", Float) = 0.25

        // Cyclone 3 Properties (Red)
        _Cyclone3_Center ("Cyclone 3 Center (Screen Pos)", Vector) = (0.75, 0.25, 0, 0)
        _Cyclone3_Strength ("Cyclone 3 Strength", Float) = 0.6
        _Cyclone3_Radius ("Cyclone 3 Radius", Float) = 0.35

        // Main Texture (can be used as a base for the ocean if needed)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Oaque" } // "Opaque" or "Transparent" depending on usage
        LOD 100

        Pass {
            Cull Off
            ZWrite Off
            // Blend SrcAlpha OneMinusSrcAlpha // Uncomment for transparency

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            // Properties from the Shaderlab block
            float4 _OceanColor;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;

            float4 _CycloneColorRed;
            float4 _CycloneColorBlue;

            float4 _Cyclone1_Center;
            float _Cyclone1_Strength;
            float _Cyclone1_Radius;

            float4 _Cyclone2_Center;
            float _Cyclone2_Strength;
            float _Cyclone2_Radius;

            float4 _Cyclone3_Center;
            float _Cyclone3_Strength;
            float _Cyclone3_Radius;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0; // Screen position for effects
                float2 uv : TEXCOORD1;       // Original mesh UVs
            };

            // Struct to return multiple values from helper function
            struct CycloneEffectResult {
                float2 swirlOffset;
                float influence;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            // Helper function to calculate swirl offset and influence for a single cyclone
            // Returns a struct to avoid complex output parameter interactions
            CycloneEffectResult CalculateCycloneEffect(float2 screenUV_normalized, float4 cycloneCenter_normalized,
                                                    float strength, float radius, float time)
            {
                CycloneEffectResult result; // Declare the result struct

                // Convert normalized screen UV and center to a consistent space (e.g., -0.5 to 0.5 range)
                float2 currentPos = screenUV_normalized - 0.5;
                float2 centerPos = cycloneCenter_normalized.xy - 0.5;

                float2 toCenter = currentPos - centerPos;
                float dist = length(toCenter);

                // Calculate influence based on distance to the center
                // Influence is 1 at the center, falls off linearly to 0 at the radius
                result.influence = max(0.0, (1.0 - dist / radius));

                // Calculate swirl amount - stronger closer to the center
                // Using a small epsilon to avoid division by zero right at the center
                // Only apply swirl within the radius
                float swirlAmount = strength / (dist + 0.001) * result.influence;

                // Calculate the angle offset based on swirl amount and time
                float angleOffset = swirlAmount * time;

                // Calculate the swirled position relative to the center
                float currentAngle = atan2(toCenter.y, toCenter.x);
                float newAngle = currentAngle + angleOffset;

                // Calculate the swirled offset from the original position
                float2 swirledPos = float2(dist * cos(newAngle), dist * sin(newAngle)) + centerPos;
                result.swirlOffset = swirledPos - currentPos;

                return result; // Return the struct
            }


            float4 frag(v2f i) : SV_Target {
                // Get normalized screen coordinates (0 to 1 range)
                float2 screenUV_normalized = i.screenPos.xy / i.screenPos.w;

                // --- Add a base wavy ocean effect ---
                float2 oceanUV_displaced = screenUV_normalized;
                float time = _Time.y;

                // Simple Perlin-like noise for waves (using sin/cos based on position and time)
                float waveNoiseX = sin(oceanUV_displaced.y * _WaveFrequency + time * _WaveSpeed) * _WaveAmplitude;
                float waveNoiseY = cos(oceanUV_displaced.x * _WaveFrequency * 0.8 + time * _WaveSpeed * 0.7) * _WaveAmplitude;

                // Apply wave displacement
                oceanUV_displaced.x += waveNoiseX;
                oceanUV_displaced.y += waveNoiseY;


                // --- Calculate combined swirl effect from cyclones ---
                float2 totalSwirlOffset = 0;
                float totalRedInfluence = 0;
                float totalBlueInfluence = 0;

                // Calculate effect for each cyclone and accumulate results
                CycloneEffectResult res1 = CalculateCycloneEffect(oceanUV_displaced, _Cyclone1_Center, _Cyclone1_Strength, _Cyclone1_Radius, time);
                totalSwirlOffset += res1.swirlOffset;
                totalRedInfluence += res1.influence; // Cyclone 1 is Red

                CycloneEffectResult res2 = CalculateCycloneEffect(oceanUV_displaced, _Cyclone2_Center, _Cyclone2_Strength, _Cyclone2_Radius, time);
                totalSwirlOffset += res2.swirlOffset;
                totalBlueInfluence += res2.influence; // Cyclone 2 is Blue

                CycloneEffectResult res3 = CalculateCycloneEffect(oceanUV_displaced, _Cyclone3_Center, _Cyclone3_Strength, _Cyclone3_Radius, time);
                totalSwirlOffset += res3.swirlOffset;
                totalRedInfluence += res3.influence; // Cyclone 3 is Red

                // Apply the total swirl displacement to the displaced ocean UV
                float2 finalUV_for_color = oceanUV_displaced + totalSwirlOffset;

                // --- Sample or Generate final color ---
                // Option 1: Sample a base texture (e.g., a water texture)
                // float4 baseColor = tex2D(_MainTex, finalUV_for_color);

                // Option 2: Use the base ocean color property
                float4 baseColor = _OceanColor;


                // Blend in Cyclone colors based on influence
                float4 finalColor = baseColor;

                // Blend Red based on total Red influence
                finalColor = lerp(finalColor, _CycloneColorRed, saturate(totalRedInfluence));

                // Blend Blue based on total Blue influence
                finalColor = lerp(finalColor, _CycloneColorBlue, saturate(totalBlueInfluence));

                // Add a subtle highlight/foam effect around cyclones based on combined influence
                float combinedInfluence = saturate(totalRedInfluence + totalBlueInfluence);
                float highlight = pow(combinedInfluence, 2.0) * 0.7; // Use power for softer falloff, adjust multiplier
                finalColor.rgb += highlight * 0.6; // Add a white-ish highlight


                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}