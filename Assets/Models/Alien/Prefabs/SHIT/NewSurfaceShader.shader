Shader "Unlit/SpecialFX/Shield"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "White" {}
		_Color("Color", Color) = (1,1,1,1)
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_Transparency("Transparency", Range(0.0,0.5)) = 0.25
		_CutoutThresh("Cutout Threshold", Range(0.0,1.0)) = 0.2
		_Distance("Distance", Float) = 1
		_Amplitude("Amplitude", Float) = 1
		_Speed("Speed", Float) = 1
		_Amount("Amount", Range(0.0,1.0)) = 1
	}

		SubShader
		{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100

			Cull Front
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				//#pragma surface surf Standard fullforwardshadows

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _TintColor;
				float _Transparency;
				float _CutoutThresh;
				float _Distance;
				float _Amplitude;
				float _Speed;
				float _Amount;
				/*
				struct Input {
					float2 uv_MainTex; // we vragen om de uv mapping van MainText t.o.v. de vertex
				};

				void surf(Input IN, inout SurfaceOutputStandard o) {
					// we pakken de kleur uit de texture en vermenigvuldigen met de fixed floats
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

					// albedo is de diffuse (eigen) kleur
					o.Albedo = float3(0,1,1);
				}*/

				v2f vert( appdata v)
				{
					v2f o;
					v.vertex.x += sin(_Time.y * _Speed + v.vertex.y * _Amplitude) * _Distance * _Amount;
					//o.Albedo = float3(.5, .5, 1);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv) + _TintColor;
					col.a = _Transparency;
					clip(col.r - _CutoutThresh);
					return col;
				}
				ENDCG
			}
		}
}