// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_EclatosActif"
{
	Properties
	{
		_SM_Eclatos_lambert2_BaseColor("SM_Eclatos_lambert2_BaseColor", 2D) = "white" {}
		_FX_BraseroFire_Texture_Noise01("FX_BraseroFire_Texture_Noise01", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _SM_Eclatos_lambert2_BaseColor;
		uniform float4 _SM_Eclatos_lambert2_BaseColor_ST;
		uniform sampler2D _FX_BraseroFire_Texture_Noise01;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color13 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			o.Albedo = color13.rgb;
			float2 uv_SM_Eclatos_lambert2_BaseColor = i.uv_texcoord * _SM_Eclatos_lambert2_BaseColor_ST.xy + _SM_Eclatos_lambert2_BaseColor_ST.zw;
			float2 panner7 = ( 0.2 * _Time.y * float2( 0.5,0.5 ) + i.uv_texcoord);
			o.Emission = ( ( tex2D( _SM_Eclatos_lambert2_BaseColor, uv_SM_Eclatos_lambert2_BaseColor ) * tex2D( _FX_BraseroFire_Texture_Noise01, panner7 ) ) * 5.0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1920;1;1906;1011;1153.104;214.2616;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;8;-1235.689,107.7108;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-987.8743,108.2568;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;1;FLOAT;0.2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;5;-787.1473,83.25993;Float;True;Property;_FX_BraseroFire_Texture_Noise01;FX_BraseroFire_Texture_Noise01;1;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-771.4045,-197.5368;Float;True;Property;_SM_Eclatos_lambert2_BaseColor;SM_Eclatos_lambert2_BaseColor;0;0;Create;True;0;0;False;0;cbe3486ede21ea7428f60aad62a188ca;cbe3486ede21ea7428f60aad62a188ca;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-73.63496,151.2301;Float;False;Constant;_Float0;Float 0;1;0;Create;True;0;0;False;0;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-295.2593,68.23153;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;104,71.5;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;13;39.26302,-105.918;Float;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;289,25;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_EclatosActif;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;5;True;True;0;False;Transparent;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;7;0;8;0
WireConnection;5;1;7;0
WireConnection;6;0;2;0
WireConnection;6;1;5;0
WireConnection;3;0;6;0
WireConnection;3;1;4;0
WireConnection;0;0;13;0
WireConnection;0;2;3;0
ASEEND*/
//CHKSM=956F873181C1E82D4985E7540FBD540798C1B722