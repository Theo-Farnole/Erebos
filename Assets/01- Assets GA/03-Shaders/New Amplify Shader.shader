// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_TKT"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_try("try", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _try;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color11 = IsGammaSpace() ? float4(0.9883373,0.9883373,0.9883373,0) : float4(0.973674,0.973674,0.973674,0);
			o.Emission = color11.rgb;
			o.Alpha = 1;
			float2 uv_TexCoord10 = i.uv_texcoord * float2( 2,2 ) + float2( 1,0 );
			float2 panner2 = ( 1.0 * _Time.y * float2( -0.35,-0.35 ) + uv_TexCoord10);
			clip( tex2D( _try, panner2 ).r - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1211.2;80.8;828;986;632.689;-203.5557;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-667.7646,780.7229;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2,2;False;1;FLOAT2;1,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;2;-379.2282,787.9199;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.35,-0.35;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;15;-172.4727,757.6473;Float;True;Property;_try2;try 2;0;0;Create;True;0;0;False;0;369a045254c9aeb4d82caf8693c88ec6;369a045254c9aeb4d82caf8693c88ec6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-78.88867,1027.756;Float;True;Property;_try;try;2;0;Create;True;0;0;False;0;8e317a0bdecd9d04e8f5a843ce1e8cd6;8e317a0bdecd9d04e8f5a843ce1e8cd6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;11;-45.04815,475.5734;Float;False;Constant;_Color0;Color 0;2;1;[HDR];Create;True;0;0;False;0;0.9883373,0.9883373,0.9883373,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;261.7038,439.2584;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_TKT;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;0;10;0
WireConnection;15;1;2;0
WireConnection;16;1;2;0
WireConnection;0;2;11;0
WireConnection;0;10;16;0
ASEEND*/
//CHKSM=C3AA3F1BD099C7BBC2989B936C9B2F67701458D4