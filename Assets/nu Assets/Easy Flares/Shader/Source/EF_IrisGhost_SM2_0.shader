// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Ghost_SM2_0" 
{
	Properties 
	{
	}
	
	SubShader 
	{
		Tags {"Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		Lighting Off
		Cull Off
		ZWrite Off
		Blend SrcAlpha One

		Pass 
		{  
			CGPROGRAM
				#pragma shader_feature _LINEAR_SPACE
				#pragma vertex vert
				#pragma fragment frag

				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma target 2.0
				#pragma glsl
			
				#include "UnityCG.cginc"
				#include "CG/Util.cginc"

				sampler2D _MainTex;
				

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};
			
				v2f vert (appdata_t v)
				{
					v2f o;
					fixed2 coords = v.uv;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = coords;
					o.color = v.color;

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					fixed4 c = tex2D(_MainTex, i.uv);
					fixed4 col = _TintColor * c;

					//--------------------------------------------------
					// Mask
					//--------------------------------------------------
					// No mask in SM2
	
					//----------------------------------
					// Flare Color
					//----------------------------------
					col = col * getGradient(i.uv, _LensColorTex) * i.color;

					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col;

				}


			ENDCG
		}
	}
}