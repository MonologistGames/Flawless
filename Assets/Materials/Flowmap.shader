Shader "Unlit/FlowMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("主颜色",Color) = (1,1,1,1)
        _FlowMap("FlowMap",2D) = "white"{}
        _FlowSpeed("向量场强度",float) = 0.1
        _TimeSpeed("全局流速",float) = 1
        [Toggle]_reserve_flow("反转流向",Int)=0
        
        }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalRender Pipeline"" RenderType"="Transparent" "Queue"="Transparent" "LightMode"="SRPDefaultUnlit"}
        Cull Off
        Lighting Off
        ZWrite On 
        Blend SrcAlpha OneMinusSrcAlpha
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _FlowMap_ST;
        float4 _MainColor;
        bool _reserve_flow;
        float _FlowSpeed;
        float _TimeSpeed;
        CBUFFER_END
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        TEXTURE2D(_FlowMap);
        SAMPLER(sampler_FlowMap);
        ENDHLSL

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _RESERVE_FLOW_ON

            struct appdata
            {
                float4 vertex:POSITION;
                half2 texcoord:TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex:SV_POSITION;
                half2 uv : TEXCOORD0;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            

            half4 frag (v2f i) : SV_Target
            {
                //从flowmap中获取流向
                float3 flowDir = SAMPLE_TEXTURE2D(_FlowMap,sampler_FlowMap,i.uv)* 2.0 - 1.0;

                //FLowSpeed控制向量场强度，值越大，不同位置流速差越明显
                flowDir*=_FlowSpeed;

                #ifdef _REVERSE_FLOW_ON
                    flowDir*=-1;
                #endif

                //构造周期相同，相位相差半个周期的波形函数
                float phase0 = frac(_Time.x*0.1*_TimeSpeed);
                float phase1 = frac(_Time.x*0.1*_TimeSpeed+0.5);

                //平铺贴图用的uv
                float2 tiling_uv = i.uv*_MainTex_ST.xy+_MainTex_ST.zw;

                //用偏移后的uv对材质进行偏移采样
                half3 tex0 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,tiling_uv-flowDir.xy*phase0);
                half3 tex1 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,tiling_uv-flowDir.xy*phase1);

                //构造函数计算随波形函数变化的权值，使得MainTex采样值在接近最大偏移时有权值为0，并因此消隐，构造较平滑的循环
                float flowLerp = abs((0.5-phase0)/0.5);
                half3 finalColor = lerp(tex0,tex1,flowLerp);

                float4 c = float4(finalColor,1.0)*_MainColor;

                return c;
                
            }
            ENDHLSL
        }
    }
}