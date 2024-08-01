Shader "Custom/ContactParticle" {
    Properties {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _DepthColor ("Depth Color", Color) = (0,0.5,1,1)
        _ParticleDepth ("Depth", Float) = 0.0
        _StriationTexture ("Striation Texture", 2D) = "white" {}
        _StriationScale ("Striation Scale", Float) = 10.0
        _ReachColor ("Reach Color", Color) = (1,0,1,1)
        _ReachIntensity ("Reach Intensity", Float) = 1.0
        _ParticleReach ("Reach", Float) = 0.0
        _ParticleAscription ("Ascription", Vector) = (0,0,0,0) 
        _WarpingVectors ("Warping Vectors", Vector) = (0,0,0,0) // Array for warping vectors
        _NumWarpingVectors ("Number of Warping Vectors", Int) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            fixed4 _DepthColor;
            float _ParticleDepth; 
            sampler2D _StriationTexture;
            float4 _StriationTexture_ST;
            float _StriationScale;
            fixed4 _ReachColor;
            float _ReachIntensity;
            float _ParticleReach; 
            float4 _ParticleAscription; 
            float4 _WarpingVectors[100]; // Assuming a maximum of 100 warping vectors
            int _NumWarpingVectors;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float depth : TEXCOORD2;
                float reach : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;

                // 1. Calculate Warped Position
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 warpedPosition = worldPos;
                for (int i = 0; i < _NumWarpingVectors; i++) {
                    float3 warpVectorPos = _WarpingVectors[i].xyz;
                    float warpMagnitude = _WarpingVectors[i].w;
                    float distanceToWarp = distance(worldPos, warpVectorPos);

                    // Apply warping function (customize this!)
                    float warpAmount = warpMagnitude / (distanceToWarp * distanceToWarp + 0.1); // Example
                    warpedPosition += warpAmount * (worldPos - warpVectorPos);
                }

                // 2. Transform to Clip Space
                o.pos = UnityObjectToClipPos(float4(warpedPosition, 1.0));

                // Pass other properties to fragment shader
                o.uv = TRANSFORM_TEX(v.uv, _StriationTexture);
                o.worldPos = worldPos;
                o.depth = _ParticleDepth;
                o.reach = _ParticleReach;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. Basic Depth and Energy Color 
                fixed4 col = lerp(_MainColor, _DepthColor, i.depth);
                float energy = length(_ParticleAscription.xyz); 
                col.rgb *= 1.0 + energy;

                // 2. Striation Visualization
                float2 st = i.worldPos.xz * _StriationScale;
                fixed4 striationCol = tex2D(_StriationTexture, st);
                col.rgb += striationCol.rgb * 0.5; 

                // 3. Reach Effect
                float reachFactor = saturate(i.reach * _ReachIntensity); 
                col.rgb = lerp(col.rgb, _ReachColor.rgb, reachFactor); 

                // 4. Geometric Hints (Example)
                float shapeFactor = sin(i.worldPos.y * 10.0 + _Time.y * 5.0) * 0.5 + 0.5;
                col.rgb *= shapeFactor;

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse" 
}