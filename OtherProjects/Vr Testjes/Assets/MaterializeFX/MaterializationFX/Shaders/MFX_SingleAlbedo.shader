// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/MFX/MfxSingleAlbedo"
{
	Properties
	{
		_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		_Albedo("Albedo", 2D) = "white" {}
		_MetallicTexture("Metallic Texture", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[Normal]_NormalMap("Normal Map", 2D) = "bump" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionTexture("Emission Texture", 2D) = "white" {}
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_ScaleDissolveTex("Scale Dissolve Tex", Range( 0 , 20)) = 1
		_DissolveSpeed("Dissolve Speed", Vector) = (0.19,0,0,0)
		_GlowDistributionTexture("Glow Distribution Texture", 2D) = "white" {}
		_ScaleGlowTex("Scale Glow Tex", Range( 0 , 20)) = 1
		_GlowSpeed("Glow Speed", Vector) = (0.19,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0
		[HDR]_DissolveColor("Dissolve Color", Color) = (0,0,0,0)
		[HDR]_GlowDistributionColor("Glow Distribution Color", Color) = (0,0,0,0)
		_Dissolve("Dissolve", Range( 0 , 1)) = 0
		_DissolveDistance("Dissolve Distance", Range( 0 , 1)) = 0
		_GlowDistance("Glow Distance", Range( 0 , 1)) = 0.1
		_GlowDistribution("Glow Distribution", Range( 0 , 1)) = 0
		_Position("Position", Range( -3 , 3)) = 0.5
		[Toggle]_PositionType("PositionType", Float) = 0
		_Direction("Direction", Vector) = (0,0,0,0)
		[Toggle]_MirrorMode("Mirror Mode", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform fixed4 _AlbedoColor;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform fixed _MirrorMode;
		uniform fixed _Position;
		uniform fixed _PositionType;
		uniform fixed3 _Direction;
		uniform fixed _DissolveDistance;
		uniform sampler2D _DissolveTexture;
		uniform fixed2 _DissolveSpeed;
		uniform fixed _ScaleDissolveTex;
		uniform fixed _Dissolve;
		uniform fixed _GlowDistance;
		uniform fixed4 _DissolveColor;
		uniform sampler2D _EmissionTexture;
		uniform float4 _EmissionTexture_ST;
		uniform fixed4 _EmissionColor;
		uniform fixed _GlowDistribution;
		uniform fixed4 _GlowDistributionColor;
		uniform sampler2D _GlowDistributionTexture;
		uniform fixed2 _GlowSpeed;
		uniform fixed _ScaleGlowTex;
		uniform sampler2D _MetallicTexture;
		uniform float4 _MetallicTexture_ST;
		uniform fixed _Metallic;
		uniform fixed _Smoothness;
		uniform float _Cutoff = 0;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float4 normal294 = tex2D( _NormalMap, uv_NormalMap );
			o.Normal = normal294.rgb;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 albedo285 = ( _AlbedoColor * tex2D( _Albedo, uv_Albedo ) );
			o.Albedo = albedo285.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float local_direction306 = ( ( _Direction.x * ase_vertex3Pos.x ) + ( _Direction.y * ase_vertex3Pos.y ) + ( _Direction.z * ase_vertex3Pos.z ) );
			float3 ase_worldPos = i.worldPos;
			float world_position314 = length( ( _Direction - ase_worldPos ) );
			float temp_output_102_0 = ( _Position - lerp(local_direction306,world_position314,_PositionType) );
			float temp_output_112_0 = ( lerp(temp_output_102_0,length( temp_output_102_0 ),_MirrorMode) - _DissolveDistance );
			float2 panner15 = ( float2( 0,0 ) + _Time.y * _DissolveSpeed);
			float2 uv_TexCoord16 = i.uv_texcoord * float2( 1,1 ) + panner15;
			float temp_output_118_0 = ( temp_output_112_0 + ( _DissolveDistance * ( tex2D( _DissolveTexture, (uv_TexCoord16*_ScaleDissolveTex + float2( 0,0 )) ).r * ( 1.0 - _Dissolve ) ) ) );
			float2 uv_EmissionTexture = i.uv_texcoord * _EmissionTexture_ST.xy + _EmissionTexture_ST.zw;
			float4 base_emission283 = ( tex2D( _EmissionTexture, uv_EmissionTexture ) * _EmissionColor );
			float2 panner245 = ( float2( 0,0 ) + _Time.y * _GlowSpeed);
			float2 uv_TexCoord247 = i.uv_texcoord * float2( 1,1 ) + panner245;
			float4 emission296 = (( temp_output_118_0 <= _GlowDistance ) ? _DissolveColor :  ( ( base_emission283 + saturate( ( 1.0 - ( ( 1.0 - (-1.0 + (_GlowDistribution - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) + temp_output_112_0 ) ) ) ) * ( _GlowDistributionColor * tex2D( _GlowDistributionTexture, (uv_TexCoord247*_ScaleGlowTex + float2( 0,0 )) ) ) ) );
			o.Emission = emission296.rgb;
			float2 uv_MetallicTexture = i.uv_texcoord * _MetallicTexture_ST.xy + _MetallicTexture_ST.zw;
			fixed4 tex2DNode22 = tex2D( _MetallicTexture, uv_MetallicTexture );
			float metallic289 = ( tex2DNode22.r * _Metallic );
			o.Metallic = metallic289;
			float smothness290 = ( tex2DNode22.a * _Smoothness );
			o.Smoothness = smothness290;
			o.Alpha = 1;
			float opacity298 = temp_output_118_0;
			clip( opacity298 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
105;342;1610;568;4322.488;919.0112;4.744584;True;False
Node;AmplifyShaderEditor.CommentaryNode;309;-434.9604,1618.297;Float;False;913.698;344.162;;4;314;313;312;310;World pos;0.5882354,0.5735294,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;300;-439.2292,1161.105;Float;False;905.8949;391.731;;7;301;302;303;304;305;306;307;Direction;0.5882354,0.5735294,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;310;-421.2106,1771.226;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;307;-405.1724,1386.214;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;301;-400.2388,1215.544;Float;False;Property;_Direction;Direction;23;0;Create;True;0,0,0;1,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;302;-179.6324,1329.197;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;-181.6324,1216.197;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;312;-210.3811,1714.025;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;304;-182.6324,1432.197;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;313;-18.03234,1713.078;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;305;-10.6282,1305.948;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;108;-449.3996,436.1732;Float;False;1419.977;571.5469;Length from start point to dissolve and glow;12;298;118;116;112;121;111;109;102;308;53;316;317;;1,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;71;-1795.89,439.7515;Float;False;1268.338;563.6168;Dissolve;12;120;6;28;27;16;15;8;1;258;259;260;315;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;314;192.9529,1708.263;Float;False;world_position;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;306;142.4135,1301.3;Float;False;local_direction;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-385.5625,679.1644;Float;False;314;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;308;-384.1149,572.3637;Float;False;306;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-421.7477,481.6112;Float;False;Property;_Position;Position;21;0;Create;True;0.5;0.64;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;316;-148.9623,608.9647;Float;False;Property;_PositionType;PositionType;22;0;Create;True;0;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;8;-1761.339,550.4435;Float;False;Property;_DissolveSpeed;Dissolve Speed;10;0;Create;True;0.19,0;0.2,-0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;1;-1760.064,702.8375;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;52;-1551.625,-583.8807;Float;False;2291.269;918.9568;Glow;22;296;282;242;43;184;236;238;284;235;237;168;261;169;262;171;247;187;245;244;279;243;162;;1,0,0,1;0;0
Node;AmplifyShaderEditor.PannerNode;15;-1537.951,609.0345;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;102;132.5352,512.3566;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;-1351.626,699.2545;Float;False;Property;_ScaleDissolveTex;Scale Dissolve Tex;9;0;Create;True;1;3;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-1340.499,565.7324;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LengthOpNode;109;332.2979,509.6273;Float;False;1;0;FLOAT;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-1513.183,-402.7553;Float;False;Property;_GlowDistribution;Glow Distribution;20;0;Create;True;0;0.639;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;244;-1509.222,-18.74087;Float;False;Property;_GlowSpeed;Glow Speed;13;0;Create;True;0.19,0;0.19,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;279;-1211.051,-398.2313;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;-1.0;False;4;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;111;-160.4583,815.465;Float;False;Property;_DissolveDistance;Dissolve Distance;18;0;Create;True;0;0.686;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;243;-1507.947,133.653;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;121;523.9289,505.6375;Float;False;Property;_MirrorMode;Mirror Mode;24;0;Create;True;0;2;0;FLOAT;0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;259;-1050.126,575.6544;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT;1.0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;72;-1553.69,-1079.764;Float;False;873.5732;457.1019;;4;158;157;154;283;Main Emission;0,0,0,1;0;0
Node;AmplifyShaderEditor.PannerNode;245;-1259.068,-19.40447;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;112;128.1091,705.892;Float;False;2;0;FLOAT;0,0,0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1766.626,879.2119;Float;False;Property;_Dissolve;Dissolve;17;0;Create;True;0;0.228;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-834.1752,496.6715;Float;True;Property;_DissolveTexture;Dissolve Texture;8;0;Create;True;None;c72a4cfc6aff9594bbf78865caa0e621;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;187;-1003.528,-399.1414;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;171;-848.143,-401.9488;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;154;-1524.936,-1025.039;Float;True;Property;_EmissionTexture;Emission Texture;7;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;157;-1438.803,-813.0695;Float;False;Property;_EmissionColor;Emission Color;6;1;[HDR];Create;True;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;262;-1085.293,214.4002;Float;False;Property;_ScaleGlowTex;Scale Glow Tex;12;0;Create;True;1;1;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;258;-641.5264,784.5545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;28;-1468.956,882.8278;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;247;-1071.497,91.41237;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-1202.753,-1021.278;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;261;-763.2472,47.58806;Float;False;3;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;2;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;169;-709.4904,-400.2806;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-1281.684,860.5728;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;283;-991.1934,-1023.273;Float;False;base_emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;235;-477.6174,20.49034;Float;True;Property;_GlowDistributionTexture;Glow Distribution Texture;11;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;284;-607.3528,-500.1033;Float;False;283;0;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;237;-438.5392,-218.3506;Float;False;Property;_GlowDistributionColor;Glow Distribution Color;16;1;[HDR];Create;True;0,0,0,0;1.5,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;315;-1055.979,940.053;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;168;-533.8247,-402.4076;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;136.7564,898.097;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;288;86.95319,-1080.27;Float;False;796.8995;456.7874;;7;22;24;20;25;45;289;290;Metallic & Smothness;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;287;-645.763,-1079.455;Float;False;708.4564;456.2273;;4;18;285;19;17;Albedo;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;236;-346.942,-402.7731;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;-179.661,-110.391;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;293;917.7592,-1075.814;Float;False;547.1859;456.7874;;2;225;294;Normal;0.3235294,0.4961459,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;17;-615.3373,-857.0171;Float;True;Property;_Albedo;Albedo;1;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;22;111.3108,-1034.194;Float;True;Property;_MetallicTexture;Metallic Texture;2;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;118;574.4821,702.7276;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;242;8.807616,-281.9766;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;43;-115.9648,154.1475;Float;False;Property;_DissolveColor;Dissolve Color;15;1;[HDR];Create;True;0,0,0,0;2.7,1.54497,0.4963236,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;119.5188,-752.81;Float;False;Property;_Smoothness;Smoothness;4;0;Create;True;0;0.952;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;-610.2986,-1036.425;Float;False;Property;_AlbedoColor;Albedo Color;0;0;Create;True;0,0,0,0;1,1,1,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;119.014,-827.1108;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-1512.481,-327.9073;Float;False;Property;_GlowDistance;Glow Distance;19;0;Create;True;0.1;0.053;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;461.4962,-987.114;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-317.466,-931.4338;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareLowerEqual;282;276.7615,23.52546;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;479.4958,-837.3328;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;225;940.0749,-1016.581;Float;True;Property;_NormalMap;Normal Map;5;1;[Normal];Create;True;None;None;True;0;True;bump;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;289;646.0239,-992.8705;Float;False;metallic;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;297;975.7368,22.4451;Float;False;296;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;295;988.2623,-66.96575;Float;False;294;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;290;644.0239,-840.8705;Float;False;smothness;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;286;1005.313,-152.7057;Float;False;285;0;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;299;986.4777,269.5336;Float;False;298;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;296;485.4494,19.28799;Float;False;emission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;292;972.9545,187.6161;Float;False;290;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;291;985.9545,107.6161;Float;False;289;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;298;747.9395,697.8693;Float;False;opacity;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;285;-158.7312,-936.5499;Float;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;294;1262.942,-1016.739;Float;False;normal;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1231.831,-27.13462;Fixed;False;True;2;Fixed;ASEMaterialInspector;0;0;Standard;QFX/MFX/MfxSingleAlbedo;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;False;0;Custom;0;True;True;0;True;TransparentCutout;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;OFF;OFF;0;False;6.91;1,0.3529412,0.4734279,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;14;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;302;0;301;2
WireConnection;302;1;307;2
WireConnection;303;0;301;1
WireConnection;303;1;307;1
WireConnection;312;0;301;0
WireConnection;312;1;310;0
WireConnection;304;0;301;3
WireConnection;304;1;307;3
WireConnection;313;0;312;0
WireConnection;305;0;303;0
WireConnection;305;1;302;0
WireConnection;305;2;304;0
WireConnection;314;0;313;0
WireConnection;306;0;305;0
WireConnection;316;0;308;0
WireConnection;316;1;317;0
WireConnection;15;2;8;0
WireConnection;15;1;1;2
WireConnection;102;0;53;0
WireConnection;102;1;316;0
WireConnection;16;1;15;0
WireConnection;109;0;102;0
WireConnection;279;0;162;0
WireConnection;121;0;102;0
WireConnection;121;1;109;0
WireConnection;259;0;16;0
WireConnection;259;1;260;0
WireConnection;245;2;244;0
WireConnection;245;1;243;2
WireConnection;112;0;121;0
WireConnection;112;1;111;0
WireConnection;6;1;259;0
WireConnection;187;0;279;0
WireConnection;171;0;187;0
WireConnection;171;1;112;0
WireConnection;258;0;6;1
WireConnection;28;0;27;0
WireConnection;247;1;245;0
WireConnection;158;0;154;0
WireConnection;158;1;157;0
WireConnection;261;0;247;0
WireConnection;261;1;262;0
WireConnection;169;0;171;0
WireConnection;120;0;258;0
WireConnection;120;1;28;0
WireConnection;283;0;158;0
WireConnection;235;1;261;0
WireConnection;315;0;120;0
WireConnection;168;0;169;0
WireConnection;116;0;111;0
WireConnection;116;1;315;0
WireConnection;236;0;284;0
WireConnection;236;1;168;0
WireConnection;238;0;237;0
WireConnection;238;1;235;0
WireConnection;118;0;112;0
WireConnection;118;1;116;0
WireConnection;242;0;236;0
WireConnection;242;1;238;0
WireConnection;25;0;22;1
WireConnection;25;1;24;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;282;0;118;0
WireConnection;282;1;184;0
WireConnection;282;2;43;0
WireConnection;282;3;242;0
WireConnection;45;0;22;4
WireConnection;45;1;20;0
WireConnection;289;0;25;0
WireConnection;290;0;45;0
WireConnection;296;0;282;0
WireConnection;298;0;118;0
WireConnection;285;0;19;0
WireConnection;294;0;225;0
WireConnection;0;0;286;0
WireConnection;0;1;295;0
WireConnection;0;2;297;0
WireConnection;0;3;291;0
WireConnection;0;4;292;0
WireConnection;0;10;299;0
ASEEND*/
//CHKSM=A76F02705C6BF811D2872E526097ACA6B6D11EBB