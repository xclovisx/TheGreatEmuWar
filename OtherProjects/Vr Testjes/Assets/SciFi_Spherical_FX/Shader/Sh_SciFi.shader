// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Shader_SciFi/SciFi"
{
	Properties
	{
		_FrontFace_Diffuse_map("Front Face Diffuse Map", 2D) = "white" {}
		_FrontFace_Color ("FrontFace Color Mult :", Color) = (1,1,1,1)
		_FrontFace_Intensity("Intensity Mult :", range (0,4.00)) = 1.00
		_BackFace_Diffuse_map("Back  Map", 2D) = "black" {}
		_BackFace_Color ("BackFace Color Mult :", Color) = (1,1,1,1)
		_BackFace_Intensity("Intensity Mult :", range (0,4.00)) = 1.00
		///////
		_OutlineTex("Outline Map", 2D) = "black" {}
		_Outline_Color ("Outline Color Mult :", Color) = (1,1,1,1)
		_Outline_Opacity("Outline Opacity :", range (0,100.00)) = 50
		///////
		_NormalPush("Normal Push :", range (-1.00,1.00)) = 1.00
		//_ColorBoost("Color Boost", range (1,25.00)) = 1.00
		///////
		_DisplacementMask("Displacement Mask", 2D) = "white" {}
		_Shrink_Faces_Amplitude("Shrink Face Factor :", range (-2.00,2.00)) = 1
		_Animation_speed("Animation Speed :", Float) = 0
		_deformation_type_Factor("Transition Factor :", range (0,1.00)) = 1
		[Toggle] _deformation_type("Stretching", Float) = 0

	}

	Subshader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Zwrite on
			ZTest on
			cull off
			CGPROGRAM
			//http://docs.unity3d.com/Manual/SL-ShaderPrograms.html
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"

			// VARIABLES ///////////////////////////////////////////////////////////////////////////////////////////
			uniform half4 _Outline_Color;
			uniform half4 _FrontFace_Color;
			uniform half4 _BackFace_Color;
			uniform sampler2D _FrontFace_Diffuse_map;
			uniform sampler2D _BackFace_Diffuse_map;
			uniform sampler2D _OutlineTex;
			uniform sampler2D _DisplacementMask;
			uniform float4 _DisplacementMask_ST;
			uniform float4 _FrontFace_Diffuse_map_ST;
			uniform float4 _BackFace_Diffuse_map_ST;
			uniform float _Shrink_Faces_Amplitude;
			uniform float _deformation_type_Factor;
			uniform float _deformation_type;
			uniform float _NormalPush;
			uniform float _Outline_Opacity;
			//uniform float _ColorBoost;
			uniform float _FrontFace_Intensity;
			uniform float _BackFace_Intensity;
			uniform float _Animation_speed;


			////////////////////////////////////////////////////////////////////////////////////////////////////////
			// STRUCTURS  //////////////////////////////////////////////////////////////////////////////////////////
			struct vertexInput
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 outliner : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float4 normal : NORMAL;

			};

			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 outliner : TEXCOORD2;
				float4 texcoord3 : TEXCOORD3;
				float3 uv_matcap : TEXCOORD4;
				float3 normalDir : TEXCOORD6;
				float4 color_out : TEXCOORD7;
				float4 normal : NORMAL;



			};
			///////////////////////////////////////////////////////////////////////////////////////////////////////

			// FUNCTIONS //////////////////////////////////////////////////////////////////////////////////////////

			float3 rejection(float3 va, float3 n)
			{
				// float3 n_vb = normalize(vb);
				float3 rejection = va - ( ((dot(va, n))/(dot(n,n)))*n );
				return rejection;
			}

			float3 remap(float2 v,float min1,float min2,float max1,float max2)
			{
				return (v.x - min1) / (min2 - min1) * (max2-max1) + max1;
			}
			/////////////////////////////////////////////////////////////////////////////////////////////////////////

			// VERTEX OPERATIONS ////////////////////////////////////////////////////////////////////////////////////
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;

				//TIME
				float T = _Time *4 ; //
				float temp = smoothstep(0,1,sin((_Time*60))) ;

				// stretch or not
				float4 coord = (_deformation_type == 1 ? v.texcoord: v.texcoord1);

				// Create transition Coord
				float2 transition = float2(1,step(coord.x,_deformation_type_Factor));

				// Get Intensity from Texture
				float4 TexInt = tex2Dlod (_DisplacementMask, float4((coord.xy * (_DisplacementMask_ST.xy  ) + (_DisplacementMask_ST.zw * _Animation_speed * T)),0,0)) ;
				o.color_out = TexInt;

				// Shrink
				float3 _rej = rejection(v.vertex.xyz,v.normal.xyz) * (-1) ;

				v.vertex += v.normal * (transition.y )* (TexInt.x)* (_NormalPush/100);
				v.vertex.xyz += _rej *(_Shrink_Faces_Amplitude)*(TexInt.x) * transition.y ;


				// PROJECTIONS

				o.pos = UnityObjectToClipPos( v.vertex);

				float3 normalVS = mul(UNITY_MATRIX_MV,v.normal);
				normalVS = normalize(normalVS);

				o.normalDir = UnityObjectToWorldNormal(v.normal);
				o.uv_matcap.xy = normalVS.xy * 0.5 + float2(0.5,0.5);



				o.outliner.xy = v.outliner.xy;
				o.texcoord.xy = v.texcoord.xy;
				o.texcoord1.xy = v.texcoord1.xy;


				return o;
			}

			////////////////////////////////////////////////////////////////////////////////////////////////////////


			// FRAGMENTS OPERATIONS ///////////////////////////////////////////////////////////////////////////////
			half4 frag(vertexOutput i , float facing : VFACE) : COLOR
			{

				float4 ff_col = tex2D(_FrontFace_Diffuse_map ,i.uv_matcap)   ;

				float temp = i.color_out.x  ;

				float4 outliner_col = tex2D(_OutlineTex ,i.outliner)* temp *(_Outline_Opacity) * _Outline_Color ;

				float4 bf_col = tex2D(_BackFace_Diffuse_map,i.uv_matcap);


				//COLORBOOST
				outliner_col.rgb *= dot(ff_col.rgb,(0.5,0.5,0.5)); //* _ColorBoost
				ff_col.rgb *= _FrontFace_Intensity * _FrontFace_Color;
				bf_col.rgb *= _BackFace_Intensity * _BackFace_Color;

				// Add Outline To the Color
				ff_col = ff_col + (outliner_col);

				return (facing >= 0 ? ff_col: bf_col) ; ;//float4(1,0.25,0.1,1)* i.normalView;
			}
			///////////////////////////////////////////////////////////////////////////////////////////////////////

			ENDCG
		}
	}
}
