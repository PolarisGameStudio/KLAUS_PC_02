// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Ghost" 
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
				#pragma target 3.0
				//#pragma enable_d3d11_debug_symbols
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
					fixed2 coords = v.uv.xy;

					o.vertex = UnityObjectToClipPos(v.vertex);

					coords = rotate(coords, _RotationValues.y, _RotationValues.x, _Time.y);

					o.uv = coords;
					o.color = v.color;

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------
					fixed opacity = getMask(i.uv).x;
					fixed4 col = _TintColor;

					#ifdef _LINEAR_SPACE
					opacity = GammaToLinear(opacity);
					#endif

					//----------------------------------
					// Flare Color
					//----------------------------------
					col = col * fixed4(1, 1, 1, opacity) * getGradient(i.uv, _LensColorTex) * i.color;


					return col;
				}


			ENDCG
		}
	}

	Fallback  "Easy Flares/src_Ghost_SM2_0" 
}