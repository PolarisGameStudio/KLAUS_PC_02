// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Starburst_SM2_0" 
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

		Offset -4, -4
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


				half4 _Starburst = fixed4(0, 0, 0, 0);
				fixed2 _StarburstPosition = fixed2(0, 0);
				

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

	
					if(_RotationValues.x > 0.01 || _RotationValues.y != 0)
					{
						coords.xy -= _OffsetScale.xy;						
						fixed2x2 rotationMatrix = getRotationMatrix(_RotationValues.y, _RotationValues.x, _Time.y);
						coords.xy = mul (coords.xy, rotationMatrix);
						coords.xy += _OffsetScale.xy;
					}

					o.uv = coords;

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{					
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------

					fixed opacity = getMask(i.uv).x;

					//--------------------------------------------------
					// CG Flare
					//--------------------------------------------------
					fixed2 uvcenter = (_OffsetScale.xy - i.uv);
					fixed2 haloVec = normalize(uvcenter) * _Starburst.x;
					

					//----------------------------------
					// Halo
					//----------------------------------
					// No halo in SM2

					//----------------------------------
					// Starburst
					//----------------------------------
					fixed weight = (length(fixed2(0.5, 0.5) - frac(haloVec)) / ROOT2);	

					fixed col =_TintColor * opacity * weight;					 
					
					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col;
			}


			ENDCG
		}
		
	}
}