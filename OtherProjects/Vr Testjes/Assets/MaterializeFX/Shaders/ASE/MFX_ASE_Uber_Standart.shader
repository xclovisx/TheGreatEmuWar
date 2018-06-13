// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/MFX/ASE/Uber/Standart"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HDR]_Color("Albedo Color", Color) = (1,1,1,1)
		_MainTex("Albedo", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Glossiness("Smoothness", Range( 0 , 1)) = 0
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_MetallicGlossMap("Metallic Texture", 2D) = "white" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Occlusion", Range( 0 , 1)) = 0
		[HDR]_EmissionColor("Emission Color", Color) = (1,1,1,1)
		_EmissionMap("Emission Texture", 2D) = "white" {}
		[HDR]_Color2("Albedo Color 2", Color) = (1,1,1,1)
		_MainTex2("Albedo 2", 2D) = "white" {}
		_BumpMap2("Normal Map 2", 2D) = "bump" {}
		[HDR]_EmissionColor2("Emission Color 2", Color) = (1,1,1,1)
		_EmissionMap2("Emission Texture 2", 2D) = "white" {}
		_EmissionMap2_Scroll("Emission Texture 2 Scroll", Vector) = (0,0,0,0)
		_Emission2Size("Emission 2 Size", Range( 0 , 1)) = 3.008756
		[HDR]_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeMap("Edge Map", 2D) = "white" {}
		_EdgeMap_Scroll("Edge Map Scroll", Vector) = (0,0,0,0)
		_EdgeSize("Edge Size", Range( 0 , 1)) = 0
		_EdgeStrength("Edge Strength", Range( 0 , 1)) = 0.1
		_DissolveMap1("Dissolve Map", 2D) = "white" {}
		_DissolveMap1_Scroll("Dissolve Map Scroll", Vector) = (0,0,0,0)
		_DissolveSize("Dissolve Size", Range( 0 , 3)) = 0
		[HDR]_DissolveEdgeColor("Dissolve Edge Color", Color) = (0,0,0,0)
		_DissolveEdgeSize("Dissolve Edge Size", Range( 0 , 1)) = 0.1
		_MaskOffset("Mask Offset", Range( -5 , 5)) = -1.253842
		_Axis("Axis", Vector) = (1,0,0,0)
		[Toggle]_PositionType("Position Type", Float) = 0
		_WorldPos("World Pos", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows noshadow 
		struct Input
		{
			float2 uv2_texcoord2;
			float3 worldPos;
			float2 uv_texcoord;
		};

		uniform sampler2D _BumpMap;
		uniform float4 _BumpMap_ST;
		uniform sampler2D _BumpMap2;
		uniform float4 _BumpMap2_ST;
		uniform float _EdgeSize;
		uniform float _PositionType;
		uniform float3 _Axis;
		uniform float3 _WorldPos;
		uniform float _MaskOffset;
		uniform sampler2D _EdgeMap;
		uniform float2 _EdgeMap_Scroll;
		uniform float4 _EdgeMap_ST;
		uniform float4 _Color;
		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float4 _Color2;
		uniform sampler2D _MainTex2;
		uniform float4 _MainTex2_ST;
		uniform float _DissolveSize;
		uniform sampler2D _DissolveMap1;
		uniform float2 _DissolveMap1_Scroll;
		uniform float4 _DissolveMap1_ST;
		uniform float _DissolveEdgeSize;
		uniform float4 _DissolveEdgeColor;
		uniform float4 _EmissionColor;
		uniform sampler2D _EmissionMap;
		uniform float4 _EmissionMap_ST;
		uniform float4 _EmissionColor2;
		uniform sampler2D _EmissionMap2;
		uniform float2 _EmissionMap2_Scroll;
		uniform float4 _EmissionMap2_ST;
		uniform float _Emission2Size;
		uniform float _EdgeStrength;
		uniform float4 _EdgeColor;
		uniform sampler2D _MetallicGlossMap;
		uniform float4 _MetallicGlossMap_ST;
		uniform float _Metallic;
		uniform float _Glossiness;
		uniform sampler2D _OcclusionMap;
		uniform float4 _OcclusionMap_ST;
		uniform float _OcclusionStrength;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv2_BumpMap = i.uv2_texcoord2 * _BumpMap_ST.xy + _BumpMap_ST.zw;
			float2 uv2_BumpMap2 = i.uv2_texcoord2 * _BumpMap2_ST.xy + _BumpMap2_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float local_direction173 = ( ( _Axis.x * ase_vertex3Pos.x ) + ( _Axis.y * ase_vertex3Pos.y ) + ( _Axis.z * ase_vertex3Pos.z ) );
			float3 ase_worldPos = i.worldPos;
			float world_position175 = length( ( _WorldPos - ase_worldPos ) );
			float pos181 = lerp(local_direction173,world_position175,_PositionType);
			float mask_pos206 = ( pos181 - _MaskOffset );
			float2 uv_EdgeMap = i.uv_texcoord * _EdgeMap_ST.xy + _EdgeMap_ST.zw;
			float2 panner112 = ( uv_EdgeMap + 1.0 * _Time.y * _EdgeMap_Scroll);
			float2 uv_TexCoord111 = i.uv_texcoord * float2( 1,1 ) + panner112;
			float edge_pos109 = ( mask_pos206 - ( _MaskOffset - tex2D( _EdgeMap, uv_TexCoord111 ).r ) );
			float temp_output_22_0 = ( (50.0 + (_EdgeSize - 0.0) * (0.0 - 50.0) / (1.0 - 0.0)) * edge_pos109 );
			float clampResult38 = clamp( temp_output_22_0 , 0.0 , 1.0 );
			float clampResult34 = clamp( ( 1.0 - abs( temp_output_22_0 ) ) , 0.0 , 1.0 );
			float edge_threshold42 = ( ( 1.0 - clampResult38 ) - clampResult34 );
			float3 lerpResult134 = lerp( UnpackNormal( tex2D( _BumpMap, uv2_BumpMap ) ) , UnpackNormal( tex2D( _BumpMap2, uv2_BumpMap2 ) ) , edge_threshold42);
			float3 normal136 = lerpResult134;
			o.Normal = normal136;
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 uv_MainTex2 = i.uv_texcoord * _MainTex2_ST.xy + _MainTex2_ST.zw;
			float4 lerpResult33 = lerp( ( _Color * tex2D( _MainTex, uv_MainTex ) ) , ( _Color2 * tex2D( _MainTex2, uv_MainTex2 ) ) , edge_threshold42);
			float4 albedo51 = lerpResult33;
			o.Albedo = albedo51.rgb;
			float2 uv_DissolveMap1 = i.uv_texcoord * _DissolveMap1_ST.xy + _DissolveMap1_ST.zw;
			float2 panner91 = ( uv_DissolveMap1 + 1.0 * _Time.y * _DissolveMap1_Scroll);
			float2 uv_TexCoord90 = i.uv_texcoord * float2( 1,1 ) + panner91;
			float alpha48 = ( _DissolveSize + ( mask_pos206 - ( _MaskOffset - tex2D( _DissolveMap1, uv_TexCoord90 ).r ) ) );
			float2 uv_EmissionMap = i.uv_texcoord * _EmissionMap_ST.xy + _EmissionMap_ST.zw;
			float4 base_emission147 = ( _EmissionColor * tex2D( _EmissionMap, uv_EmissionMap ) );
			float2 uv_EmissionMap2 = i.uv_texcoord * _EmissionMap2_ST.xy + _EmissionMap2_ST.zw;
			float2 panner198 = ( uv_EmissionMap2 + 1.0 * _Time.y * _EmissionMap2_Scroll);
			float2 uv_TexCoord199 = i.uv_texcoord * float2( 1,1 ) + panner198;
			float clampResult74 = clamp( ( ( ( 1.0 - tex2D( _EmissionMap2, uv_TexCoord199 ).r ) - 0.5 ) * 3.0 ) , 0.0 , 1.0 );
			float offset303 = _MaskOffset;
			float4 Emission2142 = ( _EmissionColor2 * ( pow( clampResult74 , 3.0 ) * saturate( ( ( mask_pos206 - offset303 ) + (0.0 + (_Emission2Size - 0.0) * (3.0 - 0.0) / (1.0 - 0.0)) ) ) ) );
			float4 lerpResult62 = lerp( base_emission147 , Emission2142 , edge_threshold42);
			float4 emission64 = lerpResult62;
			float smoothstepResult281 = smoothstep( ( 1.0 - _EdgeSize ) , 1.0 , clampResult34);
			float edge72 = smoothstepResult281;
			float4 final_emission67 = (( alpha48 <= _DissolveEdgeSize ) ? _DissolveEdgeColor :  ( emission64 + (( (1.0 + (_EdgeStrength - 0.0) * (0.1 - 1.0) / (1.0 - 0.0)) <= edge72 ) ? _EdgeColor :  ( _EdgeColor * edge72 ) ) ) );
			o.Emission = final_emission67.rgb;
			float2 uv_MetallicGlossMap = i.uv_texcoord * _MetallicGlossMap_ST.xy + _MetallicGlossMap_ST.zw;
			float4 tex2DNode117 = tex2D( _MetallicGlossMap, uv_MetallicGlossMap );
			float Metallic122 = ( tex2DNode117.r * _Metallic );
			o.Metallic = Metallic122;
			float Smothness121 = ( tex2DNode117.a * _Glossiness );
			o.Smoothness = Smothness121;
			float2 uv_OcclusionMap = i.uv_texcoord * _OcclusionMap_ST.xy + _OcclusionMap_ST.zw;
			float lerpResult128 = lerp( 1.0 , tex2D( _OcclusionMap, uv_OcclusionMap ).r , _OcclusionStrength);
			float occlusion129 = lerpResult128;
			o.Occlusion = occlusion129;
			o.Alpha = 1;
			clip( alpha48 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
22;525;1906;519;1070.194;-1951.017;1.533927;True;False
Node;AmplifyShaderEditor.CommentaryNode;183;-423.7378,2827.129;Float;False;1603.808;768.7366;;15;181;180;173;175;171;167;170;166;177;169;168;176;174;179;216;Pos;0.3123919,0.3585208,0.3970588,1;0;0
Node;AmplifyShaderEditor.Vector3Node;174;-391.5699,2900.668;Float;False;Property;_Axis;Axis;29;0;Create;True;1,0,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;168;-156.1713,3043.024;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;176;-295.2139,3254.524;Float;False;Property;_WorldPos;World Pos;31;0;Create;True;0,0,0;2,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;169;-209.9588,2902.661;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.WorldPosInputsNode;179;-307.109,3418.417;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;167;120.6452,2903.742;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;120.8962,3107.681;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;177;-4.818245,3344.156;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;119.6452,3008.429;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;216;180.4497,3343.867;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;141;-428.8928,1487.719;Float;False;2117.487;1266.031;;24;109;107;207;106;206;48;46;8;10;7;5;90;91;96;92;204;104;182;111;6;112;113;114;303;Dissolve & Edge Ramp;0,0.5034485,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;323.5943,2905.431;Float;True;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;541.9683,3009.987;Float;False;local_direction;-1;True;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;114;-374.6469,2578.943;Float;False;Property;_EdgeMap_Scroll;Edge Map Scroll;20;0;Create;False;0,0;0,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;113;-366.2211,2451.937;Float;False;0;104;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;145;-970.837,-838.501;Float;False;3019.917;1248.701;;21;239;79;142;60;252;56;78;74;77;76;75;57;199;198;196;197;297;301;305;304;283;Emission 2;0,0.08965492,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;175;401.0738,3337.532;Float;False;world_position;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;197;-943.1509,-635.3818;Float;False;0;57;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;196;-932.1509,-472.3817;Float;False;Property;_EmissionMap2_Scroll;Emission Texture 2 Scroll;16;0;Create;False;0,0;0,-0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;112;-121.993,2512.987;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ToggleSwitchNode;180;746.1513,3263.352;Float;False;Property;_PositionType;Position Type;30;0;Create;True;0;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;35.1797,1979.412;Float;False;Property;_MaskOffset;Mask Offset;28;0;Create;True;-1.253842;0.82;-5;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;182;-11.59232,1555.031;Float;True;181;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;111;137.9767,2416.424;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;198;-628.1505,-536.3818;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;966.1733,3263.43;Float;False;pos;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;104;430.0266,2302.004;Float;True;Property;_EdgeMap;Edge Map;19;0;Create;True;7a632f967e8ad42f5bd275898151ab6a;5f4153e8858407a4997b2b5b3cf502fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;204;327.484,1560.018;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;199;-452.0786,-560.2536;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;759.2487,2255.553;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;57;-670.9076,-770.4292;Float;True;Property;_EmissionMap2;Emission Texture 2;15;0;Create;False;72e8a18a778af834a815921e55b0b5e7;7af3ece29374c234f9406a3bb35df76c;True;0;False;white;Auto;False;Object;-1;Auto;ProceduralTexture;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;140;-437.5188,515.5353;Float;False;1921.035;901.1141;;14;72;41;34;71;24;38;23;22;110;25;19;42;281;282;Edge;0,0.213793,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;207;765.0082,2110.022;Float;False;206;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;206;566.8958,1556.455;Float;False;mask_pos;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;75;-380.5573,-743.0378;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;-379.9276,765.2415;Float;False;Property;_EdgeSize;Edge Size;21;0;Create;True;0;0.851;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;107;994.4472,2197.942;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;109;1270.668,2193.064;Float;True;edge_pos;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;239;-503.3015,-346.6836;Float;True;206;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;76;-225.6996,-742.9838;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-513.4498,82.7339;Float;True;Property;_Emission2Size;Emission 2 Size;17;0;Create;True;3.008756;0.263;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;-502.8893,-145.7305;Float;True;303;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;303;378.8377,1842.513;Float;False;offset;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-81.74005,769.9133;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;50.0;False;4;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;110;-105.1539,967.375;Float;True;109;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-70.61085,-743.0193;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;304;-275.8893,-217.7305;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;283;-134.5966,87.75113;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;3.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;138.5464,828.3665;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;96;-401.3016,2110.514;Float;False;0;5;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;92;-388.6284,2244.166;Float;False;Property;_DissolveMap1_Scroll;Dissolve Map Scroll;24;0;Create;False;0,0;0,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.AbsOpNode;23;386.1413,827.3668;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;74;89.20213,-743.6144;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;297;116.3568,68.21609;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;78;333.0464,-745.1406;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;301;389.6049,68.76625;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;144;-2367.079,2454.067;Float;False;1215.546;888.8182;;9;64;62;143;147;63;148;59;55;58;Emission;0,0,0,1;0;0
Node;AmplifyShaderEditor.ClampOpNode;38;478.4857,581.9752;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;24;572.4553,826.874;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;91;-109.6854,2178.051;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;58;-2330.748,2694.311;Float;True;Property;_EmissionMap;Emission Texture;10;0;Create;False;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;282;585.6412,1159.689;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;55;-2243.104,2510.947;Float;False;Property;_EmissionColor;Emission Color;9;1;[HDR];Create;False;1,1,1,1;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;90;75.08936,2126.214;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;71;746.7791,583.2764;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;56;621.2866,-717.6028;Float;False;Property;_EmissionColor2;Emission Color 2;14;1;[HDR];Create;False;1,1,1,1;10,2.265721,0.9558833,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;681.2194,-403.0577;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;34;776.375,827.1055;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;41;1076.77,681.533;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;421.9146,2041.334;Float;True;Property;_DissolveMap1;Dissolve Map;23;0;Create;False;357928dd8c8088440b4662373bd09d7a;7af3ece29374c234f9406a3bb35df76c;True;0;False;white;Auto;False;Object;-1;Auto;ProceduralTexture;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2027.555,2638.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;895.6096,-603.8718;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;138;-2921.519,1727.413;Float;False;1892.359;679.8414;;13;270;267;66;268;65;67;261;266;191;263;21;73;20;Edge Emission;0,0,0,1;0;0
Node;AmplifyShaderEditor.SmoothstepOpNode;281;1004.857,1142.609;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;142;1105.067,-606.756;Float;True;Emission2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-2190.285,3077.846;Float;True;42;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;147;-1847.064,2632.806;Float;False;base_emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;1239.524,677.5984;Float;True;edge_threshold;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;72;1263.493,1138.567;Float;True;edge;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;148;-2220.577,2927.869;Float;False;147;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;20;-2450.645,2106.615;Float;False;Property;_EdgeColor;Edge Color;18;1;[HDR];Create;True;1,1,1,1;1.014199,4,0.6176476,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;7;752.5357,1989.934;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;-2854.303,2161.33;Float;True;72;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;268;-2759.505,1976.508;Float;False;Property;_EdgeStrength;Edge Strength;22;0;Create;True;0.1;0.264;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-2190.527,3001.836;Float;False;142;0;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;62;-1942.219,2933.19;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;270;-2405.99,1886.316;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;1.0;False;4;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;8;932.5671,1795.862;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;824.5463,1668.213;Float;False;Property;_DissolveSize;Dissolve Size;25;0;Create;True;0;1.13;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-2175.788,2229.598;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;139;-2363.308,924.239;Float;False;1157.36;791.994;Diffuse;9;54;51;33;32;29;27;28;30;31;;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;131;-2366.09,401.7381;Float;False;752.2058;441.1062;Normal;5;136;135;134;133;132;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;1163.299,1672.349;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareLowerEqual;267;-1999.937,1868.927;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;65;-2894.496,1769.581;Float;True;64;0;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;125;-2364.525,-100.9189;Float;False;775.5983;398.703;Ambient Occlusion;4;129;128;127;126;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SamplerNode;31;-2198.205,1507.454;Float;True;Property;_MainTex2;Albedo 2;12;0;Create;False;None;7af3ece29374c234f9406a3bb35df76c;True;0;False;white;Auto;False;Object;-1;Auto;ProceduralTexture;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-2110.562,1326.09;Float;False;Property;_Color2;Albedo Color 2;11;1;[HDR];Create;False;1,1,1,1;1.696588,1.696588,1.696588,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;27;-2233.972,965.9473;Float;False;Property;_Color;Albedo Color;1;1;[HDR];Create;False;1,1,1,1;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;115;-2360.427,-636.3569;Float;False;803.3513;419.69;Metallic & Smoothness;7;122;121;120;119;118;117;116;;0,0,0,1;0;0
Node;AmplifyShaderEditor.SamplerNode;28;-2315.616,1132.712;Float;True;Property;_MainTex;Albedo;2;0;Create;False;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;-1732.511,2929.326;Float;True;emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;263;-1884.084,2050.768;Float;False;48;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;266;-1833.756,2228.011;Float;False;Property;_DissolveEdgeColor;Dissolve Edge Color;26;1;[HDR];Create;True;0,0,0,0;0,2.517242,5,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;1404.641,1544.734;Float;True;alpha;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-2018.423,1093.392;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-1731.499,1485.125;Float;False;42;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;127;-2325.607,159.1787;Float;False;Property;_OcclusionStrength;Occlusion;8;0;Create;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;135;-2041.038,725.6877;Float;False;42;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;66;-1830.751,1784.17;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-1976.316,2150.18;Float;False;Property;_DissolveEdgeSize;Dissolve Edge Size;27;0;Create;True;0.1;0.615;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;117;-2335.049,-591.3156;Float;True;Property;_MetallicGlossMap;Metallic Texture;6;0;Create;False;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;133;-2341.837,643.4477;Float;True;Property;_BumpMap2;Normal Map 2;13;0;Create;False;None;ca02b40705c7a9a41bd0355840d6aa1b;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1895.012,1451.535;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-2332.794,-319.4345;Float;False;Property;_Glossiness;Smoothness;4;0;Create;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;126;-2325.607,-48.821;Float;True;Property;_OcclusionMap;Occlusion;7;0;Create;False;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;132;-2337.39,455.3405;Float;True;Property;_BumpMap;Normal Map;3;0;Create;False;None;None;True;1;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;116;-2331.299,-395.7352;Float;False;Property;_Metallic;Metallic;5;0;Create;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-1984.865,-507.0139;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-1731.833,1284.201;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareLowerEqual;261;-1577.201,1999.727;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;COLOR;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-1977.549,-376.1849;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;128;-1986.477,27.58154;Float;False;3;0;FLOAT;1.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;134;-1992.136,572.4476;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;1900.775,814.4269;Float;False;67;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-1301.85,1996.763;Float;False;final_emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;124;1901.03,958.9058;Float;False;121;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;121;-1819.582,-379.2387;Float;False;Smothness;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;52;1924.548,603.3089;Float;False;51;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;136;-1822.036,567.6877;Float;False;normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;1916.192,1115.315;Float;False;48;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;129;-1802.378,24.15844;Float;False;occlusion;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1439.285,1225.089;Float;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;122;-1823.127,-511.2867;Float;False;Metallic;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;137;1908.081,692.4705;Float;False;136;0;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;123;1917.03,884.9058;Float;False;122;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;1901.373,1032.003;Float;False;129;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2160.694,767.8633;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;QFX/MFX/ASE/Uber/Standart;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;False;0;Custom;0.5;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;169;0;174;0
WireConnection;167;0;169;0
WireConnection;167;1;168;1
WireConnection;170;0;169;2
WireConnection;170;1;168;3
WireConnection;177;0;176;0
WireConnection;177;1;179;0
WireConnection;166;0;169;1
WireConnection;166;1;168;2
WireConnection;216;0;177;0
WireConnection;171;0;167;0
WireConnection;171;1;166;0
WireConnection;171;2;170;0
WireConnection;173;0;171;0
WireConnection;175;0;216;0
WireConnection;112;0;113;0
WireConnection;112;2;114;0
WireConnection;180;0;173;0
WireConnection;180;1;175;0
WireConnection;111;1;112;0
WireConnection;198;0;197;0
WireConnection;198;2;196;0
WireConnection;181;0;180;0
WireConnection;104;1;111;0
WireConnection;204;0;182;0
WireConnection;204;1;6;0
WireConnection;199;1;198;0
WireConnection;106;0;6;0
WireConnection;106;1;104;1
WireConnection;57;1;199;0
WireConnection;206;0;204;0
WireConnection;75;0;57;1
WireConnection;107;0;207;0
WireConnection;107;1;106;0
WireConnection;109;0;107;0
WireConnection;76;0;75;0
WireConnection;303;0;6;0
WireConnection;25;0;19;0
WireConnection;77;0;76;0
WireConnection;304;0;239;0
WireConnection;304;1;305;0
WireConnection;283;0;79;0
WireConnection;22;0;25;0
WireConnection;22;1;110;0
WireConnection;23;0;22;0
WireConnection;74;0;77;0
WireConnection;297;0;304;0
WireConnection;297;1;283;0
WireConnection;78;0;74;0
WireConnection;301;0;297;0
WireConnection;38;0;22;0
WireConnection;24;0;23;0
WireConnection;91;0;96;0
WireConnection;91;2;92;0
WireConnection;282;0;19;0
WireConnection;90;1;91;0
WireConnection;71;0;38;0
WireConnection;252;0;78;0
WireConnection;252;1;301;0
WireConnection;34;0;24;0
WireConnection;41;0;71;0
WireConnection;41;1;34;0
WireConnection;5;1;90;0
WireConnection;59;0;55;0
WireConnection;59;1;58;0
WireConnection;60;0;56;0
WireConnection;60;1;252;0
WireConnection;281;0;34;0
WireConnection;281;1;282;0
WireConnection;142;0;60;0
WireConnection;147;0;59;0
WireConnection;42;0;41;0
WireConnection;72;0;281;0
WireConnection;7;0;6;0
WireConnection;7;1;5;1
WireConnection;62;0;148;0
WireConnection;62;1;143;0
WireConnection;62;2;63;0
WireConnection;270;0;268;0
WireConnection;8;0;206;0
WireConnection;8;1;7;0
WireConnection;21;0;20;0
WireConnection;21;1;73;0
WireConnection;46;0;10;0
WireConnection;46;1;8;0
WireConnection;267;0;270;0
WireConnection;267;1;73;0
WireConnection;267;2;20;0
WireConnection;267;3;21;0
WireConnection;64;0;62;0
WireConnection;48;0;46;0
WireConnection;29;0;27;0
WireConnection;29;1;28;0
WireConnection;66;0;65;0
WireConnection;66;1;267;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;120;0;117;1
WireConnection;120;1;116;0
WireConnection;33;0;29;0
WireConnection;33;1;32;0
WireConnection;33;2;54;0
WireConnection;261;0;263;0
WireConnection;261;1;191;0
WireConnection;261;2;266;0
WireConnection;261;3;66;0
WireConnection;119;0;117;4
WireConnection;119;1;118;0
WireConnection;128;1;126;1
WireConnection;128;2;127;0
WireConnection;134;0;132;0
WireConnection;134;1;133;0
WireConnection;134;2;135;0
WireConnection;67;0;261;0
WireConnection;121;0;119;0
WireConnection;136;0;134;0
WireConnection;129;0;128;0
WireConnection;51;0;33;0
WireConnection;122;0;120;0
WireConnection;0;0;52;0
WireConnection;0;1;137;0
WireConnection;0;2;68;0
WireConnection;0;3;123;0
WireConnection;0;4;124;0
WireConnection;0;5;130;0
WireConnection;0;10;49;0
ASEEND*/
//CHKSM=E9C122A7BA462D121BE8ED02DA3EE8A13D6B8617