// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Glow_SM2_0" 
{
	Properties 
	{

	}

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
	
		Lighting Off
		ZWrite On
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
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;


					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------

					fixed2 m = getMask(i.uv);

					fixed4 col = _TintColor;
					col.a *= m.x;
					col.a += col.a;

	
					//----------------------------------
					// Flare Color
					//----------------------------------

					col =  col * getGradient(i.uv, _LensColorTex);// * ccc;
					
					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col;
				}

			ENDCG
		}
	}
}