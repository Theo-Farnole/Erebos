// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_Athena"
{
	Properties
	{
		_SM_Athena_Body_UV_lambert1_BaseColor("SM_Athena_Body_UV_lambert1_BaseColor", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _SM_Athena_Body_UV_lambert1_BaseColor;
		uniform float4 _SM_Athena_Body_UV_lambert1_BaseColor_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_SM_Athena_Body_UV_lambert1_BaseColor = i.uv_texcoord * _SM_Athena_Body_UV_lambert1_BaseColor_ST.xy + _SM_Athena_Body_UV_lambert1_BaseColor_ST.zw;
			float4 tex2DNode2 = tex2D( _SM_Athena_Body_UV_lambert1_BaseColor, uv_SM_Athena_Body_UV_lambert1_BaseColor );
			o.Albedo = tex2DNode2.rgb;
			o.Emission = ( tex2DNode2 * 2.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
530.4;296.8;1074;768;1718.053;293.3057;1.639688;False;False
Node;AmplifyShaderEditor.SamplerNode;2;-596,-13;Float;True;Property;_SM_Athena_Body_UV_lambert1_BaseColor;SM_Athena_Body_UV_lambert1_BaseColor;0;0;Create;True;0;0;False;0;ca9a64271d1c96240bcba6ab6c08fcbb;77324571f0854a146bd16a4020c224d6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-546,176;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-161,52;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;5,-3;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_Athena;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;0;0;2;0
WireConnection;0;2;3;0
ASEEND*/
//CHKSM=5E0EC4517A514BBA0FEE9A914C41096EE90F8D68