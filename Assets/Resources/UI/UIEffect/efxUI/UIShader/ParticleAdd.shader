// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:True,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32724,y:32693,varname:node_4795,prsc:2|emission-1145-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:32067,y:32571,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a0203ddbaece4aa4fa472d774b9718ed,ntxv:0,isnm:False|UVIN-5255-OUT;n:type:ShaderForge.SFN_Multiply,id:2393,x:32347,y:32688,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT,E-6074-A;n:type:ShaderForge.SFN_VertexColor,id:2053,x:32067,y:32749,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:32067,y:32910,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:32052,y:33087,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:884,x:31495,y:32445,varname:node_884,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Time,id:7610,x:31387,y:32751,varname:node_7610,prsc:2;n:type:ShaderForge.SFN_ValueProperty,id:1735,x:31377,y:32646,ptovrint:False,ptlb:U,ptin:_U,varname:node_1735,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8871,x:31398,y:33005,ptovrint:False,ptlb:V,ptin:_V,varname:_node_1735_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:27,x:31590,y:32646,varname:node_27,prsc:2|A-1735-OUT,B-7610-T;n:type:ShaderForge.SFN_Multiply,id:7694,x:31641,y:32844,varname:node_7694,prsc:2|A-7610-T,B-8871-OUT;n:type:ShaderForge.SFN_Append,id:1039,x:31866,y:32688,varname:node_1039,prsc:2|A-27-OUT,B-7694-OUT;n:type:ShaderForge.SFN_Add,id:5255,x:31883,y:32506,varname:node_5255,prsc:2|A-884-UVOUT,B-1039-OUT;n:type:ShaderForge.SFN_Multiply,id:7092,x:32333,y:32862,varname:node_7092,prsc:2|A-6074-A,B-2053-A,C-797-A;n:type:ShaderForge.SFN_Multiply,id:1145,x:32504,y:32775,varname:node_1145,prsc:2|A-2393-OUT,B-7092-OUT;proporder:6074-797-1735-8871;pass:END;sub:END;*/

Shader "UI/ParticleAdd" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _U ("U", Float ) = 0
        _V ("V", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles metal d3d11_9x 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _TintColor;
            uniform float _U;
            uniform float _V;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_7610 = _Time;
                float2 node_5255 = (i.uv0+float2((_U*node_7610.g),(node_7610.g*_V)));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_5255, _MainTex));
                float3 emissive = ((_MainTex_var.rgb*i.vertexColor.rgb*_TintColor.rgb*2.0*_MainTex_var.a)*(_MainTex_var.a*i.vertexColor.a*_TintColor.a));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, finalRGBA, fixed4(0,0,0,1));
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}
