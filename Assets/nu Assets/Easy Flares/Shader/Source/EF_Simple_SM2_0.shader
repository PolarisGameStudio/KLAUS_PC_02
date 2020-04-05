// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Easy Flares/src_Simple_SM2_0" 
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


				half _StarburstIntensity, _StarburstSpikes, _StarburstRoughness, _StarburstSoftness;
				half _HaloIntensity, _HaloSize, _Scattering;
				

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

	
					if(_RotationValues.x > 0.01)
					{
						coords.xy -= 0.5;
						
						fixed freq = _RotationValues.x * PI_2 *  _Time;
						
						fixed s = -sin ( freq );
						fixed c = cos ( freq );

						fixed2x2 rotationMatrix = fixed2x2(c, -s, s, c);

						coords.xy = mul (coords.xy, rotationMatrix);
						coords.xy += 0.5;
					}

					o.uv = TRANSFORM_TEX(coords, _MainTex);

					return o;
				}
			
				fixed4 frag (v2f i) : SV_Target
				{
					//--------------------------------------------------
					// Mask
					//--------------------------------------------------
					fixed opacity = getMask(i.uv);

					fixed4 col = _TintColor * tex2D(_MainTex, i.uv);
					
					//----------------------------------
					// Flare Color
					//----------------------------------
					fixed2 uv = -i.uv + fixed2(1.0, 1.0);
					fixed2 color_uv = length(fixed2(0.5, 0.5) - uv) / ROOT2;

					col = col * opacity * getGradient(i.uv, _LensColorTex);

					#ifdef _LINEAR_SPACE
					col = GammaToLinearColor(col);
					#endif

					return col;
				}


			ENDCG
		}
	}
}