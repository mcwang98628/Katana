Shader "PCX/UIShaderLightBeam"
{
    Properties
    {
        
        _LightPow ("Pow", Float) = 1
        _LightAngle ("LightAngle", int) = 303
        
        [PerRendererData]
        [HideInInspector]_MainTex ("MainTex", 2D) = "white" { }
        
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "Queue" = "TransParent" }
        Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]
        ZTest off
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        
        Pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            struct v2f
            {
                float4 pos: SV_POSITION;
                float2 uv: TEXCOORD;
                fixed4 color: COLOR;
            };
            float _LightPow;
            float _LightAngle;
            sampler2D _MainTex;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(v2f i): SV_TARGET
            {
                float4 mainTex = tex2D(_MainTex, i.uv);
                float4 color = mainTex * i.color;
                
                float alpha = 1 - distance(i.uv, half2(0.5, 0.5)) * 2;
                alpha *=distance(i.uv, half2(0.5, 0.5))>0.49?0:1;
                alpha = pow(alpha, _LightPow);

                half2 vec2 = normalize(i.uv - half2(0.5, 0.5));
                float angle = acos(dot(vec2, half2(0, 1))) * 180 / 3.14 ;
                int dir = vec2.x > 0?1: 0;
                alpha *= ((int) (angle / _LightAngle) + dir) % 2 == 0?0: 1;             
                
                
                return half4(color.rgb, alpha*color.a);
            }
            
            ENDCG
            
        }
    }
}