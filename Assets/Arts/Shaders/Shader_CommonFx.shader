//特效通用Shader功能如下：
//颜色强度
//UV流动
//遮照
//扭曲

Shader "PCX/CommonEffect"
{
    Properties
    {
        //混合模式 ， 剔除模式
        [Header(RenderingMode)] [Space(4)]
        [Enum(UnityEngine.Rendering.BlendMode)]_SrcBlend ("Src Blend", int) = 0
        [Enum(UnityEngine.Rendering.BlendMode)]_DstBlend ("Dst Blend", int) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_Cull ("Cull", int) = 0
        
        [Header(Base)] [Space(4)]
        _MainTex ("MainTex", 2D) = "while" { }//主贴图
        _Color ("Color", Color) = (1, 1, 1, 1)      //主颜色
        _Intensity ("Intensity", Range(-4, 4)) = 1  //强度
        _MainUVSpeedX ("MainUVSpeed X", float) = 0  //主贴图UV X流速
        _MainUVSpeedY ("MainUVSpeed Y", float) = 0  //主贴图UV Y流速

        [Header(Mask)] [Space(4)]
        [Toggle] _MaskEnabled ("Mask Enabled", int) = 0//是否开启Mask的变体
        _MaskTex ("MaskTex", 2D) = "while" { }//遮照贴图
        _MaskUVSpeedX ("MaskUVSpeed X", float) = 0  //遮照UV X流速
        _MaskUVSpeedY ("MaskUVSpeed Y", float) = 0  //遮照UV Y流速

        [Header(Distort)]
        [Toggle] _DistortEnabled ("Distort Enabled", int) = 0//是否开启扭曲的变体
        _DistortTex ("DistortTex", 2D) = "while" { }//扭曲贴图
        _Distort ("Distort", Range(0, 1)) = 0 //扭曲强度
        _DistortUVSpeedX ("DistortUVSpeed X", float) = 0  //扭曲UV X流速
        _DistortUVSpeedY ("DistortUVSpeed Y", float) = 0  //扭曲UV Y流速
    }

    SubShader
    {
        
        Tags { "Queue" = "Transparent" }
        
        Blend [_SrcBlend] [_DstBlend]

        Cull [_Cull]

        pass
        {
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            //变体
            #pragma shader_feature _MASKENABLED_ON
            #pragma shader_feature _DISTORTENABLED_ON

            //Base
            sampler2D _MainTex;float4 _MainTex_ST;
            fixed4 _Color;
            float _Intensity;
            float _MainUVSpeedX, _MainUVSpeedY;
            //遮照
            sampler2D _MaskTex;float4 _MaskTex_ST;
            float _MaskUVSpeedX, _MaskUVSpeedY;
            //扭曲
            sampler2D _DistortTex;float4 _DistortTex_ST;
            float _Distort;
            float _DistortUVSpeedX, _DistortUVSpeedY;

            struct appdata
            {
                float4 vertex: POSITION;
                float4 uv: TEXCOORD;
            };
            struct v2f
            {
                float4 pos: SV_POSITION;
                float4 uv: TEXCOORD0;//主贴图和遮照贴图的UV
                float2 disuv: TEXCOORD1;//扭曲贴图UV
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex) + float2(_MainUVSpeedX, _MainUVSpeedY) * _Time.y;

                #if _MASKENABLED_ON
                    o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex) + float2(_MaskUVSpeedX, _MaskUVSpeedY) * _Time.y;
                #endif
                
                #if _DISTORTENABLED_ON
                    o.disuv = TRANSFORM_TEX(v.uv, _DistortTex) + float2(_DistortUVSpeedX, _DistortUVSpeedY) * _Time.y;
                #endif
                
                return o;
            }

            fixed4 frag(v2f i): SV_TARGET
            {
                fixed4 color = _Color * _Intensity;
                float2 mainuv = i.uv.xy;
                #if _DISTORTENABLED_ON
                    fixed4 distortTex = tex2D(_DistortTex, i.disuv);
                    mainuv = lerp(i.uv.xy, distortTex, _Distort);
                #endif
                
                color *= tex2D(_MainTex, mainuv);

                #if _MASKENABLED_ON
                    fixed4 maskTex = tex2D(_MaskTex, i.uv.zw);
                    color *= maskTex;
                #endif

                return color;
            }
            ENDCG
            
        }
    }
}