// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader " Erebos/SH_DeathFog_Global"
{
	Properties
	{
		_noise_map("noise_map", 2D) = "white" {}
		_1678bump("1678-bump", 2D) = "white" {}
		_mask("mask", 2D) = "white" {}
		_FX_DeathFog_Texture_Mask4("FX_DeathFog_Texture_Mask4", 2D) = "white" {}
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
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _1678bump;
		uniform sampler2D _noise_map;
		uniform sampler2D _mask;
		uniform float4 _mask_ST;
		uniform sampler2D _FX_DeathFog_Texture_Mask4;
		uniform float4 _FX_DeathFog_Texture_Mask4_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color73 = IsGammaSpace() ? float4(0.1132075,0.1132075,0.1132075,0) : float4(0.01219616,0.01219616,0.01219616,0);
			o.Emission = color73.rgb;
			float2 uv_TexCoord3 = i.uv_texcoord * float2( 1.5,1.5 );
			float2 panner2 = ( 0.5 * _Time.y * float2( -0.1,-1 ) + uv_TexCoord3);
			float2 panner13 = ( 0.6 * _Time.y * float2( 0.1,-0.5 ) + i.uv_texcoord);
			float2 uv_mask = i.uv_texcoord * _mask_ST.xy + _mask_ST.zw;
			float4 tex2DNode53 = tex2D( _mask, uv_mask );
			float3 desaturateInitialColor72 = ( pow( ( float4( i.uv_texcoord, 0.0 , 0.0 ) + tex2D( _1678bump, panner2 ) + tex2D( _noise_map, panner13 ) ) , 3.0 ) * float4( i.uv_texcoord, 0.0 , 0.0 ) * tex2DNode53 * tex2DNode53 ).rgb;
			float desaturateDot72 = dot( desaturateInitialColor72, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar72 = lerp( desaturateInitialColor72, desaturateDot72.xxx, 1.0 );
			float2 uv_FX_DeathFog_Texture_Mask4 = i.uv_texcoord * _FX_DeathFog_Texture_Mask4_ST.xy + _FX_DeathFog_Texture_Mask4_ST.zw;
			float4 clampResult65 = clamp( ( float4( desaturateVar72 , 0.0 ) + tex2D( _FX_DeathFog_Texture_Mask4, uv_FX_DeathFog_Texture_Mask4 ) ) , float4( 0,0,0,0 ) , float4( 1,0,0,0 ) );
			o.Alpha = ( clampResult65 * i.vertexColor.a ).r;
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
1090.4;73.6;956;1000;179.003;731.2504;1.681269;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-1638.547,-325.2332;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1639.785,-82.20811;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-1386.547,-323.7332;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.1,-1;False;1;FLOAT;0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-1389.99,-80.70811;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.5;False;1;FLOAT;0.6;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-1153.039,-350.2121;Float;True;Property;_1678bump;1678-bump;1;0;Create;True;0;0;False;0;e2d5365aa0a61c84e84baa80ecea377a;e2d5365aa0a61c84e84baa80ecea377a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;30;-887.7677,133.1251;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;7;-1157.733,-106.6978;Float;True;Property;_noise_map;noise_map;0;0;Create;True;0;0;False;0;2693b281ec19f554fb1333a4122c02fa;2693b281ec19f554fb1333a4122c02fa;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-549.8915,-145.5298;Float;True;3;3;0;FLOAT2;0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;62;-270.1724,-149.4561;Float;True;2;0;COLOR;0,0,0,0;False;1;FLOAT;3;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-360.6719,297.4322;Float;True;Property;_mask;mask;2;0;Create;True;0;0;False;0;f40c8ee257d0813448ce5f179b5cb9bc;3b317f93a495db944abda686e3360802;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;14.5029,111.3258;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT2;1,1;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;69;24.80103,362.1173;Float;True;Property;_FX_DeathFog_Texture_Mask4;FX_DeathFog_Texture_Mask4;3;0;Create;True;0;0;False;0;745b6133175d7a942abeb9b5bd2a9e07;745b6133175d7a942abeb9b5bd2a9e07;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;72;289.556,109.5078;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;71;540.2244,109.3968;Float;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;65;800.9614,108.3048;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;68;812.9328,232.6789;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;67;1081.157,105.4325;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;73;802.4784,-71.07039;Float;False;Constant;_Color0;Color 0;4;0;Create;True;0;0;False;0;0.1132075,0.1132075,0.1132075,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1226.755,-97.45226;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard; Erebos/SH_DeathFog_Global;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;3;0
WireConnection;13;0;12;0
WireConnection;9;1;2;0
WireConnection;7;1;13;0
WireConnection;27;0;30;0
WireConnection;27;1;9;0
WireConnection;27;2;7;0
WireConnection;62;0;27;0
WireConnection;14;0;62;0
WireConnection;14;1;30;0
WireConnection;14;2;53;0
WireConnection;14;3;53;0
WireConnection;72;0;14;0
WireConnection;71;0;72;0
WireConnection;71;1;69;0
WireConnection;65;0;71;0
WireConnection;67;0;65;0
WireConnection;67;1;68;4
WireConnection;0;2;73;0
WireConnection;0;9;67;0
ASEEND*/
//CHKSM=523B076079198A258F8A8549C2B8F057C20BE86C