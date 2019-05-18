// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_BraseroFire"
{
	Properties
	{
		_TextureSample5("Texture Sample 5", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_yeeeeboisdsdsd("yeeee boi sdsdsd", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample5;
		uniform sampler2D _yeeeeboisdsdsd;
		uniform sampler2D _TextureSample2;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord251 = i.uv_texcoord * float2( 0.7,0.7 ) + float2( 1,1 );
			float2 panner256 = ( 1.5 * _Time.y * float2( 0.05,-0.5 ) + uv_TexCoord251);
			float2 uv_TexCoord248 = i.uv_texcoord * float2( 0.5,0.5 );
			float2 panner252 = ( 0.9 * _Time.y * float2( 0.1,-0.5 ) + uv_TexCoord248);
			float4 tex2DNode264 = tex2D( _TextureSample5, ( ( tex2D( _yeeeeboisdsdsd, panner256 ) + tex2D( _TextureSample2, panner252 ) + float4( i.uv_texcoord, 0.0 , 0.0 ) ) * float4( i.uv_texcoord, 0.0 , 0.0 ) ).rg );
			float3 temp_cast_3 = (( 5.0 * tex2DNode264.r )).xxx;
			o.Emission = temp_cast_3;
			o.Alpha = 1;
			clip( tex2DNode264.a - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1352;81.6;687;985;802.471;-1007.804;1.3;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;251;-671.5039,1250.213;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.7,0.7;False;1;FLOAT2;1,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;248;-601.4714,1476.646;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;256;-421.5052,1250.213;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,-0.5;False;1;FLOAT;1.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;252;-355.9086,1476.792;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,-0.5;False;1;FLOAT;0.9;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;458;-149.8829,1224.788;Float;True;Property;_yeeeeboisdsdsd;yeeee boi sdsdsd;3;0;Create;True;0;0;False;0;None;48a21a8e0294df245918fd54aec53cad;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;250;-103.693,1782.944;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;255;-174.6918,1449.719;Float;True;Property;_TextureSample2;Texture Sample 2;2;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;445;366.4335,1433.941;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;450;787.9935,1754.602;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;264;1130.079,1728.096;Float;True;Property;_TextureSample5;Texture Sample 5;0;0;Create;True;0;0;False;0;None;0bfb06a6482525b4baa8a7fd3be657c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;266;1573.366,1640.829;Float;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;267;1721.806,1730.984;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2017.975,1594.124;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_BraseroFire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;256;0;251;0
WireConnection;252;0;248;0
WireConnection;458;1;256;0
WireConnection;255;1;252;0
WireConnection;445;0;458;0
WireConnection;445;1;255;0
WireConnection;445;2;250;0
WireConnection;450;0;445;0
WireConnection;450;1;250;0
WireConnection;264;1;450;0
WireConnection;267;0;266;0
WireConnection;267;1;264;1
WireConnection;0;2;267;0
WireConnection;0;10;264;4
ASEEND*/
//CHKSM=D4288A01DE73C5C8DB092FFCE976E3AC44F6A097