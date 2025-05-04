// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spine/SkeletonColorMatrix" {
Properties {
_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
_MainTex ("Texture to blend", 2D) = "black" {}
_ColorMatrixR("color matrix r",Vector) = (1,0,0,0)
_ColorMatrixG("color matrix g",Vector) = (0,1,0,0)
_ColorMatrixB("color matrix b",Vector) = (0,0,1,0)
_ColorMatrixA("color matrix a",Vector) = (0,0,0,1)
_OutLineAlpha("outline alpha",Range(0,1)) = 0
}
// 2 texture stage GPUs
SubShader {
Tags { "Queue"="Transparent+130" "IgnoreProjector"="True" "RenderType"="Transparent" }
LOD 100
Cull Off
ZWrite Off
Blend One OneMinusSrcAlpha
Lighting Off
/*Pass {
Name "OutLine"
CGPROGRAM
//往上描边一个像素。
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct appdata_t
{
float4 vertex : POSITION;
float2 texcoord : TEXCOORD0;
fixed4 color : COLOR;
};
struct v2f
{
float4 vertex : SV_POSITION;
half2 texcoord : TEXCOORD0;
fixed4 color : COLOR;
//fixed4 color1 : COLOR1;
};
sampler2D _MainTex;
float4x4 _ColorMatrix;
float4 _ColorMatrixR;
float4 _ColorMatrixG;
float4 _ColorMatrixB;
float4 _ColorMatrixA;
float _OutLineAlpha;
v2f vert (appdata_t v)
{
v2f o;
o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
o.vertex.y += 0.003;
o.texcoord = v.texcoord;
o.color = v.color;
//o.color1 = o.vertex;
return o;
}
fixed4 frag (v2f i) : COLOR
{
fixed4 col = tex2D(_MainTex, i.texcoord.xy);
col = fixed4(0,0,0,col.a*_OutLineAlpha);
return col;
}
ENDCG
}*/
Pass {
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
struct appdata_t
{
float4 vertex : POSITION;
float2 texcoord : TEXCOORD0;
fixed4 color : COLOR;
};
struct v2f
{
float4 vertex : SV_POSITION;
half2 texcoord : TEXCOORD0;
fixed4 color : COLOR;
//fixed4 color1 : COLOR1;
};
sampler2D _MainTex;
float4x4 _ColorMatrix;
float4 _ColorMatrixR;
float4 _ColorMatrixG;
float4 _ColorMatrixB;
float4 _ColorMatrixA;
v2f vert (appdata_t v)
{
v2f o;
o.vertex = UnityObjectToClipPos(v.vertex);
o.texcoord = v.texcoord;
o.color = v.color;
//o.color1 = o.vertex;
return o;
}
fixed4 frag (v2f i) : COLOR
{
_ColorMatrix = float4x4(_ColorMatrixR,_ColorMatrixG,_ColorMatrixB,_ColorMatrixA);
fixed4 col = tex2D(_MainTex, i.texcoord.xy) ;
fixed4 color = i.color;
col *= color;
col = mul(_ColorMatrix,col);
//col.a *= step(0,i.color1.g);
return col;
}
ENDCG
}
/*Pass {
Name "Caster"
Tags { "LightMode"="ShadowCaster" }
Offset 1, 1
Fog { Mode Off }
ZWrite On
ZTest LEqual
Cull Off
Lighting Off
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_shadowcaster
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"
struct v2f { 
V2F_SHADOW_CASTER;
float2  uv : TEXCOORD1;
};
uniform float4 _MainTex_ST;
v2f vert (appdata_base v) {
v2f o;
TRANSFER_SHADOW_CASTER(o)
o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
return o;
}
uniform sampler2D _MainTex;
uniform fixed _Cutoff;
float4 frag (v2f i) : COLOR {
fixed4 texcol = tex2D(_MainTex, i.uv);
clip(texcol.a - _Cutoff);
SHADOW_CASTER_FRAGMENT(i)
}
ENDCG
}*/
}
// 1 texture stage GPUs
SubShader {
Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
LOD 100
Cull Off
ZWrite Off
//ZWrite On
Blend One OneMinusSrcAlpha
Lighting Off
Pass {
ColorMaterial AmbientAndDiffuse
SetTexture [_MainTex] {
Combine texture * primary DOUBLE, texture * primary
}
}
}

}