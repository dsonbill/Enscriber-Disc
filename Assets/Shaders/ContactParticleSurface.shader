Shader "Custom/ContactParticleSurface" {
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
        _WarpingVectors ("Warping Vectors", Vector) = (0,0,0,0) 
        _NumWarpingVectors ("Number of Warping Vectors", Int) = 0
        _ParticleDelta ("Delta", Vector) = (0,0,0)
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        // Base Pass
        Pass {
            CGPROGRAM
            #pragma surface surf Lambert

            // Properties
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
            float4 _WarpingVectors[100]; 
            int _NumWarpingVectors;

            struct Input {
                float2 uv_StriationTexture;
                float3 worldPos;
            };

            void surf (Input IN, inout SurfaceOutput o) {
                // 1. Basic Depth and Energy Color
                fixed4 col = lerp(_MainColor, _DepthColor, _ParticleDepth);
                float energy = length(_ParticleAscription.xyz);
                col.rgb *= 1.0 + energy;

                // 2. Striation Visualization
                float2 st = IN.worldPos.xz * _StriationScale;
                fixed4 striationCol = tex2D(_StriationTexture, st);
                col.rgb += striationCol.rgb * 0.5;

                // 3. Reach Effect
                float reachFactor = saturate(_ParticleReach * _ReachIntensity);
                col.rgb = lerp(col.rgb, _ReachColor.rgb, reachFactor);

                // 4. Geometric Hints (Example)
                float shapeFactor = sin(IN.worldPos.y * 10.0 + _Time.y * 5.0) * 0.5 + 0.5;
                col.rgb *= shapeFactor;

                // 5. Spacetime Warping (Applied to worldPos in Input struct)
                float3 warpedPosition = IN.worldPos;
                for (int i = 0; i < _NumWarpingVectors; i++) {
                    float3 warpVectorPos = _WarpingVectors[i].xyz;
                    float warpMagnitude = _WarpingVectors[i].w;
                    float distanceToWarp = distance(IN.worldPos, warpVectorPos);

                    // Apply warping function (customize this!)
                    float warpAmount = warpMagnitude / (distanceToWarp * distanceToWarp + 0.1);
                    warpedPosition += warpAmount * (IN.worldPos - warpVectorPos);
                }
                
                IN.worldPos = warpedPosition; // Update worldPos in Input struct
                o.Albedo = col.rgb; 
            }
            ENDCG
        }

        // Forward Add Pass
        Pass {
            Tags { "LightMode" = "ForwardAdd" }
            Blend One One // Additive blending 

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            float3 _ParticleDelta;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1; 
            };

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                float3 forceDelta = _ParticleDelta;

                #ifdef UNITY_PASS_FORWARDBASE
                // Light loop
                for (int l = 1; l < UNITY_LIGHT_COUNT; l++) {
                    Light light = GetUnityLight(l);

                    float3 lightDir = light.dir;
                    float distToLight = length(lightDir);
                    float attenuation = light.atten;

                    forceDelta *= attenuation / (distToLight * distToLight);
                }
                #endif

                // Apply forceDelta to output color, simulating position shift
                float4 outputColor = tex2D(_MainTex, i.uv + forceDelta.xy); // Sample texture with offset
                return outputColor; 
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}