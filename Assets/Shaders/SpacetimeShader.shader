Shader "Custom/SpacetimeShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _WarpingVectors ("Warping Vectors", Vector) = (0,0,0,0) 
        _NumWarpingVectors ("Number of Warping Vectors", Int) = 0
        _WarpIntensity ("Warp Intensity", Float) = 1.0
        _WarpScale ("Warp Scale", Float) = 1.0
        _WarpFalloff ("Warp Falloff", Float) = 1.0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _WarpingVectors[100]; // Assuming max 100 warping vectors
            int _NumWarpingVectors;
            float _WarpIntensity;
            float _WarpScale;
            float _WarpFalloff;

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 1. Sample Main Texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // 2. Calculate Warped UV
                float2 warpedUV = i.uv; // Start with original UV
                for (int j = 0; j < _NumWarpingVectors; j++)
                {
                    float3 warpVectorPos = _WarpingVectors[j].xyz;
                    float warpMagnitude = _WarpingVectors[j].w * _WarpIntensity; // Apply warp intensity

                    // Calculate distance to warping vector
                    float distanceToWarp = distance(i.worldPos, warpVectorPos);

                    // Calculate warp amount based on distance and falloff
                    float warpAmount = warpMagnitude / (distanceToWarp * distanceToWarp + _WarpFalloff); // Custom falloff

                    // Calculate warp direction (you might need to adjust this based on Contact Theory)
                    float2 warpDirection = normalize(i.uv - float2(warpVectorPos.x, warpVectorPos.z)) * _WarpScale; 

                    // Apply warping to UV
                    warpedUV += warpAmount * warpDirection;
                }

                // 3. Sample Texture with Warped UV
                col = tex2D(_MainTex, warpedUV); 

                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}