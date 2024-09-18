Shader "Katana/Particles"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {

        pass
        {
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma prefer_hlslcc gles
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


            
            CBUFFER_START(UnityPerMaterial)
            half4 _Color;
            CBUFFER_END
            

            struct appdata
            {
                float4 vertex: POSITION;
                half4 color:COLOR;
            };
            struct v2f
            {
                float4 pos: SV_POSITION;
                half4 color:COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i):SV_TARGET
            {
                return i.color * _Color;// * i.color;
            }
            
            ENDHLSL
            
        }
    }
}