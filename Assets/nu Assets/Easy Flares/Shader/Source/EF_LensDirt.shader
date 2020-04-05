// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Dirt" 
{
	Properties 
	{

	}

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Lighting Off
		Cull Off
		ZWrite Off
		Blend SrcAlpha One
	
		SubShader 
		{
			Pass 
			{		
				CGPROGRAM
				#pragma shader_feature _LINEAR_SPACE
				#pragma vertex vert
				#pragma fragment frag

				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma glsl

				#include "UnityCG.cginc"
				#include "CG/Util.cginc"

				sampler2D _MainTex;

				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 uv : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 uv : TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					o.color = v.color;


					return o;
				}

				//sampler2D_float _CameraDepthTexture;
				float _InvFade, _BlurAmount;
			
				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------
					fixed opacity = getMask(i.uv);



					fixed4 col = i.color * _TintColor * tex2D(_MainTex, i.uv);

					col.a *= opacity;

					col = col * getGradient(i.uv, _LensColorTex);


					return col;
				}
				ENDCG 
			}
		}	
	}

	Fallback  "Easy Flares/src_Dirt_SM2_0" 
}
