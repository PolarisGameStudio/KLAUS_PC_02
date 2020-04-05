Shader "Easy Flares/src_Dirt_SM2_0" 
{
	Properties 
	{

	}

	Category 
	{
		Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Transparent" }

		Blend SrcAlpha One
		Cull Off 
		Lighting Off 
		ZWrite Off 

	
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
					float2 uv : TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;

					o.vertex = v.vertex;
					o.uv = v.uv;	

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------
					fixed4 col = _TintColor * tex2D(_MainTex, i.uv);

					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col;
				}
				ENDCG 
			}
		}	
	}
}
