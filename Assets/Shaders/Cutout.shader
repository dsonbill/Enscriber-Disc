Shader "CUTOUTSHADER/Cutout" {
       Properties {
           _Brightness ("Brightness", Float) = 1
           _Color ("Main Color", Color) = (1,1,1,1)
           _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
           _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
           _FlipNormals("Flip Normals", Float) = 1
}
 
SubShader {
            Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
            LOD 200
         
            CGPROGRAM
            #pragma surface surf Lambert alphatest:_Cutoff vertex:vert
 
            #include "UnityCG.cginc"
 
            sampler2D _MainTex;
            fixed4 _Color;
            float _Brightness;
 
            struct Input {
                float2 uv_MainTex;
            };
 
            float _FlipNormals;
 
            void vert(inout appdata_full v) {
                v.normal.xyz = v.normal * _FlipNormals;
 
            }
 
            void surf (Input IN, inout SurfaceOutput o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                c.r = c.r * _Brightness;
                c.g = c.g * _Brightness;
                c.b = c.b * _Brightness;
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
         
            ENDCG
        }
 
Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}