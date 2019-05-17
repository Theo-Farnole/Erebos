// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_BraseroFire"
{
	Properties
	{
		_TextureSample10("Texture Sample 10", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample8("Texture Sample 8", 2D) = "white" {}
		_TextureSample9("Texture Sample 9", 2D) = "white" {}
		_TextureSample7("Texture Sample 7", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
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

		uniform sampler2D _TextureSample10;
		uniform sampler2D _TextureSample9;
		uniform float4 _TextureSample9_ST;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;
		uniform sampler2D _TextureSample7;
		uniform sampler2D _TextureSample8;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample9 = i.uv_texcoord * _TextureSample9_ST.xy + _TextureSample9_ST.zw;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			float2 panner164 = ( 2.0 * _Time.y * float2( 0,-0.5 ) + i.uv_texcoord);
			float temp_output_170_0 = ( i.uv_texcoord.y * 25.0 );
			float4 temp_output_176_0 = ( tex2D( _TextureSample0, uv_TextureSample0 ) * tex2D( _TextureSample7, panner164 ) * temp_output_170_0 );
			float4 lerpResult178 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample9, uv_TextureSample9 ) , temp_output_176_0);
			float2 uv_TexCoord167 = i.uv_texcoord * float2( 0.3,0.3 ) + float2( -0.67,0 );
			float2 panner172 = ( 1.0 * _Time.y * float2( 0,-0.1 ) + uv_TexCoord167);
			float4 lerpResult179 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample8, panner172 ) , temp_output_176_0);
			float4 lerpResult180 = lerp( lerpResult178 , lerpResult179 , temp_output_170_0);
			float4 tex2DNode181 = tex2D( _TextureSample10, lerpResult180.rg );
			float3 temp_cast_3 = (tex2DNode181.r).xxx;
			float3 desaturateInitialColor182 = temp_cast_3;
			float desaturateDot182 = dot( desaturateInitialColor182, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar182 = lerp( desaturateInitialColor182, desaturateDot182.xxx, 0.5 );
			o.Emission = ( desaturateVar182 * 3.0 );
			o.Alpha = 1;
			clip( tex2DNode181.a - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1920;1;1906;1011;5394.163;1637.801;4.610763;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;162;-1615.066,572.0616;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;167;-1626.512,842.9135;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;-0.67,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;164;-1361.503,571.2076;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;166;-1663.004,104.2159;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;1.16,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;165;-1134.948,1088.45;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;174;-1180.286,544.1346;Float;True;Property;_TextureSample7;Texture Sample 7;7;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;-654.1292,1136.835;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;163;-1185.564,318.3253;Float;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;False;0;a61f9d2d1db90804d9398d4fa0fbde90;a61f9d2d1db90804d9398d4fa0fbde90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;172;-1372.949,843.0596;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;173;-1424.005,104.2159;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;175;-1168.857,815.2965;Float;True;Property;_TextureSample8;Texture Sample 8;3;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;177;-1182.158,75.07283;Float;True;Property;_TextureSample9;Texture Sample 9;5;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;-421.4627,527.4267;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;178;-172.4046,66.09285;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;179;-180.5333,780.7245;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;180;242.4053,1083.484;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;181;613.6992,1055.876;Float;True;Property;_TextureSample10;Texture Sample 10;0;0;Create;True;0;0;False;0;c05f3e149852a294dbfa6d10eb5dc929;c05f3e149852a294dbfa6d10eb5dc929;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;182;957.6329,785.4976;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;183;1258.135,862.3444;Float;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;216;-507.213,-896.2245;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;-748.1427,-1149.523;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;214;-1508.838,-1601.875;Float;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;217;-499.0842,-1610.855;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;1444.525,785.5085;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;219;287.0197,-621.0728;Float;True;Property;_TextureSample5;Texture Sample 5;1;0;Create;True;0;0;False;0;c05f3e149852a294dbfa6d10eb5dc929;c05f3e149852a294dbfa6d10eb5dc929;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;220;630.9529,-891.4514;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;213;-1495.537,-861.6525;Float;True;Property;_TextureSample3;Texture Sample 3;4;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;218;-84.27455,-593.4648;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;212;-1750.684,-1572.732;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;222;1117.846,-891.4406;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;210;-1512.244,-1358.623;Float;True;Property;_TextureSample2;Texture Sample 2;10;0;Create;True;0;0;False;0;a61f9d2d1db90804d9398d4fa0fbde90;a61f9d2d1db90804d9398d4fa0fbde90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;209;-980.8092,-540.1139;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;208;-1506.966,-1132.815;Float;True;Property;_TextureSample1;Texture Sample 1;8;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;207;-1461.628,-588.4988;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;206;-1989.683,-1572.732;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;1.16,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;205;-1688.182,-1105.742;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;204;-1953.191,-834.0355;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;-0.67,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;203;-1941.745,-1104.888;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;221;931.4554,-814.6046;Float;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;211;-1699.628,-833.8894;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1764.187,913.7781;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_BraseroFire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;2;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;164;0;162;0
WireConnection;174;1;164;0
WireConnection;170;0;165;2
WireConnection;172;0;167;0
WireConnection;173;0;166;0
WireConnection;175;1;172;0
WireConnection;177;1;173;0
WireConnection;176;0;163;0
WireConnection;176;1;174;0
WireConnection;176;2;170;0
WireConnection;178;0;165;0
WireConnection;178;1;177;0
WireConnection;178;2;176;0
WireConnection;179;0;165;0
WireConnection;179;1;175;0
WireConnection;179;2;176;0
WireConnection;180;0;178;0
WireConnection;180;1;179;0
WireConnection;180;2;170;0
WireConnection;181;1;180;0
WireConnection;182;0;181;1
WireConnection;216;0;207;0
WireConnection;216;1;213;0
WireConnection;216;2;215;0
WireConnection;215;0;210;0
WireConnection;215;1;208;0
WireConnection;215;2;209;0
WireConnection;214;1;212;0
WireConnection;217;0;207;0
WireConnection;217;1;214;0
WireConnection;217;2;215;0
WireConnection;186;0;182;0
WireConnection;186;1;183;0
WireConnection;219;1;218;0
WireConnection;220;0;219;1
WireConnection;213;1;211;0
WireConnection;218;0;217;0
WireConnection;218;1;216;0
WireConnection;218;2;209;0
WireConnection;212;0;206;0
WireConnection;222;0;220;0
WireConnection;222;1;221;0
WireConnection;209;0;207;2
WireConnection;208;1;205;0
WireConnection;205;0;203;0
WireConnection;211;0;204;0
WireConnection;0;2;186;0
WireConnection;0;10;181;4
ASEEND*/
//CHKSM=BC4B5DC32983030877F69CB2E2DA52A8566A09B4