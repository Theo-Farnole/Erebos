// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader " Erebos/SH_DeathFog_Global"
{
	Properties
	{
		_noise_map("noise_map", 2D) = "white" {}
		_1678bump("1678-bump", 2D) = "white" {}
		_FX_DeathFog_Texture_Mask4("FX_DeathFog_Texture_Mask4", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_Float1("Float 1", Float) = 0
		_Float4("Float 4", Float) = 1.94
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _1678bump;
		uniform float _Float0;
		uniform sampler2D _noise_map;
		uniform float _Float1;
		uniform sampler2D _FX_DeathFog_Texture_Mask4;
		uniform float _Float4;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color73 = IsGammaSpace() ? float4(0.1132075,0.1132075,0.1132075,0) : float4(0.01219615,0.01219615,0.01219615,0);
			float4 Color114 = color73;
			o.Emission = Color114.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 break75 = ase_worldPos;
			float2 appendResult76 = (float2(break75.x , break75.y));
			float2 panner2 = ( 0.5 * _Time.y * float2( -0.1,-1 ) + ( appendResult76 / _Float0 ));
			float2 panner13 = ( 0.6 * _Time.y * float2( 0.1,-0.5 ) + ( appendResult76 / _Float1 ));
			float3 desaturateInitialColor72 = ( pow( ( float4( i.uv_texcoord, 0.0 , 0.0 ) + tex2D( _1678bump, panner2 ) + tex2D( _noise_map, panner13 ) ) , 3.0 ) * float4( i.uv_texcoord, 0.0 , 0.0 ) * ( 1.0 - ( i.uv_texcoord.y * 1.2 ) ) ).rgb;
			float desaturateDot72 = dot( desaturateInitialColor72, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar72 = lerp( desaturateInitialColor72, desaturateDot72.xxx, 1.0 );
			float2 appendResult121 = (float2(( appendResult76 / _Float4 ).x , i.uv_texcoord.y));
			float4 clampResult65 = clamp( ( float4( desaturateVar72 , 0.0 ) + tex2D( _FX_DeathFog_Texture_Mask4, appendResult121 ) ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			float4 Opacity112 = ( clampResult65 * i.vertexColor.a );
			o.Alpha = Opacity112.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1920;1;1906;1011;1686.423;-489.8295;1;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;74;-3844.059,144.809;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.BreakToComponentsNode;75;-3589.716,144.8387;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;82;-2786.504,460.3555;Float;False;Property;_Float1;Float 1;8;0;Create;True;0;0;False;0;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2779.269,246.73;Float;False;Property;_Float0;Float 0;6;0;Create;True;0;0;False;0;0;17.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;76;-3291.085,146.2928;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;81;-2599.389,395.4549;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;80;-2587.527,152.0771;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;2;-2297.7,154.0335;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-1;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-2301.143,397.0586;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.5;False;1;FLOAT;0.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-2782.151,1101.373;Float;False;Property;_Float4;Float 4;11;0;Create;True;0;0;False;0;1.94;14.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-2022.549,612.9248;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;9;-2064.194,127.5546;Float;True;Property;_1678bump;1678-bump;2;0;Create;True;0;0;False;0;e2d5365aa0a61c84e84baa80ecea377a;e2d5365aa0a61c84e84baa80ecea377a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-2068.887,371.0689;Float;True;Property;_noise_map;noise_map;0;0;Create;True;0;0;False;0;2693b281ec19f554fb1333a4122c02fa;2693b281ec19f554fb1333a4122c02fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1472.566,704.6291;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;117;-2580.307,1084.659;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1461.046,332.2369;Float;True;3;3;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;85;-1209.962,704.6292;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;62;-1181.327,328.3106;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;119;-2367.8,1082.865;Float;False;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;121;-1590.482,1085.645;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-896.6517,589.0925;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT2;1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;-815.3018,1047.841;Float;True;Property;_FX_DeathFog_Texture_Mask4;FX_DeathFog_Texture_Mask4;4;0;Create;True;0;0;False;0;745b6133175d7a942abeb9b5bd2a9e07;e1f80f955b5f4ae44a73f5d530f76093;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;72;-621.5985,587.2745;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;-370.9301,587.1635;Float;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;68;-98.22168,710.4457;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;65;-110.193,586.0715;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;73;-78.53544,-216.7363;Float;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;0.1132075,0.1132075,0.1132075,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;170.0025,583.1992;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;241.5673,-217.2768;Float;False;Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;112;345.0741,574.519;Float;False;Opacity;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;107;325.7369,-2191.717;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;97;-1628.263,-2650.234;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;Create;True;0;0;False;0;e2d5365aa0a61c84e84baa80ecea377a;e2d5365aa0a61c84e84baa80ecea377a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;106;64.99994,-2190.625;Float;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;87;-3211.68,-2630.933;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;110;412.3353,-2404.831;Float;False;Constant;_Color1;Color 1;4;0;Create;True;0;0;False;0;0.1132075,0.1132075,0.1132075,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;109;605.9325,-2194.589;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;108;337.7083,-2067.343;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;102;-774.0312,-2073.16;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;94;-1865.214,-2380.73;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.5;False;1;FLOAT;0.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;103;-460.7216,-2188.696;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT2;1,1;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;1354.663,161.5432;Float;False;114;Color;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;95;-1861.771,-2623.755;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-1;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;98;-1632.958,-2406.72;Float;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;False;0;2693b281ec19f554fb1333a4122c02fa;2693b281ec19f554fb1333a4122c02fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;93;-2151.6,-2625.711;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;92;-2163.461,-2382.334;Float;False;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;88;-2957.336,-2630.904;Float;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.TextureCoordinatesNode;96;-1362.992,-2166.897;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-1025.116,-2445.552;Float;True;3;3;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;105;-450.4235,-1937.905;Float;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;745b6133175d7a942abeb9b5bd2a9e07;e1f80f955b5f4ae44a73f5d530f76093;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-2350.577,-2317.433;Float;False;Property;_Float3;Float 3;9;0;Create;True;0;0;False;0;0;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-2343.341,-2531.059;Float;False;Property;_Float2;Float 2;7;0;Create;True;0;0;False;0;0;17.6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;-1922.432,-2109.128;Float;True;Property;_Texture1;Texture 1;10;0;Create;True;0;0;False;0;None;None;False;white;Auto;Texture2D;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1036.635,-2073.16;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;1.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;101;-745.3969,-2449.478;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;104;-185.6685,-2190.514;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;90;-2658.706,-2629.449;Float;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;115;1396.156,373.1674;Float;False;112;Opacity;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1794.686,104.1352;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard; Erebos/SH_DeathFog_Global;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;75;0;74;0
WireConnection;76;0;75;0
WireConnection;76;1;75;1
WireConnection;81;0;76;0
WireConnection;81;1;82;0
WireConnection;80;0;76;0
WireConnection;80;1;78;0
WireConnection;2;0;80;0
WireConnection;13;0;81;0
WireConnection;9;1;2;0
WireConnection;7;1;13;0
WireConnection;84;0;30;2
WireConnection;117;0;76;0
WireConnection;117;1;118;0
WireConnection;27;0;30;0
WireConnection;27;1;9;0
WireConnection;27;2;7;0
WireConnection;85;0;84;0
WireConnection;62;0;27;0
WireConnection;119;0;117;0
WireConnection;121;0;119;0
WireConnection;121;1;30;2
WireConnection;14;0;62;0
WireConnection;14;1;30;0
WireConnection;14;2;85;0
WireConnection;69;1;121;0
WireConnection;72;0;14;0
WireConnection;71;0;72;0
WireConnection;71;1;69;0
WireConnection;65;0;71;0
WireConnection;67;0;65;0
WireConnection;67;1;68;4
WireConnection;114;0;73;0
WireConnection;112;0;67;0
WireConnection;107;0;106;0
WireConnection;97;1;95;0
WireConnection;106;0;104;0
WireConnection;106;1;105;0
WireConnection;109;0;107;0
WireConnection;109;1;108;4
WireConnection;102;0;100;0
WireConnection;94;0;92;0
WireConnection;103;0;101;0
WireConnection;103;1;96;0
WireConnection;103;2;102;0
WireConnection;95;0;93;0
WireConnection;98;1;94;0
WireConnection;93;0;90;0
WireConnection;93;1;89;0
WireConnection;92;0;90;0
WireConnection;92;1;91;0
WireConnection;88;0;87;0
WireConnection;99;0;96;0
WireConnection;99;1;97;0
WireConnection;99;2;98;0
WireConnection;100;0;96;2
WireConnection;101;0;99;0
WireConnection;104;0;103;0
WireConnection;90;0;88;0
WireConnection;90;1;88;1
WireConnection;0;2;113;0
WireConnection;0;9;115;0
ASEEND*/
//CHKSM=B1F43014C879C80466A2821D83D0D428B92CA7E1