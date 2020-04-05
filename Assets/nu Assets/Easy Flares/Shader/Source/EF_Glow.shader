// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Glow" 
{
	Properties 
	{

	}

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		Lighting Off
		Cull Off
		ZWrite Off
		Blend SrcAlpha One

		//-------------------------
		// Linear Space
		//-------------------------

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

				fixed _PointIntensity;
				fixed _PointDensity;

				sampler2D _MainTex;
				
				struct appdata_t 
				{
					float4 vertex : POSITION;
					fixed2 uv : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					fixed2 uv : TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					fixed2 coords = v.uv.xy;

					o.vertex = UnityObjectToClipPos(v.vertex);
					coords = rotateAround(coords, _RotationValues.y, _RotationValues.x, _Time.y, _OffsetScale.xy);
					o.uv = coords;
					

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------

					fixed2 m = getMask(i.uv);
					fixed opacity = m.x;


					fixed4 col = _TintColor;
					//fixed4 col = (fixed4)(1,1,1,1);


					#ifdef _LINEAR_SPACE
					col = (fixed4)(1,1,1,1);
					#endif

					fixed3 p = pow(2, -m.y * _PointDensity) * _PointIntensity;
					

					col.rgb += p;
					col.a *= opacity;
					col.a += col.a;

	
					//----------------------------------
					// Flare Color
					//----------------------------------
					fixed2 uv = rotateAround(i.uv, _RotationValues.y, 0, 0, _OffsetScale.xy);


					fixed4 gradient = getGradient(uv, _LensColorTex);

					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col * gradient) * _TintColor;
					#else
					col = col * gradient;
					#endif


					return col;
				}


			ENDCG
		}

	}

	Fallback "Easy Flares/src_Glow_SM2_0" 
}