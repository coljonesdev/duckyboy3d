﻿    Shader "Custom/Back Side" {
        Properties {
            _MainTex ("Base (RGB)", 2D) = "white" {}
        }
        SubShader {
            Tags { "RenderType"="Opaque" }
            Cull Front
            LOD 200
           
            CGPROGRAM
            #pragma surface surf Lambert vertex:vert
     
            sampler2D _MainTex;
     
            struct Input {
                float2 uv_MainTex;
            };
     
            void vert (inout appdata_full v, out Input o) {
                UNITY_INITIALIZE_OUTPUT(Input, o);
                v.normal = -v.normal;
            }
     
            void surf(Input IN, inout SurfaceOutput o) {
                half4 c = tex2D(_MainTex, IN.uv_MainTex);
                o.Albedo = c.rgb;
                o.Alpha = c.a;
            }
            ENDCG
        }
        FallBack "Diffuse"
    }