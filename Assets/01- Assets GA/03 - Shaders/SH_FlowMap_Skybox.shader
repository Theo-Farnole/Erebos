// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_FlowMap_Skybox"
{
	Properties
	{
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_TextureSample6("Texture Sample 6", 2D) = "white" {}
		_TextureSample4("Texture Sample 4", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample2;
		uniform sampler2D _TextureSample4;
		uniform float4 _TextureSample4_ST;
		uniform sampler2D _TextureSample6;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample4 = i.uv_texcoord * _TextureSample4_ST.xy + _TextureSample4_ST.zw;
			float4 tex2DNode58 = tex2D( _TextureSample4, uv_TextureSample4 );
			float temp_output_46_0 = ( _Time.y * 0.1 );
			float temp_output_45_0 = frac( ( temp_output_46_0 + 0.5 ) );
			float4 lerpResult79 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2DNode58 , temp_output_45_0);
			float4 lerpResult82 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2DNode58 , frac( ( temp_output_46_0 + 1.0 ) ));
			float4 lerpResult84 = lerp( tex2D( _TextureSample2, lerpResult79.rg ) , tex2D( _TextureSample6, lerpResult82.rg ) , abs( (-1.0 + (temp_output_45_0 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ));
			o.Emission = ( lerpResult84 * 1.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1044;73.6;1002;1000;2614.084;-309.6843;1.996184;True;False
Node;AmplifyShaderEditor.RangedFloatNode;48;-2958.476,711.3475;Float;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;47;-2982.887,561.2895;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-2374.74,654.4689;Float;False;Constant;_Float6;Float 6;4;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-2738.17,580.8024;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2369.824,1450.559;Float;False;Constant;_Float7;Float 7;4;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-2188.448,588.873;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-2183.531,1384.963;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;45;-1932.747,589.3444;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;58;-2101.398,1077.871;Float;True;Property;_TextureSample4;Texture Sample 4;2;0;Create;True;0;0;False;0;None;6965005bbff15974dac5ad53ab214280;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;80;-2055.702,887.85;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FractNode;52;-1919.607,1387.391;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;85;-1609.132,1703.135;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;79;-1653.668,547.5008;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;82;-1649.084,1339.234;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;53;-1336.714,518.2784;Float;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;148da43fbde65484bbfa67e20d4e04b3;148da43fbde65484bbfa67e20d4e04b3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;83;-1296.327,1314.224;Float;True;Property;_TextureSample6;Texture Sample 6;1;0;Create;True;0;0;False;0;148da43fbde65484bbfa67e20d4e04b3;148da43fbde65484bbfa67e20d4e04b3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;86;-1205.161,1704.025;Float;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;-763.9926,932.8257;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-488.166,1009.253;Float;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1214.792,-200.2285;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-302.166,931.2531;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;9;-1635.179,-109.2922;Float;True;True;True;False;False;1;0;COLOR;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-43.15607,883.0164;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_FlowMap_Skybox;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;47;2
WireConnection;46;1;48;0
WireConnection;42;0;46;0
WireConnection;42;1;43;0
WireConnection;50;0;46;0
WireConnection;50;1;51;0
WireConnection;45;0;42;0
WireConnection;52;0;50;0
WireConnection;85;0;45;0
WireConnection;79;0;80;0
WireConnection;79;1;58;0
WireConnection;79;2;45;0
WireConnection;82;0;80;0
WireConnection;82;1;58;0
WireConnection;82;2;52;0
WireConnection;53;1;79;0
WireConnection;83;1;82;0
WireConnection;86;0;85;0
WireConnection;84;0;53;0
WireConnection;84;1;83;0
WireConnection;84;2;86;0
WireConnection;6;0;84;0
WireConnection;6;1;7;0
WireConnection;0;2;6;0
ASEEND*/
//CHKSM=39E966CBD4A10B372BD0EAAF9423F6F7A7AEC4F8