Shader "PCX/UIShader"
{
    Properties
    {
        [PerRendererData] 
        _MainTex("MainTex",2D) = "white"{}
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15
        
        [Toggle]_GrayEnabled("GRAYENABLED",int) = 0
        
    }
    SubShader
    {
        Tags{"Queue" = "TransParent"}
        Blend SrcAlpha OneMinusSrcAlpha,SrcAlpha OneMinusSrcAlpha
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
            
            #pragma multi_compile _ _GRAYENABLED_ON
            
            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv:TEXCOORD;
                fixed4 color:COLOR;
            };
            struct v2f
            {
                float4 pos:SV_POSITION;
                float2 uv:TEXCOORD;
                fixed4 color:COLOR;
            };
            
            sampler2D _MainTex; 
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }
            
            fixed4 frag(v2f i):SV_TARGET 
            {
                float4 mainTex = tex2D(_MainTex,i.uv); 
                float4 color = mainTex * i.color;
                #if _GRAYENABLED_ON
                color.rgb = Luminance(color.rgb);
                #endif
                                
                
                return color;
            }
            
            ENDCG
        }
    }
}