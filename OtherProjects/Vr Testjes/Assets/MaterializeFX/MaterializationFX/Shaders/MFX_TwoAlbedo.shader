// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "QFX/MFX/MfxTwoAlbedo"
{
	Properties
	{
		_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		_Albedo("Albedo", 2D) = "white" {}
		_MetallicTexture("Metallic Texture", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smothness("Smothness", Range( 0 , 1)) = 0
		_Normals("Normals", 2D) = "bump" {}
		_NormalsMix("Normals Mix", Range( 0 , 1)) = 1
		_BurntNormals("Burnt Normals", 2D) = "bump" {}
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionTexture("Emission Texture", 2D) = "white" {}
		_BurnTexture("Burn Texture", 2D) = "white" {}
		_BurnAnimationOffset("Burn Animation Offset", 2D) = "white" {}
		_BurntAlbedoColor("Burnt Albedo Color", Color) = (0,0,0,0)
		_BurntTexture("Burnt Texture", 2D) = "white" {}
		_BurntEmissionTexture("Burnt Emission Texture", 2D) = "white" {}
		[HDR]_BurnColor("Burn Color", Color) = (0,0,0,0)
		[HDR]_BurntEmissionColor("Burnt Emission Color", Color) = (0,0,0,0)
		_Cutoff( "Mask Clip Value", Float ) = 0.1
		_Position("Position", Range( -3 , 3)) = -0.2166527
		_BurnSize("Burn Size", Range( 1 , 6)) = 8
		_BurnEmissionDistribution("Burn Emission Distribution", Range( 1 , 6)) = 8
		_DissolveDistribution("Dissolve Distribution", Range( 0 , 1)) = 0
		[Toggle]_Invert("Invert", Float) = 1
		_Direction("Direction", Vector) = (0,0,0,0)
		[Toggle]_PositionType("PositionType", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZTest LEqual
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed2 uv3_texcoord3;
			fixed2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform sampler2D _BurntNormals;
		uniform float4 _BurntNormals_ST;
		uniform fixed _NormalsMix;
		uniform fixed4 _AlbedoColor;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform sampler2D _BurntTexture;
		uniform float4 _BurntTexture_ST;
		uniform fixed4 _BurntAlbedoColor;
		uniform fixed _Invert;
		uniform fixed _PositionType;
		uniform fixed3 _Direction;
		uniform fixed _Position;
		uniform sampler2D _BurnAnimationOffset;
		uniform float4 _BurnAnimationOffset_ST;
		uniform fixed _BurnSize;
		uniform sampler2D _EmissionTexture;
		uniform float4 _EmissionTexture_ST;
		uniform fixed4 _EmissionColor;
		uniform sampler2D _BurnTexture;
		uniform float4 _BurnTexture_ST;
		uniform fixed4 _BurnColor;
		uniform fixed _BurnEmissionDistribution;
		uniform sampler2D _BurntEmissionTexture;
		uniform float4 _BurntEmissionTexture_ST;
		uniform fixed4 _BurntEmissionColor;
		uniform sampler2D _MetallicTexture;
		uniform float4 _MetallicTexture_ST;
		uniform fixed _Metallic;
		uniform fixed _Smothness;
		uniform fixed _DissolveDistribution;
		uniform float _Cutoff = 0.1;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv3_Normals = i.uv3_texcoord3 * _Normals_ST.xy + _Normals_ST.zw;
			float2 uv_BurntNormals = i.uv_texcoord * _BurntNormals_ST.xy + _BurntNormals_ST.zw;
			fixed4 tex2DNode83 = tex2D( _BurntNormals, uv_BurntNormals );
			float4 appendResult182 = (fixed4(0.0 , tex2DNode83.g , 0.0 , tex2DNode83.r));
			float3 lerpResult103 = lerp( UnpackNormal( tex2D( _Normals, uv3_Normals ) ) , UnpackNormal( appendResult182 ) , _NormalsMix);
			float3 Normal197 = lerpResult103;
			o.Normal = Normal197;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 albedo195 = ( _AlbedoColor * tex2D( _Albedo, uv_Albedo ) );
			float2 uv_BurntTexture = i.uv_texcoord * _BurntTexture_ST.xy + _BurntTexture_ST.zw;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float local_direction453 = ( ( _Direction.x * ase_vertex3Pos.x ) + ( _Direction.y * ase_vertex3Pos.y ) + ( _Direction.z * ase_vertex3Pos.z ) );
			float3 ase_worldPos = i.worldPos;
			float world_position462 = length( ( _Direction - ase_worldPos ) );
			float2 uv_BurnAnimationOffset = i.uv_texcoord * _BurnAnimationOffset_ST.xy + _BurnAnimationOffset_ST.zw;
			float temp_output_258_0 = ( lerp(local_direction453,world_position462,_PositionType) - ( _Position - tex2D( _BurnAnimationOffset, uv_BurnAnimationOffset ).r ) );
			float temp_output_259_0 = ( temp_output_258_0 * (20.0 + (_BurnSize - 1.0) * (0.0 - 20.0) / (6.0 - 1.0)) );
			float clampResult318 = clamp( ( ( lerp(local_direction453,world_position462,_PositionType) + 0.5 ) - temp_output_259_0 ) , 0.0 , 1.0 );
			float4 lerpResult309 = lerp( albedo195 , ( tex2D( _BurntTexture, uv_BurntTexture ) * _BurntAlbedoColor ) , lerp(( 1.0 - clampResult318 ),clampResult318,_Invert));
			float4 albedowithburn310 = lerpResult309;
			o.Albedo = albedowithburn310.rgb;
			float2 uv_EmissionTexture = i.uv_texcoord * _EmissionTexture_ST.xy + _EmissionTexture_ST.zw;
			float4 baseemission239 = ( tex2D( _EmissionTexture, uv_EmissionTexture ) * _EmissionColor );
			float2 uv_BurnTexture = i.uv_texcoord * _BurnTexture_ST.xy + _BurnTexture_ST.zw;
			float clampResult291 = clamp( ( 1.0 - abs( temp_output_259_0 ) ) , 0.0 , 1.0 );
			float4 lerpResult274 = lerp( baseemission239 , ( tex2D( _BurnTexture, uv_BurnTexture ) * _BurnColor ) , clampResult291);
			float clampResult341 = clamp( ( 1.0 - abs( ( temp_output_258_0 * (6.0 + (_BurnEmissionDistribution - 0.0) * (0.0 - 6.0) / (6.0 - 0.0)) ) ) ) , 0.0 , 1.0 );
			float2 uv_BurntEmissionTexture = i.uv_texcoord * _BurntEmissionTexture_ST.xy + _BurntEmissionTexture_ST.zw;
			float clampResult330 = clamp( ( ( ( 1.0 - tex2D( _BurntEmissionTexture, uv_BurntEmissionTexture ).r ) - 0.5 ) * 3.0 ) , 0.0 , 1.0 );
			float4 burnemission272 = ( lerpResult274 + ( ( lerp(( 1.0 - clampResult318 ),clampResult318,_Invert) * clampResult341 ) * pow( clampResult330 , 3.0 ) * _BurntEmissionColor ) );
			o.Emission = burnemission272.rgb;
			float2 uv_MetallicTexture = i.uv_texcoord * _MetallicTexture_ST.xy + _MetallicTexture_ST.zw;
			fixed4 tex2DNode185 = tex2D( _MetallicTexture, uv_MetallicTexture );
			float Metallic188 = ( tex2DNode185.r * _Metallic );
			o.Metallic = Metallic188;
			float Smothness189 = ( tex2DNode185.a * _Smothness );
			o.Smoothness = Smothness189;
			o.Alpha = 1;
			float dissolvethreshold378 = ( lerp(( 1.0 - clampResult318 ),clampResult318,_Invert) - (0.0 + (clampResult291 - 0.0) * (1.0 - 0.0) / (0.26 - 0.0)) );
			float start_position409 = (0.0 + (temp_output_258_0 - -3.0) * (1.0 - 0.0) / (3.0 - -3.0));
			float clampResult440 = clamp( ( _DissolveDistribution * start_position409 ) , 0.0 , 1.0 );
			float dissolve350 = (( dissolvethreshold378 > 0.0 ) ? clampResult440 :  1.0 );
			clip( dissolve350 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14401
105;342;1610;568;2940.402;1869.365;5.360494;True;False
Node;AmplifyShaderEditor.CommentaryNode;452;-718.2722,-1490.004;Float;False;905.8949;391.731;;7;451;443;446;453;445;277;444;Direction;0.5882354,0.5735294,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;461;-718.3333,-1047.141;Float;False;913.698;344.162;;4;462;460;458;457;World pos;0.5882354,0.5735294,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;457;-688.3237,-854.4667;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PosVertexDataNode;277;-680.7153,-1256.568;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;444;-675.7817,-1427.238;Float;False;Property;_Direction;Direction;24;0;Create;True;0,0,0;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;458;-497.3673,-906.2471;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;445;-455.1753,-1313.585;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;443;-457.1753,-1426.585;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;446;-458.1753,-1210.585;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;460;-297.792,-909.0006;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;319;246.5863,-1503.218;Float;False;3667.097;1617.669;;56;310;272;309;334;274;307;345;332;378;308;342;331;291;321;347;300;271;275;341;294;330;318;299;329;316;261;340;338;328;259;336;337;326;258;298;260;344;343;283;254;253;255;280;252;302;303;251;398;405;406;407;409;442;454;463;464;Burn & Emission;1,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;451;-286.1711,-1336.834;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;462;-66.93365,-913.8159;Float;False;world_position;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;464;380.6268,-966.374;Float;False;462;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;255;532.1648,-443.4161;Float;True;Property;_BurnAnimationOffset;Burn Animation Offset;11;0;Create;True;None;df645ca93f6d2cf49bb80e914afd3d87;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;453;-133.1294,-1341.482;Float;False;local_direction;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;454;382.556,-1101.622;Float;False;453;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;280;392.423,-850.5815;Float;False;Property;_Position;Position;18;0;Create;True;-0.2166527;0.19;-3;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;463;675.558,-958.479;Float;False;Property;_PositionType;PositionType;25;0;Create;True;0;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;260;882.7297,-372.5345;Float;False;Property;_BurnSize;Burn Size;19;0;Create;True;8;3.77;1;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;254;930.4149,-626.8662;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;343;865.5757,-142.5972;Float;False;Property;_BurnEmissionDistribution;Burn Emission Distribution;21;0;Create;True;8;4.76;1;6;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;298;1152.036,-366.1499;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;6.0;False;3;FLOAT;20.0;False;4;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;258;1161.964,-650.3511;Float;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;283;984.3898,-953.2651;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;326;1692.548,-224.3882;Float;True;Property;_BurntEmissionTexture;Burnt Emission Texture;14;0;Create;True;None;324e028e688c4324fa336048d9faf3c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;259;1535.174,-564.4185;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;344;1143.516,-136.3935;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;6.0;False;3;FLOAT;6.0;False;4;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;336;2011.522,-197.266;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;261;1709.521,-563.4186;Float;False;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;316;1299.814,-952.6301;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;337;1533.37,-400.1487;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;4.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;318;1569.029,-952.3755;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;328;2179.936,-198.5507;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;338;1671.755,-399.9891;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;294;1877.118,-562.6268;Float;False;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;304;-1694.017,-922.0385;Float;False;819.8988;450.0425;;4;239;238;236;237;Main Emission;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;39;-1675.112,310.9511;Float;False;1337.307;639.9694;;7;197;103;181;13;82;182;83;Normals;0.3088235,0.5137931,1,1;0;0
Node;AmplifyShaderEditor.TFHCRemapNode;442;1071.286,-1289.401;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;-3.0;False;2;FLOAT;3.0;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;291;2087.335,-663.0591;Float;True;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;354;237.0444,203.9603;Float;False;1458.699;661.5581;;8;350;441;379;381;440;431;427;430;Dissolve;0.8490872,0.3161765,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;406;1817.044,-950.8586;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;236;-1669.402,-867.5405;Float;True;Property;_EmissionTexture;Emission Texture;9;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;340;1806.323,-399.9892;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;329;2340.653,-200.5864;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;305;-1683.797,-1481.072;Float;False;732.6794;443.2622;;4;193;195;194;80;Albedo;0,0,0,1;0;0
Node;AmplifyShaderEditor.ColorNode;237;-1663.927,-640.2769;Float;False;Property;_EmissionColor;Emission Color;8;0;Create;True;0,0,0,0;0,0,0,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;80;-1665.066,-1242.047;Float;True;Property;_Albedo;Albedo;1;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;427;261.433,406.8836;Float;False;Property;_DissolveDistribution;Dissolve Distribution;22;0;Create;True;0;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;238;-1272.375,-708.7309;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;193;-1659.535,-1427.148;Float;False;Property;_AlbedoColor;Albedo Color;0;0;Create;True;0,0,0,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;409;1243.854,-1293.11;Float;False;start_position;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;341;2089.323,-444.9892;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;407;1811.465,-857.088;Float;False;Property;_Invert;Invert;23;0;Create;True;1;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;299;2169.562,-880.2693;Float;False;Property;_BurnColor;Burn Color;15;1;[HDR];Create;True;0,0,0,0;4,1.387424,0.6176472,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;275;2103.683,-1085.408;Float;True;Property;_BurnTexture;Burn Texture;10;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;405;2412.586,-667.4795;Float;True;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.26;False;3;FLOAT;0.0;False;4;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;430;305.4681,594.3226;Float;True;409;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;330;2498.466,-198.7505;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;83;-1618.813,591.9593;Float;True;Property;_BurntNormals;Burnt Normals;7;0;Create;True;None;ca02b40705c7a9a41bd0355840d6aa1b;True;0;False;bump;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;321;2757.148,-94.91475;Float;False;Property;_BurntEmissionColor;Burnt Emission Color;16;1;[HDR];Create;True;0,0,0,0;4,1.387424,0.6176472,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;306;-1705.379,-299.9932;Float;False;803.3513;419.69;;7;188;189;187;186;183;184;185;Metallic & Smothness;0,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;194;-1332.978,-1316.668;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;182;-1269.376,587.7513;Float;False;FLOAT4;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;398;2592.294,-768.8261;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;331;2762.136,-214.0921;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;3.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;300;2463.445,-969.694;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;342;2465.393,-465.3846;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;239;-1091.658,-714.2068;Float;False;baseemission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;347;1482.205,-1143.782;Float;False;Property;_BurntAlbedoColor;Burnt Albedo Color;12;0;Create;True;0,0,0,0;1,1,1,1;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;431;686.2783,497.0257;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;271;2460.52,-1085.807;Float;False;239;0;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;308;1479.675,-1347.265;Float;True;Property;_BurntTexture;Burnt Texture;13;0;Create;True;None;df645ca93f6d2cf49bb80e914afd3d87;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;381;1067.537,739.1577;Float;False;Constant;_Float1;Float 1;26;0;Create;True;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;378;2773.952,-773.156;Float;True;dissolvethreshold;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;379;966.7369,266.3936;Float;True;378;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;307;1769.361,-1421.583;Float;False;195;0;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;440;972.0905,496.6536;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1153.301,851.1783;Float;False;Property;_NormalsMix;Normals Mix;6;0;Create;True;1;0.326;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;183;-1676.251,-44.37175;Float;False;Property;_Metallic;Metallic;3;0;Create;True;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;195;-1162.979,-1321.668;Float;False;albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;184;-1680.746,32.92897;Float;False;Property;_Smothness;Smothness;4;0;Create;True;0;0.075;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.UnpackScaleNormalNode;181;-1120.376,587.7513;Float;True;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;1.0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;274;2734.434,-992.1473;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;82;-1210.589,369.0103;Float;True;Property;_Normals;Normals;5;0;Create;True;None;None;True;2;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;345;1793.921,-1208.94;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;185;-1680.001,-254.9519;Float;True;Property;_MetallicTexture;Metallic Texture;2;0;Create;True;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;332;3041.279,-260.5898;Float;True;3;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;309;2084.264,-1319.566;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-1322.5,-39.82145;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;103;-805.1798,488.4513;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCCompareGreater;441;1256.005,453.1627;Float;False;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-1329.817,-170.6501;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;334;3308.541,-769.5127;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;191;2138.815,550.5549;Float;False;188;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;198;2135.423,373.8956;Float;False;197;0;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;2141.501,734.1584;Float;False;350;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;189;-1164.533,-42.87535;Float;False;Smothness;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;2139.576,630.5002;Float;False;189;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;302;595.0408,-735.3912;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;312;2133.573,298.7746;Float;False;310;0;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;252;595.2488,-542.2404;Float;False;Constant;_Float0;Float 0;22;0;Create;True;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;350;1462.225,447.0446;Float;False;dissolve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;197;-579.957,484.1693;Float;True;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;188;-1168.078,-174.923;Float;False;Metallic;-1;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;272;3663.264,-774.6866;Float;True;burnemission;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;303;276.361,-588.2183;Float;False;Property;_BurnSpeed;Burn Speed;20;0;Create;True;2;0.48;0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;251;330.9551,-733.8392;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FmodOpNode;253;735.0966,-632.7344;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;273;2136.685,456.798;Float;False;272;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;310;2270.587,-1324.363;Float;True;albedowithburn;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2465.209,415.759;Fixed;False;True;2;Fixed;ASEMaterialInspector;0;0;Standard;QFX/MFX/MfxTwoAlbedo;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;3;False;0;0;False;0;Custom;0.1;True;True;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;0;4;10;25;False;0.5;True;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;17;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0.0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;458;0;444;0
WireConnection;458;1;457;0
WireConnection;445;0;444;2
WireConnection;445;1;277;2
WireConnection;443;0;444;1
WireConnection;443;1;277;1
WireConnection;446;0;444;3
WireConnection;446;1;277;3
WireConnection;460;0;458;0
WireConnection;451;0;443;0
WireConnection;451;1;445;0
WireConnection;451;2;446;0
WireConnection;462;0;460;0
WireConnection;453;0;451;0
WireConnection;463;0;454;0
WireConnection;463;1;464;0
WireConnection;254;0;280;0
WireConnection;254;1;255;1
WireConnection;298;0;260;0
WireConnection;258;0;463;0
WireConnection;258;1;254;0
WireConnection;283;0;463;0
WireConnection;259;0;258;0
WireConnection;259;1;298;0
WireConnection;344;0;343;0
WireConnection;336;0;326;1
WireConnection;261;0;259;0
WireConnection;316;0;283;0
WireConnection;316;1;259;0
WireConnection;337;0;258;0
WireConnection;337;1;344;0
WireConnection;318;0;316;0
WireConnection;328;0;336;0
WireConnection;338;0;337;0
WireConnection;294;0;261;0
WireConnection;442;0;258;0
WireConnection;291;0;294;0
WireConnection;406;0;318;0
WireConnection;340;0;338;0
WireConnection;329;0;328;0
WireConnection;238;0;236;0
WireConnection;238;1;237;0
WireConnection;409;0;442;0
WireConnection;341;0;340;0
WireConnection;407;0;406;0
WireConnection;407;1;318;0
WireConnection;405;0;291;0
WireConnection;330;0;329;0
WireConnection;194;0;193;0
WireConnection;194;1;80;0
WireConnection;182;1;83;2
WireConnection;182;3;83;1
WireConnection;398;0;407;0
WireConnection;398;1;405;0
WireConnection;331;0;330;0
WireConnection;300;0;275;0
WireConnection;300;1;299;0
WireConnection;342;0;407;0
WireConnection;342;1;341;0
WireConnection;239;0;238;0
WireConnection;431;0;427;0
WireConnection;431;1;430;0
WireConnection;378;0;398;0
WireConnection;440;0;431;0
WireConnection;195;0;194;0
WireConnection;181;0;182;0
WireConnection;274;0;271;0
WireConnection;274;1;300;0
WireConnection;274;2;291;0
WireConnection;345;0;308;0
WireConnection;345;1;347;0
WireConnection;332;0;342;0
WireConnection;332;1;331;0
WireConnection;332;2;321;0
WireConnection;309;0;307;0
WireConnection;309;1;345;0
WireConnection;309;2;407;0
WireConnection;187;0;185;4
WireConnection;187;1;184;0
WireConnection;103;0;82;0
WireConnection;103;1;181;0
WireConnection;103;2;13;0
WireConnection;441;0;379;0
WireConnection;441;2;440;0
WireConnection;441;3;381;0
WireConnection;186;0;185;1
WireConnection;186;1;183;0
WireConnection;334;0;274;0
WireConnection;334;1;332;0
WireConnection;189;0;187;0
WireConnection;302;0;251;3
WireConnection;302;1;303;0
WireConnection;350;0;441;0
WireConnection;197;0;103;0
WireConnection;188;0;186;0
WireConnection;272;0;334;0
WireConnection;253;0;302;0
WireConnection;253;1;252;0
WireConnection;310;0;309;0
WireConnection;0;0;312;0
WireConnection;0;1;198;0
WireConnection;0;2;273;0
WireConnection;0;3;191;0
WireConnection;0;4;192;0
WireConnection;0;10;351;0
ASEEND*/
//CHKSM=5D40AE9A81046EEC88B30119E15BE5C3DC24D60F