// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spine/SkeletonGray" {
	Properties {
		_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.8
		_MainTex ("Texture to blend", 2D) = "black" {}
		_GraySwitch("_GraySwitch",Range(0,1)) = 0
		_MaskPos ("Mask Pos",Vector) = (1,1,1,1)
	}
	// 2 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		Lighting Off

		Pass {
            CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			struct v2f { 
				float2  uv : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 modePos : TEXCOORD2;
			};

			uniform float4 _MainTex_ST;
			uniform fixed4 _MaskPos;
			
			v2f vert (appdata_base v) {
				v2f o;
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.modePos = v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			uniform sampler2D _MainTex;
			uniform fixed _GraySwitch;

			float4 frag (v2f i) : COLOR 
			{
			    float4 texcol = tex2D(_MainTex, i.uv);
				if (_GraySwitch == 0) {
					half gray = length(texcol.rgb) / 3;
					texcol = fixed4(gray, gray, gray, texcol.a);
				}
				float clipVlaue = (i.modePos.x-_MaskPos.x) * (i.modePos.x-_MaskPos.x) + (i.modePos.y - _MaskPos.y)*(i.modePos.y - _MaskPos.y) - _MaskPos.z;
				clip(-clipVlaue);
				return texcol;
			}
			ENDCG
		}

		Pass {
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
		}
	}
	// 1 texture stage GPUs
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100

		Cull Off
		ZWrite Off
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