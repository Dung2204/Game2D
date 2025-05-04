// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask
//处理NGUI遮住粒子特效问题 Mobile/Particles/Additive2
Shader "Mobile/Particles/Additive2" {
Properties {
	_MainTex ("Particle Texture", 2D) = "white" {}
}

Category {
	//Queue队列+20 = 3020 > 3000(NGUI)
	Tags { "Queue"="Transparent+20" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}