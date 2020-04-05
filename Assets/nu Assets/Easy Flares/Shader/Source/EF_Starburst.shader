// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Starburst" 
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
				half4 _Starburst = fixed4(0, 0, 0, 0);

				half _HaloIntensity;
				half _HaloSize;
				half _Scattering;
				int _EnableHalo;
				fixed4 _StarburstPosition;
				

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

					coords.xy -= _StarburstPosition.xy;											
					fixed2x2 rotationMatrix = getRotationMatrix(_RotationValues.y, _RotationValues.x, _Time.y);
					coords.xy = mul (coords.xy, rotationMatrix);
					coords.xy += _StarburstPosition.xy;
			
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

					fixed4 col;

					//--------------------------------------------------
					// CG Flare
					//--------------------------------------------------
					fixed2 uvcenter = (_StarburstPosition.xy - i.uv);
	
					fixed2 haloVec = normalize(uvcenter) * _Starburst.x;// * pow(length(uv.xy), _Distortion);
					
	
					//--------------------------------------------------
					// Scattering
					//--------------------------------------------------
					fixed t = _Scattering * TORAD;		

					fixed2x2 rotationMatrix = fixed2x2(1, -t, t, 0.5);
					haloVec.xy = mul (haloVec.xy, rotationMatrix);
					
			
					//----------------------------------
					// Halo
					//----------------------------------
					fixed4 result = fixed4(0, 0, 0, 0);

					fixed len = (length(_StarburstPosition.xy - i.uv)) / 2 * ROOT2;
					fixed fac = pow(1 - len, 10 * _HaloSize) * _HaloIntensity;
	
					result = lerp(fixed4(0, 0, 0, 0), fac, _EnableHalo > 0);
		
					//----------------------------------
					// Starburst
					//----------------------------------
					half ln = length(fixed2(0.5, 0.5) - frac(haloVec));


					fixed weight = ln / ROOT2;
					fixed w = pow(1.0 - weight, 1.0 - _Starburst.z) * _Starburst.y;

					weight += w * _Starburst.w;
					weight *= _Starburst.y;

					result += weight;
	
					//----------------------------------
					// Flare Color
					//----------------------------------
					fixed4 gradient = getGradient(i.uv, _LensColorTex);

					
					//#ifdef _LINEAR_SPACE
					//gradient = LinearToGammaColor(gradient);
					//#endif

					result *= gradient;



					col = opacity * saturate(result);

					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col * _TintColor;
				}


			ENDCG
		}

		
	}

	Fallback  "Easy Flares/src_Starburst_SM2_0"
}