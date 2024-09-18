Shader "Custom/Shader_UI_BackGround"
{
    Properties
    {
        //_IconColor("IconColor", Color) = (0, 0, 0, 0)
        //_BackgroundColor("BackgroundColor", Color) = (0, 0, 0, 0)
        _IconScale("IconScale", Int) = 1
        _RotateAngle("IconRotateAngle", Int) = 45
        _MoveSpeed("MoveSpeed", Float) = 1
        _Vignette("Vignette", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry+0"
        }
        
        Pass
        {
            Name "Pass"
            Tags 
            { 
                // LightMode: <None>
            }
            
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest off
            ZWrite On
            // ColorMask: <None>
            
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Debug
            // <None>
            
            // --------------------------------------------------
            // Pass
            
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>
            
            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define VARYINGS_NEED_POSITION_WS 
            #define SHADERPASS_UNLIT
            
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
            
            // --------------------------------------------------
            // Graph
            
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float4 _IconColor;
            float4 _BackgroundColor;
            float _IconScale;
            float _RotateAngle;
            float _MoveSpeed;
            float _Vignette;
            CBUFFER_END
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex); float4 _MainTex_TexelSize;
            SAMPLER(_SampleTexture2D_DB550E50_Sampler_3_Linear_Repeat);
            
            // Graph Functions
            
            void Unity_Length_float4(float4 In, out float Out)
            {
                Out = length(In);
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
            {
                Out = clamp(In, Min, Max);
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Add_float4(float4 A, float4 B, out float4 Out)
            {
                Out = A + B;
            }
            
            void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
            {
                Out = A * B;
            }
            
            void Unity_Fraction_float4(float4 In, out float4 Out)
            {
                Out = frac(In);
            }
            
            void Unity_Rotate_Degrees_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                //rotation matrix
                Rotation = Rotation * (3.1415926f/180.0f);
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
                
                //center rotation matrix
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix*2 - 1;
                
                //multiply the UVs by the rotation matrix
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
                
                Out = UV;
            }
            
            void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
            {
                Out = lerp(A, B, T);
            }
            
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float3 WorldSpacePosition;
                float4 ScreenPosition;
                float3 TimeParameters;
            };
            
            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _ScreenPosition_4429C55F_Out_0 = float4(IN.ScreenPosition.xy / IN.ScreenPosition.w * 2 - 1, 0, 0);
                float _Length_44514883_Out_1;
                Unity_Length_float4(_ScreenPosition_4429C55F_Out_0, _Length_44514883_Out_1);
                float _Property_9123B41C_Out_0 = _Vignette;
                float _Add_CD17C6E9_Out_2;
                Unity_Add_float(_Length_44514883_Out_1, _Property_9123B41C_Out_0, _Add_CD17C6E9_Out_2);
                float _Subtract_19A7D309_Out_2;
                Unity_Subtract_float(2, _Add_CD17C6E9_Out_2, _Subtract_19A7D309_Out_2);
                float _Clamp_6004D0C5_Out_3;
                Unity_Clamp_float(_Subtract_19A7D309_Out_2, 0, 1, _Clamp_6004D0C5_Out_3);
                float4 _Property_75C170B0_Out_0 = _BackgroundColor;
                float4 _Property_82FCDC3F_Out_0 = _IconColor;
                float _Property_D208D658_Out_0 = _IconScale;
                float4 _ScreenPosition_8907C72A_Out_0 = frac(float4((IN.ScreenPosition.x / IN.ScreenPosition.w * 2 - 1) * _ScreenParams.x / _ScreenParams.y, IN.ScreenPosition.y / IN.ScreenPosition.w * 2 - 1, 0, 0));
                float _Property_41F8BAE_Out_0 = _MoveSpeed;
                float _Multiply_A22D15A_Out_2;
                Unity_Multiply_float(_Property_41F8BAE_Out_0, IN.TimeParameters.x, _Multiply_A22D15A_Out_2);
                float4 _Add_5D8FD3EE_Out_2;
                Unity_Add_float4(_ScreenPosition_8907C72A_Out_0, (_Multiply_A22D15A_Out_2.xxxx), _Add_5D8FD3EE_Out_2);
                float4 _Multiply_9745DD9B_Out_2;
                Unity_Multiply_float((_Property_D208D658_Out_0.xxxx), _Add_5D8FD3EE_Out_2, _Multiply_9745DD9B_Out_2);
                float4 _Fraction_5A059CA_Out_1;
                Unity_Fraction_float4(_Multiply_9745DD9B_Out_2, _Fraction_5A059CA_Out_1);
                float _Property_5FD1B0BF_Out_0 = _RotateAngle;
                float2 _Rotate_6366FBD1_Out_3;
                Unity_Rotate_Degrees_float((_Fraction_5A059CA_Out_1.xy), float2 (0.5, 0.5), _Property_5FD1B0BF_Out_0, _Rotate_6366FBD1_Out_3);
                float4 _SampleTexture2D_DB550E50_RGBA_0 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, _Rotate_6366FBD1_Out_3);
                float _SampleTexture2D_DB550E50_R_4 = _SampleTexture2D_DB550E50_RGBA_0.r;
                float _SampleTexture2D_DB550E50_G_5 = _SampleTexture2D_DB550E50_RGBA_0.g;
                float _SampleTexture2D_DB550E50_B_6 = _SampleTexture2D_DB550E50_RGBA_0.b;
                float _SampleTexture2D_DB550E50_A_7 = _SampleTexture2D_DB550E50_RGBA_0.a;
                float4 _Multiply_90B39360_Out_2;
                Unity_Multiply_float(_Property_82FCDC3F_Out_0, _SampleTexture2D_DB550E50_RGBA_0, _Multiply_90B39360_Out_2);
                float4 _Lerp_DBFFB64B_Out_3;
                Unity_Lerp_float4(_Property_75C170B0_Out_0, _Multiply_90B39360_Out_2, (_SampleTexture2D_DB550E50_A_7.xxxx), _Lerp_DBFFB64B_Out_3);
                float4 _Multiply_957A7451_Out_2;
                Unity_Multiply_float((_Clamp_6004D0C5_Out_3.xxxx), _Lerp_DBFFB64B_Out_3, _Multiply_957A7451_Out_2);
                surface.Color = (_Multiply_957A7451_Out_2.xyz);
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0;
                return surface;
            }
            
            // --------------------------------------------------
            // Structs and Packing
            
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
            
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_POSITION;
                #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                float3 interp00 : TEXCOORD0;
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output = (PackedVaryings)0;
                output.positionCS = input.positionCS;
                output.interp00.xyz = input.positionWS;
                #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output = (Varyings)0;
                output.positionCS = input.positionCS;
                output.positionWS = input.interp00.xyz;
                #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                #endif
                return output;
            }
            
            // --------------------------------------------------
            // Build Graph Inputs
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                output.WorldSpacePosition =          input.positionWS;
                output.ScreenPosition =              ComputeScreenPos(TransformWorldToHClip(input.positionWS), _ProjectionParams.x);
                output.TimeParameters =              _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                    #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                return output;
            }
            
            
            // --------------------------------------------------
            // Main
            
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
            
            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
