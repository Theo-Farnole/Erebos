// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_Add"
{
	Properties
	{
		_Emissivpowa("Emissiv powa", Float) = 2
		_Cutoff( "Mask Clip Value", Float ) = -0.04
		_FX_BlackHole_Texture_Rim("FX_BlackHole_Texture_Rim", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend One One , One One
		BlendOp Add
		CGPROGRAM
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred noambient 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _FX_BlackHole_Texture_Rim;
		uniform float4 _FX_BlackHole_Texture_Rim_ST;
		uniform float _Emissivpowa;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Cutoff = -0.04;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_FX_BlackHole_Texture_Rim = i.uv_texcoord * _FX_BlackHole_Texture_Rim_ST.xy + _FX_BlackHole_Texture_Rim_ST.zw;
			float4 tex2DNode13 = tex2D( _FX_BlackHole_Texture_Rim, uv_FX_BlackHole_Texture_Rim );
			o.Emission = ( i.vertexColor * tex2DNode13 * _Emissivpowa ).rgb;
			float temp_output_12_0 = 0.0;
			o.Metallic = temp_output_12_0;
			o.Smoothness = temp_output_12_0;
			o.Occlusion = temp_output_12_0;
			o.Alpha = temp_output_12_0;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth9 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD( ase_screenPos ))));
			float distanceDepth9 = abs( ( screenDepth9 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( 1.0 ) );
			float clampResult11 = clamp( distanceDepth9 , 0.0 , 1.0 );
			clip( ( ( i.vertexColor.a * tex2DNode13.a ) * clampResult11 ) - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
999;73;919;928;528.913;116.0002;1;True;False
Node;AmplifyShaderEditor.VertexColorNode;2;-871.4681,8.028146;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-865.6255,488.2487;Float;True;Property;_FX_BlackHole_Texture_Rim;FX_BlackHole_Texture_Rim;3;0;Create;True;0;0;False;0;30c74f3b576201d4a9e4991769ac3923;30c74f3b576201d4a9e4991769ac3923;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;9;-497.0868,545.3318;Float;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-581.5509,165.2976;Float;False;Property;_Emissivpowa;Emissiv powa;1;0;Create;True;0;0;False;0;2;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;11;-210.2896,655.2259;Float;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-365.9922,318.772;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-368.5265,8.562152;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-56.46954,202.2447;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-950.0943,249.0367;Float;True;Property;_MainTexture;Main Texture;0;0;Create;True;0;0;False;0;None;30c74f3b576201d4a9e4991769ac3923;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-93.16084,393.5573;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;139.7634,27.14066;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_Add;False;False;False;False;True;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;-0.04;True;False;0;True;Custom;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;1;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;9;0
WireConnection;7;0;2;4
WireConnection;7;1;13;4
WireConnection;4;0;2;0
WireConnection;4;1;13;0
WireConnection;4;2;8;0
WireConnection;10;0;7;0
WireConnection;10;1;11;0
WireConnection;0;2;4;0
WireConnection;0;3;12;0
WireConnection;0;4;12;0
WireConnection;0;5;12;0
WireConnection;0;9;12;0
WireConnection;0;10;10;0
ASEEND*/
//CHKSM=6B37681B2860CE6517AC23D4F750C22CCEDB170E