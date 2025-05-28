// Made with Amplify Shader Editor v1.9.1.5
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Robot_with_decals_BI"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,0)
		_TexMain("Diffuse", 2D) = "white" {}
		_PaintedPartsColor("PaintedPartsColor", Color) = (0,0,0,0)
		_PaintedPartsMask("Painted Parts Mask", 2D) = "white" {}
		_Decals1Color("Decals 1 Color", Color) = (1,0.07075471,0.07075471,0)
		_Decals1Opacity("Decals 1 Opacity", Range( 0 , 1)) = 0
		_Decals1("Decals 1", 2D) = "white" {}
		_Decals2Color("Decals 2 Color", Color) = (1,0.07075471,0.07075471,0)
		_Decals2Opacity("Decals 2 Opacity", Range( 0 , 1)) = 0
		_Decals2("Decals 2", 2D) = "white" {}
		[HDR]_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionIntencity("Emission Intencity", Range( 0 , 1)) = 0
		_Emission("Emission", 2D) = "white" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_AmbientOclusion("Ambient Oclusion", 2D) = "white" {}
		_OcclusionIntencity("Occlusion Intencity", Range( 0 , 0.75)) = 0
		[Normal]_NormalMap("NormalMap", 2D) = "bump" {}
		_NormalIntencity("Normal Intencity", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 5.0
		#pragma only_renderers d3d11 glcore gles3 metal vulkan 
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _NormalIntencity;
		uniform float4 _Color;
		uniform sampler2D _TexMain;
		uniform float4 _TexMain_ST;
		uniform float4 _PaintedPartsColor;
		uniform sampler2D _PaintedPartsMask;
		uniform float4 _PaintedPartsMask_ST;
		uniform float4 _Decals1Color;
		uniform sampler2D _Decals1;
		uniform float4 _Decals1_ST;
		uniform float _Decals1Opacity;
		uniform float4 _Decals2Color;
		uniform sampler2D _Decals2;
		uniform float4 _Decals2_ST;
		uniform float _Decals2Opacity;
		uniform float4 _EmissionColor;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float _EmissionIntencity;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _OcclusionIntencity;
		uniform sampler2D _AmbientOclusion;
		uniform float4 _AmbientOclusion_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = ( UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) ) * _NormalIntencity );
			float2 uv_TexMain = i.uv_texcoord * _TexMain_ST.xy + _TexMain_ST.zw;
			float4 tex2DNode2 = tex2D( _TexMain, uv_TexMain );
			float4 blendOpSrc27 = tex2DNode2;
			float4 blendOpDest27 = _PaintedPartsColor;
			float2 uv_PaintedPartsMask = i.uv_texcoord * _PaintedPartsMask_ST.xy + _PaintedPartsMask_ST.zw;
			float4 lerpResult39 = lerp( tex2DNode2 , ( saturate( ( blendOpSrc27 * blendOpDest27 ) )) , tex2D( _PaintedPartsMask, uv_PaintedPartsMask ));
			float4 blendOpSrc40 = tex2DNode2;
			float4 blendOpDest40 = _Decals1Color;
			float2 uv_Decals1 = i.uv_texcoord * _Decals1_ST.xy + _Decals1_ST.zw;
			float4 lerpResult7 = lerp( ( _Color * lerpResult39 ) , ( saturate( ( blendOpSrc40 * blendOpDest40 ) )) , ( tex2D( _Decals1, uv_Decals1 ) * _Decals1Opacity ));
			float2 uv_Decals2 = i.uv_texcoord * _Decals2_ST.xy + _Decals2_ST.zw;
			float4 lerpResult13 = lerp( lerpResult7 , _Decals2Color , ( tex2D( _Decals2, uv_Decals2 ) * _Decals2Opacity ));
			o.Albedo = lerpResult13.rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float4 blendOpSrc22 = _EmissionColor;
			float4 blendOpDest22 = tex2D( _Emission, uv_Emission );
			o.Emission = ( ( saturate( (( blendOpDest22 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest22 ) * ( 1.0 - blendOpSrc22 ) ) : ( 2.0 * blendOpDest22 * blendOpSrc22 ) ) )) * _EmissionIntencity ).rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode18 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = ( tex2DNode18 * _Metallic ).r;
			o.Smoothness = ( tex2DNode18.a * _Smoothness );
			float2 uv_AmbientOclusion = i.uv_texcoord * _AmbientOclusion_ST.xy + _AmbientOclusion_ST.zw;
			float4 tex2DNode19 = tex2D( _AmbientOclusion, uv_AmbientOclusion );
			float temp_output_1_0_g2 = ( tex2DNode19 + ( 1.0 - _OcclusionIntencity ) ).r;
			o.Occlusion = ( ( _OcclusionIntencity - temp_output_1_0_g2 ) / ( tex2DNode19.r - temp_output_1_0_g2 ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19105
Node;AmplifyShaderEditor.SamplerNode;11;-1816.144,-31.50973;Inherit;True;Property;_Decals2;Decals 2;9;0;Create;True;0;0;0;False;0;False;-1;None;747e5a9b677565343ab80ccf4c85284b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;12;-1728.144,-208.5097;Inherit;False;Property;_Decals2Color;Decals 2 Color;7;0;Create;True;0;0;0;False;0;False;1,0.07075471,0.07075471,0;0.2169808,0.2169808,0.2169808,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-1526.645,57.99025;Inherit;False;Property;_Decals2Opacity;Decals 2 Opacity;8;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1246.645,-26.00972;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;13;-913.3912,-373.7001;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;20;-1731.589,548.3934;Inherit;True;Property;_Emission;Emission;12;0;Create;True;0;0;0;False;0;False;-1;None;0206f4cf75adf7a4595ed2c03345aaed;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;22;-1340.489,387.8935;Inherit;False;Overlay;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;-1088.715,1094.954;Inherit;True;Property;_MetallicSmoothness;MetallicSmoothness;13;0;Create;True;0;0;0;False;0;False;-1;None;8fa0d3b0a295c1b46ab9ea46d374570b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-749.1636,1288.162;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1063.165,1393.692;Inherit;False;Property;_Smoothness;Smoothness;15;0;Create;True;0;0;0;False;0;False;0;0.891;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-753.4816,1187.105;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1067.483,1292.635;Inherit;False;Property;_Metallic;Metallic;14;0;Create;True;0;0;0;False;0;False;0;0.809;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-762.8264,703.4114;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1050.126,880.2112;Inherit;False;Property;_NormalIntencity;Normal Intencity;19;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;21;-1677.589,374.3935;Inherit;False;Property;_EmissionColor;Emission Color;10;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.270603,1.902076,0.1545437,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;37;-1352.47,532.6898;Inherit;False;Property;_EmissionIntencity;Emission Intencity;11;0;Create;True;0;0;0;False;0;False;0;0.993;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1010.47,315.6898;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;23;-1096.926,676.1113;Inherit;True;Property;_NormalMap;NormalMap;18;1;[Normal];Create;True;0;0;0;False;0;False;-1;None;b665f9fe3ec14644eb7e5bcbae0b1969;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1773.295,-1422.549;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;1;-2173.295,-1646.549;Inherit;False;Property;_Color;Color;0;0;Create;False;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-2901.026,-1009.243;Inherit;True;Property;_PaintedPartsMask;Painted Parts Mask;3;0;Create;True;0;0;0;False;0;False;-1;None;9ce30ca378cacf849bdc1974a3d57f15;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-2805.026,-1201.243;Inherit;False;Property;_PaintedPartsColor;PaintedPartsColor;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,0.173879,0.1556601,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;7;-1369.755,-1060.306;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;5;-2194.871,-734.1536;Inherit;True;Property;_Decals1;Decals 1;6;0;Create;True;0;0;0;False;0;False;-1;None;4e6455e8faa20fa49a65a0d8f7088c71;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;16;-2181.11,-526.2717;Inherit;False;Property;_Decals1Opacity;Decals 1 Opacity;5;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1628.88,-839.9624;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;2;-2957.396,-1448.049;Inherit;True;Property;_TexMain;Diffuse;1;0;Create;False;0;0;0;True;0;False;-1;None;2d7edc99b4c93294a97d7982c432d52a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;27;-2514.997,-1527.748;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;6;-2276.833,-989.4001;Inherit;False;Property;_Decals1Color;Decals 1 Color;4;0;Create;True;0;0;0;False;0;False;1,0.07075471,0.07075471,0;1,0.9118239,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;40;-1999.704,-1115.937;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;39;-2159.396,-1362.047;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;19;-1217.988,1624.151;Inherit;True;Property;_AmbientOclusion;Ambient Oclusion;16;0;Create;True;0;0;0;False;0;False;-1;None;741db1ac7bc385744a41de069d79d0aa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;53;-957.021,1829.457;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;71.41772,783.1488;Float;False;True;-1;7;ASEMaterialInspector;0;0;Standard;Robot_with_decals_BI;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;5;d3d11;glcore;gles3;metal;vulkan;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-673.083,1504.71;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;63;-332.1169,1687.879;Inherit;True;Inverse Lerp;-1;;2;09cbe79402f023141a4dc1fddd4c9511;0;3;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-1298.358,1890.241;Inherit;False;Property;_OcclusionIntencity;Occlusion Intencity;17;0;Create;True;0;0;0;False;0;False;0;0.5;0;0.75;0;1;FLOAT;0
WireConnection;14;0;11;0
WireConnection;14;1;15;0
WireConnection;13;0;7;0
WireConnection;13;1;12;0
WireConnection;13;2;14;0
WireConnection;22;0;21;0
WireConnection;22;1;20;0
WireConnection;34;0;18;4
WireConnection;34;1;33;0
WireConnection;35;0;18;0
WireConnection;35;1;36;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;38;0;22;0
WireConnection;38;1;37;0
WireConnection;4;0;1;0
WireConnection;4;1;39;0
WireConnection;7;0;4;0
WireConnection;7;1;40;0
WireConnection;7;2;17;0
WireConnection;17;0;5;0
WireConnection;17;1;16;0
WireConnection;27;0;2;0
WireConnection;27;1;28;0
WireConnection;40;0;2;0
WireConnection;40;1;6;0
WireConnection;39;0;2;0
WireConnection;39;1;27;0
WireConnection;39;2;30;0
WireConnection;53;0;31;0
WireConnection;0;0;13;0
WireConnection;0;1;24;0
WireConnection;0;2;38;0
WireConnection;0;3;35;0
WireConnection;0;4;34;0
WireConnection;0;5;63;0
WireConnection;55;0;19;0
WireConnection;55;1;53;0
WireConnection;63;1;55;0
WireConnection;63;2;19;0
WireConnection;63;3;31;0
ASEEND*/
//CHKSM=140DDA73D884BB11DF7BA2B73240577FD45427C1