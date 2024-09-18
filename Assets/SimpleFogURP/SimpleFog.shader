// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SimpleFog"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		[HideInInspector]_SunDirection("SunDirection", Vector) = (0,0,0,0)
		[HDR]_SunColor("SunColor", Color) = (1,1,1,0)
		_SunPower("SunPower", Float) = 1
		[HDR]_TopColor("TopColor", Color) = (1,1,1,0)
		_TopPower("TopPower", Float) = 1
		[HDR]_SkyColor("SkyColor", Color) = (1,1,1,0)
		[HDR]_GroundColor("GroundColor", Color) = (1,1,1,0)
		_GroundPower("GroundPower", Float) = 1
		_StartDistance("StartDistance", Float) = 0
		_EndDistance("EndDistance", Float) = 5000
		_FogHeight("FogHeight", Float) = 0
		_FogHeightFade("FogHeightFade", Float) = 0
		_FogHeightPower("FogHeightPower", Float) = 0
		_HeightFogDensity("HeightFogDensity", Float) = 0
		_HeightFollowCamera("HeightFollowCamera", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				float4 ase_texcoord4 : TEXCOORD4;
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _GroundPower;
			uniform float4 _GroundColor;
			uniform float _TopPower;
			uniform float4 _TopColor;
			uniform float4 _SkyColor;
			uniform float3 _SunDirection;
			uniform float _SunPower;
			uniform float4 _SunColor;
			uniform float _StartDistance;
			uniform float _EndDistance;
			uniform float _FogHeight;
			uniform float _HeightFollowCamera;
			uniform float _FogHeightFade;
			uniform float _FogHeightPower;
			uniform float _HeightFogDensity;
			float2 UnStereo( float2 UV )
			{
				#if UNITY_SINGLE_PASS_STEREO
				float4 scaleOffset = unity_StereoScaleOffset[ unity_StereoEyeIndex ];
				UV.xy = (UV.xy - scaleOffset.zw) / scaleOffset.xy;
				#endif
				return UV;
			}
			
			float3 InvertDepthDir72_g1( float3 In )
			{
				float3 result = In;
				#if !defined(ASE_SRP_VERSION) || ASE_SRP_VERSION <= 70301
				result *= float3(1,1,-1);
				#endif
				return result;
			}
			


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				float4 ase_clipPos = UnityObjectToClipPos(v.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				o.ase_texcoord4 = screenPos;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv_MainTex = i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 screenPos = i.ase_texcoord4;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float2 UV22_g3 = ase_screenPosNorm.xy;
				float2 localUnStereo22_g3 = UnStereo( UV22_g3 );
				float2 break64_g1 = localUnStereo22_g3;
				float clampDepth69_g1 = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy );
				#ifdef UNITY_REVERSED_Z
				float staticSwitch38_g1 = ( 1.0 - clampDepth69_g1 );
				#else
				float staticSwitch38_g1 = clampDepth69_g1;
				#endif
				float3 appendResult39_g1 = (float3(break64_g1.x , break64_g1.y , staticSwitch38_g1));
				float4 appendResult42_g1 = (float4((appendResult39_g1*2.0 + -1.0) , 1.0));
				float4 temp_output_43_0_g1 = mul( unity_CameraInvProjection, appendResult42_g1 );
				float3 temp_output_46_0_g1 = ( (temp_output_43_0_g1).xyz / (temp_output_43_0_g1).w );
				float3 In72_g1 = temp_output_46_0_g1;
				float3 localInvertDepthDir72_g1 = InvertDepthDir72_g1( In72_g1 );
				float4 appendResult49_g1 = (float4(localInvertDepthDir72_g1 , 1.0));
				float4 temp_output_13_0 = mul( unity_CameraToWorld, appendResult49_g1 );
				float4 normalizeResult24 = normalize( ( float4( _WorldSpaceCameraPos , 0.0 ) - temp_output_13_0 ) );
				float4 WorldDirection60 = normalizeResult24;
				float dotResult64 = dot( (WorldDirection60).xyz , float3(0,-1,0) );
				float clampResult67 = clamp( dotResult64 , 0.0 , 1.0 );
				float saferPower59 = max( ( 1.0 - clampResult67 ) , 0.0001 );
				float temp_output_59_0 = pow( saferPower59 , _GroundPower );
				float dotResult75 = dot( (WorldDirection60).xyz , float3(0,-1,0) );
				float clampResult76 = clamp( dotResult75 , 0.0 , 1.0 );
				float saferPower79 = max( clampResult76 , 0.0001 );
				float temp_output_79_0 = pow( saferPower79 , _TopPower );
				float clampResult88 = clamp( ( 1.0 - temp_output_59_0 ) , 0.0 , 1.0 );
				float clampResult87 = clamp( ( clampResult88 - temp_output_79_0 ) , 0.0 , 1.0 );
				float4 SkyGradient48 = ( ( temp_output_59_0 * _GroundColor ) + ( temp_output_79_0 * _TopColor ) + ( clampResult87 * _SkyColor ) );
				float dotResult19 = dot( float4( _SunDirection , 0.0 ) , WorldDirection60 );
				float clampResult71 = clamp( (0.0 + (dotResult19 - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) , 0.0 , 1.0 );
				float temp_output_27_0 = pow( clampResult71 , _SunPower );
				float4 SunGradient45 = ( temp_output_27_0 * _SunColor );
				float SunStrength51 = temp_output_27_0;
				float eyeDepth30 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float clampResult40 = clamp( (0.0 + (eyeDepth30 - _StartDistance) * (1.0 - 0.0) / (_EndDistance - _StartDistance)) , 0.0 , 1.0 );
				float DistanceFogDensity41 = clampResult40;
				float4 WorldPos89 = temp_output_13_0;
				float temp_output_123_0 = (WorldPos89).y;
				float RayHighPoint125 = max( _WorldSpaceCameraPos.y , temp_output_123_0 );
				float lerpResult214 = lerp( _FogHeight , ( _WorldSpaceCameraPos.y + _FogHeight ) , _HeightFollowCamera);
				float FogLowPoint132 = lerpResult214;
				float FogHighPoint131 = ( _FogHeightFade + lerpResult214 );
				float clampResult195 = clamp( RayHighPoint125 , FogLowPoint132 , FogHighPoint131 );
				float RayLowPoint127 = min( _WorldSpaceCameraPos.y , temp_output_123_0 );
				float clampResult196 = clamp( RayLowPoint127 , FogLowPoint132 , FogHighPoint131 );
				float FogHeightFade156 = _FogHeightFade;
				float clampResult211 = clamp( pow( ( 1.0 - ( ( ( ( clampResult195 + clampResult196 ) * 0.5 ) - FogLowPoint132 ) / FogHeightFade156 ) ) , _FogHeightPower ) , 0.0 , 1.0 );
				float4 temp_output_134_0 = ( float4( _WorldSpaceCameraPos , 0.0 ) - WorldPos89 );
				float RayHeight138 = abs( (temp_output_134_0).y );
				float RayLength139 = length( temp_output_134_0 );
				float clampResult184 = clamp( ( ( ( clampResult211 * ( ( clampResult195 - clampResult196 ) / RayHeight138 ) ) + ( ( min( RayHighPoint125 , FogLowPoint132 ) - min( RayLowPoint127 , FogLowPoint132 ) ) / RayHeight138 ) ) * _HeightFogDensity * ( RayLength139 - _StartDistance ) ) , 0.0 , 1.0 );
				float HeightFogDensity98 = clampResult184;
				float clampResult217 = clamp( ( DistanceFogDensity41 + HeightFogDensity98 ) , 0.0 , 1.0 );
				float4 lerpResult34 = lerp( tex2D( _MainTex, uv_MainTex ) , ( SkyGradient48 + ( SunGradient45 * SunStrength51 ) ) , clampResult217);
				

				finalColor = lerpResult34;

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
8;81;2543;1272;2472.882;1296.014;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;62;-4758.894,-715.2978;Inherit;False;1021.844;517.4996;WorldDirection;6;21;20;24;60;13;89;WorldPosition&Direction;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;13;-4717.553,-647.9789;Inherit;True;Reconstruct World Position From Depth;-1;;1;e7094bcbcc80eb140b2a3dbe6a861de8;0;0;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;129;-5614.878,-2696.158;Inherit;False;1170.931;1056.731;Comment;22;91;214;213;134;106;139;140;138;137;208;156;125;131;132;127;133;128;126;123;92;122;215;CalculateRayValues;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-5513.298,-1986.475;Inherit;False;Property;_FogHeight;FogHeight;10;0;Create;True;0;0;0;False;0;False;0;2.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;89;-4241.166,-650.849;Inherit;False;WorldPos;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;106;-5537.993,-2656.756;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;213;-5385.239,-1826.291;Inherit;False;Property;_HeightFollowCamera;HeightFollowCamera;14;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;215;-5227.593,-1940.261;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;122;-5370.374,-2210.655;Inherit;False;89;WorldPos;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;123;-5188.838,-2218.447;Inherit;False;FLOAT;1;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;92;-5015.741,-2080.709;Inherit;False;Property;_FogHeightFade;FogHeightFade;11;0;Create;True;0;0;0;False;0;False;0;65.78;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;214;-5041.239,-1963.291;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;133;-4812.901,-2080.014;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;128;-4845.513,-2238.168;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceCameraPos;21;-4629.193,-420.634;Inherit;True;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMaxOpNode;126;-4853.514,-2365.168;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-4687.913,-2235.669;Inherit;False;RayLowPoint;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;-4677.94,-2379.646;Inherit;False;RayHighPoint;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;131;-4671.901,-2089.014;Inherit;False;FogHighPoint;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;132;-4665.901,-1964.014;Inherit;False;FogLowPoint;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;212;-4037.017,-2782.627;Inherit;False;3853.042;1167.451;Comment;35;130;141;142;143;195;196;198;181;154;155;157;158;99;159;172;170;163;162;179;197;210;165;151;211;144;166;161;164;160;182;167;105;183;184;98;HeightFogDensity;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;20;-4283.463,-512.6335;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;142;-3975.312,-2618.066;Inherit;False;127;RayLowPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;141;-3929.256,-2365.77;Inherit;False;131;FogHighPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;24;-4123.463,-512.4227;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;-3987.017,-2723.598;Inherit;False;125;RayHighPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;143;-3947.572,-2471.171;Inherit;False;132;FogLowPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;196;-3612.718,-2581.102;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;50;-4676.274,164.8201;Inherit;False;3076.729;1267.94;SkyGradient;27;83;48;86;85;64;69;57;59;58;68;67;49;65;66;87;88;84;63;80;79;82;76;78;75;74;73;72;SkyGradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;60;-3938.829,-518.7578;Inherit;False;WorldDirection;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;195;-3612.701,-2711.972;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-4343.563,280.8444;Inherit;False;60;WorldDirection;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;198;-3346.573,-2731.579;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;181;-3231.398,-2663.501;Inherit;False;Constant;_Float1;Float 1;14;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;134;-5149.528,-2635.616;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;66;-4080.53,281.2338;Inherit;False;FLOAT3;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;154;-3024.618,-2732.627;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;65;-4111.531,425.2334;Inherit;False;Constant;_Vector0;Vector 0;8;0;Create;True;0;0;0;False;0;False;0,-1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;156;-4666.718,-1835.134;Inherit;False;FogHeightFade;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;64;-3849.39,287.0454;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;208;-5002.21,-2625.894;Inherit;False;FLOAT;1;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-2929.438,-2584.48;Inherit;False;156;FogHeightFade;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;155;-2847.318,-2714.326;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;67;-3678.109,286.8546;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;72;-4366.29,658.5195;Inherit;False;60;WorldDirection;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.AbsOpNode;137;-4852.254,-2637.149;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;158;-2662.438,-2698.48;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;73;-4125.599,831.0529;Inherit;False;Constant;_Vector1;Vector 1;8;0;Create;True;0;0;0;False;0;False;0,-1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SwizzleNode;74;-4083.773,658.909;Inherit;False;FLOAT3;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-3557.717,426.7679;Inherit;False;Property;_GroundPower;GroundPower;7;0;Create;True;0;0;0;False;0;False;1;5.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;68;-3505.089,286.7868;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;138;-4688.159,-2623.046;Inherit;False;RayHeight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;172;-3804.73,-1930.113;Inherit;False;127;RayLowPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;162;-3794.691,-2189.841;Inherit;False;125;RayHighPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;163;-3800.927,-2088.113;Inherit;False;132;FogLowPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;170;-3795.73,-1779.062;Inherit;False;132;FogLowPoint;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;159;-2523.214,-2688.922;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;99;-2565.233,-2576.144;Inherit;False;Property;_FogHeightPower;FogHeightPower;12;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;44;-3510.012,-759.7549;Inherit;False;1836.955;647.3507;SunGradient;11;25;45;51;47;27;15;29;26;19;61;71;SunGradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMinOpNode;179;-3517.007,-1969.574;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;61;-3201.9,-325.281;Inherit;False;60;WorldDirection;1;0;OBJECT;;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;25;-3217.385,-620.6597;Inherit;False;Property;_SunDirection;SunDirection;0;1;[HideInInspector];Create;True;0;0;0;False;0;False;0,0,0;-0.2303274,-0.9464241,-0.2263421;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;151;-3241.352,-2335.221;Inherit;False;138;RayHeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;75;-3850.457,664.6991;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;165;-3533.277,-2156.129;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;197;-3223.074,-2441.677;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;210;-2338.326,-2693.406;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;59;-3310.001,286.5849;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LengthOpNode;140;-4867.648,-2491.185;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;211;-2112.953,-2694.144;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;161;-3351.676,-2136.533;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;166;-3326.137,-1930.402;Inherit;False;138;RayHeight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;144;-2845.685,-2474.312;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;19;-2872.984,-616.0229;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-3667.44,823.2094;Inherit;False;Property;_TopPower;TopPower;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;76;-3684.177,664.6733;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;139;-4649.648,-2496.185;Inherit;False;RayLength;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;84;-3058.401,948.8511;Inherit;False;2;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;43;-2976.195,-1265.688;Inherit;False;1303.852;372.0544;DistanceFogDensity;7;31;41;40;39;36;35;30;DistanceFogDensity;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;167;-1895.656,-2117.529;Inherit;False;139;RayLength;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;31;-2924.195,-1171.178;Float;False;0;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;164;-3070.844,-2069.457;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;88;-2897.656,948.7665;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;79;-3323.069,659.4465;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;-1526.079,-2719.62;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-2620.153,-1086.688;Inherit;False;Property;_StartDistance;StartDistance;8;0;Create;True;0;0;0;False;0;False;0;45;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;26;-2727.211,-616.3213;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;85;-2698.383,952.3085;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;71;-2530.247,-615.6372;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-2633.51,-348.692;Inherit;False;Property;_SunPower;SunPower;2;0;Create;True;0;0;0;False;0;False;1;2.91;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-2613.542,-1000.182;Inherit;False;Property;_EndDistance;EndDistance;9;0;Create;True;0;0;0;False;0;False;5000;500;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-1529.786,-2472.935;Inherit;False;Property;_HeightFogDensity;HeightFogDensity;13;0;Create;True;0;0;0;False;0;False;0;0.005;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;30;-2630.221,-1171.718;Inherit;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;218;-1661.043,-2052.917;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;182;-1336.771,-2695.079;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;27;-2371.553,-615.8909;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;-2396.25,-384.1873;Inherit;False;Property;_SunColor;SunColor;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;2.639016,1.604108,1.210842,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;82;-2859.471,738.8502;Inherit;False;Property;_TopColor;TopColor;3;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.01201493,0.1585369,0.8490566,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;87;-2512.671,951.3645;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;-2601.632,1152.285;Inherit;False;Property;_SkyColor;SkyColor;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;39;-2339.173,-1167.113;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;57;-3010.28,400.559;Inherit;False;Property;_GroundColor;GroundColor;6;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.4164987,1.17439,1.741101,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-1152.7,-2537.705;Inherit;False;3;3;0;FLOAT;1;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;80;-2615.921,659.9992;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-2777.371,284.5194;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;86;-2315.869,1008.377;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;184;-795.7844,-2590.318;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;40;-2124.111,-1167.195;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-2157.057,-614.2186;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-1994.884,-618.9784;Inherit;False;SunGradient;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;51;-1991.558,-424.6313;Inherit;False;SunStrength;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;83;-2134.802,625.9522;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;41;-1936.377,-1171.923;Inherit;False;DistanceFogDensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;98;-424.9752,-2443.091;Inherit;False;HeightFogDensity;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;42;-845.1317,-523.4536;Inherit;False;41;DistanceFogDensity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;-815.4231,-385.655;Inherit;False;98;HeightFogDensity;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;48;-1843.084,619.1758;Inherit;False;SkyGradient;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;46;-1249.295,-606.2907;Inherit;False;45;SunGradient;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;53;-1331.898,-504.4836;Inherit;False;51;SunStrength;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;55;-1276.959,-688.1982;Inherit;False;48;SkyGradient;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;220;-1082.882,-581.0139;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;216;-588.542,-602.6611;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;9;-1160.759,-986.834;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-946.2589,-990.9341;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;219;-965.8823,-686.0139;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;217;-416.3487,-761.4719;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;-263.5182,-994.1748;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;103;-562.4231,-459.655;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;54;-734.6649,-672.2324;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;8;52.32371,-1000.39;Float;False;True;-1;2;ASEMaterialInspector;0;4;SimpleFog;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;89;0;13;0
WireConnection;215;0;106;2
WireConnection;215;1;91;0
WireConnection;123;0;122;0
WireConnection;214;0;91;0
WireConnection;214;1;215;0
WireConnection;214;2;213;0
WireConnection;133;0;92;0
WireConnection;133;1;214;0
WireConnection;128;0;106;2
WireConnection;128;1;123;0
WireConnection;126;0;106;2
WireConnection;126;1;123;0
WireConnection;127;0;128;0
WireConnection;125;0;126;0
WireConnection;131;0;133;0
WireConnection;132;0;214;0
WireConnection;20;0;21;0
WireConnection;20;1;13;0
WireConnection;24;0;20;0
WireConnection;196;0;142;0
WireConnection;196;1;143;0
WireConnection;196;2;141;0
WireConnection;60;0;24;0
WireConnection;195;0;130;0
WireConnection;195;1;143;0
WireConnection;195;2;141;0
WireConnection;198;0;195;0
WireConnection;198;1;196;0
WireConnection;134;0;106;0
WireConnection;134;1;122;0
WireConnection;66;0;63;0
WireConnection;154;0;198;0
WireConnection;154;1;181;0
WireConnection;156;0;92;0
WireConnection;64;0;66;0
WireConnection;64;1;65;0
WireConnection;208;0;134;0
WireConnection;155;0;154;0
WireConnection;155;1;143;0
WireConnection;67;0;64;0
WireConnection;137;0;208;0
WireConnection;158;0;155;0
WireConnection;158;1;157;0
WireConnection;74;0;72;0
WireConnection;68;0;67;0
WireConnection;138;0;137;0
WireConnection;159;0;158;0
WireConnection;179;0;172;0
WireConnection;179;1;170;0
WireConnection;75;0;74;0
WireConnection;75;1;73;0
WireConnection;165;0;162;0
WireConnection;165;1;163;0
WireConnection;197;0;195;0
WireConnection;197;1;196;0
WireConnection;210;0;159;0
WireConnection;210;1;99;0
WireConnection;59;0;68;0
WireConnection;59;1;58;0
WireConnection;140;0;134;0
WireConnection;211;0;210;0
WireConnection;161;0;165;0
WireConnection;161;1;179;0
WireConnection;144;0;197;0
WireConnection;144;1;151;0
WireConnection;19;0;25;0
WireConnection;19;1;61;0
WireConnection;76;0;75;0
WireConnection;139;0;140;0
WireConnection;84;1;59;0
WireConnection;164;0;161;0
WireConnection;164;1;166;0
WireConnection;88;0;84;0
WireConnection;79;0;76;0
WireConnection;79;1;78;0
WireConnection;160;0;211;0
WireConnection;160;1;144;0
WireConnection;26;0;19;0
WireConnection;85;0;88;0
WireConnection;85;1;79;0
WireConnection;71;0;26;0
WireConnection;30;0;31;0
WireConnection;218;0;167;0
WireConnection;218;1;35;0
WireConnection;182;0;160;0
WireConnection;182;1;164;0
WireConnection;27;0;71;0
WireConnection;27;1;29;0
WireConnection;87;0;85;0
WireConnection;39;0;30;0
WireConnection;39;1;35;0
WireConnection;39;2;36;0
WireConnection;183;0;182;0
WireConnection;183;1;105;0
WireConnection;183;2;218;0
WireConnection;80;0;79;0
WireConnection;80;1;82;0
WireConnection;69;0;59;0
WireConnection;69;1;57;0
WireConnection;86;0;87;0
WireConnection;86;1;49;0
WireConnection;184;0;183;0
WireConnection;40;0;39;0
WireConnection;47;0;27;0
WireConnection;47;1;15;0
WireConnection;45;0;47;0
WireConnection;51;0;27;0
WireConnection;83;0;69;0
WireConnection;83;1;80;0
WireConnection;83;2;86;0
WireConnection;41;0;40;0
WireConnection;98;0;184;0
WireConnection;48;0;83;0
WireConnection;220;0;46;0
WireConnection;220;1;53;0
WireConnection;216;0;42;0
WireConnection;216;1;102;0
WireConnection;11;0;9;0
WireConnection;219;0;55;0
WireConnection;219;1;220;0
WireConnection;217;0;216;0
WireConnection;34;0;11;0
WireConnection;34;1;219;0
WireConnection;34;2;217;0
WireConnection;103;1;102;0
WireConnection;54;0;55;0
WireConnection;54;1;46;0
WireConnection;54;2;53;0
WireConnection;8;0;34;0
ASEEND*/
//CHKSM=C69C9C4ED995B6F153E1968869A19148852E38B6