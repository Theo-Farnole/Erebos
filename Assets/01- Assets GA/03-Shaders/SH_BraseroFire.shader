// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Erebos/SH_BraseroFire"
{
	Properties
	{
		_TextureSample17("Texture Sample 17", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_TextureSample15("Texture Sample 15", 2D) = "white" {}
		_TextureSample18("Texture Sample 18", 2D) = "white" {}
		_TextureSample16("Texture Sample 16", 2D) = "white" {}
		_TextureSample14("Texture Sample 14", 2D) = "white" {}
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

		uniform sampler2D _TextureSample17;
		uniform sampler2D _TextureSample18;
		uniform sampler2D _TextureSample14;
		uniform float4 _TextureSample14_ST;
		uniform sampler2D _TextureSample16;
		uniform sampler2D _TextureSample15;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord322 = i.uv_texcoord * float2( 0.3,0.3 ) + float2( 1.16,1 );
			float2 panner321 = ( 2.0 * _Time.y * float2( 0,-0.1 ) + uv_TexCoord322);
			float2 uv_TextureSample14 = i.uv_texcoord * _TextureSample14_ST.xy + _TextureSample14_ST.zw;
			float2 uv_TexCoord307 = i.uv_texcoord * float2( 0.5,0.5 );
			float2 panner319 = ( 1.0 * _Time.y * float2( 0,-0.5 ) + uv_TexCoord307);
			float temp_output_323_0 = ( i.uv_texcoord.y * 25.0 );
			float4 temp_output_308_0 = ( tex2D( _TextureSample14, uv_TextureSample14 ) * tex2D( _TextureSample16, panner319 ) * temp_output_323_0 );
			float4 lerpResult312 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample18, panner321 ) , temp_output_308_0);
			float2 uv_TexCoord317 = i.uv_texcoord * float2( 0.2,0.2 ) + float2( -0.67,0 );
			float2 panner324 = ( 1.5 * _Time.y * float2( 0,-0.1 ) + uv_TexCoord317);
			float4 lerpResult318 = lerp( float4( i.uv_texcoord, 0.0 , 0.0 ) , tex2D( _TextureSample15, panner324 ) , temp_output_308_0);
			float4 lerpResult315 = lerp( lerpResult312 , lerpResult318 , temp_output_323_0);
			float4 tex2DNode320 = tex2D( _TextureSample17, lerpResult315.rg );
			float3 temp_cast_3 = (tex2DNode320.r).xxx;
			float3 desaturateInitialColor314 = temp_cast_3;
			float desaturateDot314 = dot( desaturateInitialColor314, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar314 = lerp( desaturateInitialColor314, desaturateDot314.xxx, 0.5 );
			o.Emission = ( desaturateVar314 * 3.0 );
			o.Alpha = 1;
			clip( tex2DNode320.a - _Cutoff );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
1962;52;1906;950;2293.101;-461.1387;1;True;False
Node;AmplifyShaderEditor.TextureCoordinatesNode;307;-1902.513,1079.415;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;319;-1648.951,1078.561;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;322;-1950.451,611.5703;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;1.16,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;316;-1422.396,1595.804;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;317;-1913.959,1350.267;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.2;False;1;FLOAT2;-0.67,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;323;-941.5776,1644.189;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;309;-1473.012,825.6795;Float;True;Property;_TextureSample14;Texture Sample 14;13;0;Create;True;0;0;False;0;a61f9d2d1db90804d9398d4fa0fbde90;a61f9d2d1db90804d9398d4fa0fbde90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;311;-1467.734,1051.488;Float;True;Property;_TextureSample16;Texture Sample 16;11;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;324;-1660.396,1350.413;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;321;-1711.453,611.5703;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;308;-708.9109,1034.78;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;310;-1456.306,1322.65;Float;True;Property;_TextureSample15;Texture Sample 15;6;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;325;-1469.606,582.4283;Float;True;Property;_TextureSample18;Texture Sample 18;8;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;312;-459.8523,573.4483;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;318;-467.9811,1288.079;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;315;-45.04229,1590.838;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;320;326.2513,1563.23;Float;True;Property;_TextureSample17;Texture Sample 17;2;0;Create;True;0;0;False;0;c05f3e149852a294dbfa6d10eb5dc929;c05f3e149852a294dbfa6d10eb5dc929;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;314;670.184,1292.852;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;313;970.6866,1369.698;Float;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;251;-1688.75,-1558.648;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.1,0.1;False;1;FLOAT2;1.16,1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;264;587.9538,-606.9876;Float;True;Property;_TextureSample5;Texture Sample 5;1;0;Create;True;0;0;False;0;c05f3e149852a294dbfa6d10eb5dc929;c05f3e149852a294dbfa6d10eb5dc929;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;256;-1449.751,-1558.648;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;2;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;260;-1207.904,-1587.79;Float;True;Property;_TextureSample4;Texture Sample 4;7;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;294;-699.2939,2578.903;Float;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;326;1157.079,1292.863;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;252;-1387.249,-1091.657;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;263;216.6599,-579.3797;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;249;-1652.258,-819.9505;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.2,0.2;False;1;FLOAT2;-0.67,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;255;-1206.032,-1118.73;Float;True;Property;_TextureSample2;Texture Sample 2;10;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;293;-1229.024,3118.966;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;267;1418.78,-877.3553;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;296;-983.3383,3384.16;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;254;-1398.695,-819.8045;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.1;False;1;FLOAT;1.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;257;-679.8752,-526.0286;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;25;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;298;-444.8997,3090.727;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;261;-206.2788,-882.1392;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;262;-198.15,-1596.77;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;290;-1203.592,2849.444;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;248;-1640.812,-1090.803;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.3,0.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;265;931.8862,-877.3661;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;289;-1457.155,2850.298;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;299;-450.8997,2825.727;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;300;-131.7719,2971.664;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;250;-1160.694,-574.4136;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;301;171.9428,3406.628;Float;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT2;0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;303;1153.278,3186.695;Float;False;Constant;_Float2;Float 2;9;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;305;1339.669,3109.859;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DesaturateOpNode;304;852.7756,3109.849;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;295;-1024.932,3091.203;Float;True;Property;_TextureSample8;Texture Sample 8;4;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;306;-851.2602,3609.198;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.02,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;259;-1194.604,-847.5674;Float;True;Property;_TextureSample3;Texture Sample 3;5;0;Create;True;0;0;False;0;db1fe8065d9aeed469449be79e8c3124;db1fe8065d9aeed469449be79e8c3124;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;297;-583.1462,3574.355;Float;True;Property;_TextureSample10;Texture Sample 10;14;0;Create;True;0;0;False;0;8653b32da6e95304ba3f5fe45455ec8e;8653b32da6e95304ba3f5fe45455ec8e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;266;1232.388,-800.5195;Float;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;253;-1211.31,-1344.539;Float;True;Property;_TextureSample1;Texture Sample 1;12;0;Create;True;0;0;False;0;a61f9d2d1db90804d9398d4fa0fbde90;a61f9d2d1db90804d9398d4fa0fbde90;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;291;-1482.587,3118.82;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.5,0.5;False;1;FLOAT2;-0.67,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;258;-447.2086,-1135.438;Float;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;292;-1022.374,2822.372;Float;True;Property;_TextureSample6;Texture Sample 6;9;0;Create;True;0;0;False;0;f16a5ad4e72c5194595a6d5c79262b49;f16a5ad4e72c5194595a6d5c79262b49;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;302;508.8428,3380.227;Float;True;Property;_TextureSample11;Texture Sample 11;0;0;Create;True;0;0;False;0;c05f3e149852a294dbfa6d10eb5dc929;c05f3e149852a294dbfa6d10eb5dc929;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1556.981,1431.968;Float;False;True;6;Float;ASEMaterialInspector;0;0;Standard;Erebos/SH_BraseroFire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;TransparentCutout;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;3;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;319;0;307;0
WireConnection;323;0;316;2
WireConnection;311;1;319;0
WireConnection;324;0;317;0
WireConnection;321;0;322;0
WireConnection;308;0;309;0
WireConnection;308;1;311;0
WireConnection;308;2;323;0
WireConnection;310;1;324;0
WireConnection;325;1;321;0
WireConnection;312;0;316;0
WireConnection;312;1;325;0
WireConnection;312;2;308;0
WireConnection;318;0;316;0
WireConnection;318;1;310;0
WireConnection;318;2;308;0
WireConnection;315;0;312;0
WireConnection;315;1;318;0
WireConnection;315;2;323;0
WireConnection;320;1;315;0
WireConnection;314;0;320;1
WireConnection;264;1;263;0
WireConnection;256;0;251;0
WireConnection;260;1;256;0
WireConnection;294;0;292;0
WireConnection;326;0;314;0
WireConnection;326;1;313;0
WireConnection;252;0;248;0
WireConnection;263;0;262;0
WireConnection;263;1;261;0
WireConnection;263;2;257;0
WireConnection;255;1;252;0
WireConnection;293;0;291;0
WireConnection;267;0;265;0
WireConnection;267;1;266;0
WireConnection;254;0;249;0
WireConnection;257;0;250;2
WireConnection;298;0;295;0
WireConnection;298;1;296;0
WireConnection;298;2;297;0
WireConnection;261;0;250;0
WireConnection;261;1;259;0
WireConnection;261;2;258;0
WireConnection;262;0;250;0
WireConnection;262;1;260;0
WireConnection;262;2;258;0
WireConnection;290;0;289;0
WireConnection;265;0;264;1
WireConnection;299;0;294;0
WireConnection;299;1;296;0
WireConnection;299;2;297;0
WireConnection;300;0;299;0
WireConnection;300;1;298;0
WireConnection;301;0;300;0
WireConnection;301;1;296;0
WireConnection;301;2;297;0
WireConnection;305;0;304;0
WireConnection;305;1;303;0
WireConnection;304;0;302;1
WireConnection;295;1;293;0
WireConnection;259;1;254;0
WireConnection;297;1;306;0
WireConnection;258;0;253;0
WireConnection;258;1;255;0
WireConnection;258;2;257;0
WireConnection;292;1;290;0
WireConnection;302;1;301;0
WireConnection;0;2;326;0
WireConnection;0;10;320;4
ASEEND*/
//CHKSM=E6B2D3C3DF882439174181D2C625AA76C85C6869