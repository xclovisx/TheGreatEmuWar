﻿Shader "Custom/InsideOut" {
	Properties {
		_Color ("Color", Color) = (1,1,1,0.5)
		_MainTex ("Albedo (RGB)", 2D) = "White" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		Cull Front
		CGPROGRAM
		#pragma surface surf Standard vertex:vert

			void vert(inout appdata_full v) {
			v.normal.xyz = v.normal * -1;
		}

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}