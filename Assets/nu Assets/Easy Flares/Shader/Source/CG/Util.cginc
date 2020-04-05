#ifndef mask
#define mask

#define PI			3.1415926536
#define PI_2		6.2831853072
#define E			2.7182818284
#define TORAD		0.0174532925
#define ROOT2		0.70710679
#define ROOT2_INV	1.41421356
#define GAMMA 2.2f


// Speed, Rotation
fixed4 _RotationValues = fixed4(0, 0, 0, 0);

//fixed _SampleScale;
fixed4 _SampleOffsetScale = fixed4(0, 0, 0, 0);

// Smooth[x], Inner[y], Outer[z], Invert[w]
fixed4 _Mask = fixed4(0, 0, 0, 0);


fixed _EnableMask = fixed(1.0);


half4 _OffsetScale;	
half4 _TintColor = fixed4(1, 1, 1, 1);	
half4 _MainTex_ST;

fixed _GradientMode;

sampler2D _LensColorTex;


fixed2 getMask(fixed2 uv)
{	
	// Length of vector from center [0.5, 0.5] to current uv
	fixed x1 = length((_OffsetScale.xy - uv) * _OffsetScale.zw);

	fixed circleFalloff = saturate(x1 / _Mask.z);

	
	fixed2 opacity = fixed2(1, circleFalloff);


	fixed x2 = (_Mask.z - x1) / (_Mask.z - _Mask.y);


	//opacity = (x1 >= _Mask.y) ? fixed2(pow(x2, _Mask.x * fixed(0.1)), circleFalloff) : opacity;
	opacity = lerp(opacity, fixed2(pow(x2, _Mask.x * fixed(0.1)), circleFalloff), (x1 >= _Mask.y));	

		
	fixed y = (x1 - _Mask.z) / (fixed(0.5) - _Mask.z);
	fixed x = fixed(1.0) - pow(y, _Mask.x * fixed(0.1));

	opacity = _Mask.w > fixed(0.0) ? ( x1 >= _Mask.z ? fixed2(x, y) : fixed2(fixed(1.0) - opacity.x, opacity.y)) * opacity.x * 2 : opacity;

	opacity.x = lerp(opacity.x, 0, x1 >= _Mask.z);

	opacity = saturate(opacity);


	//return _EnableMask > 0.0 ? opacity : fixed2(1, 0);
	return lerp(fixed2(1, 0), opacity, _EnableMask > 0.0);
}

fixed2x2 getRotationMatrix(fixed rotation, fixed speed, fixed t)
{	
	fixed p = (rotation * TORAD) + (speed * PI_2 * t);		
	fixed a = -sin(p);
	fixed b =  cos(p);

	fixed2x2 mat = fixed2x2(b, -a, a, b);

	return mat;
}

fixed2 rotateAround(fixed2 value, fixed rotation, fixed speed, fixed t, fixed2 center)
{
	value.xy -= center.xy;
						
	fixed2x2 mat = getRotationMatrix(rotation, speed, t);

	value.xy = mul (value.xy, mat);
	value.xy += center.xy;

	return value;
}

fixed2 rotate(fixed2 value, fixed rotation, fixed speed, fixed t)
{	
	return rotateAround(value, rotation, speed, t, fixed2(0.5, 0.5));
}

fixed2 getSamplingCenter(int en, fixed2 center)
{	
	if(en == 0)
	{
		return fixed2(0.5, 0.5);
	}
	else if(en == 1)
	{
		return fixed2(0.5, 0.5);
	}
	else if(en == 10)
	{
		return center;
	}

	return center;
}

fixed4 getGradient(fixed2 uv, sampler2D tex)
{
	fixed2 uv_ = _OffsetScale.xy + _SampleOffsetScale.xy - uv;

	fixed2 color_uv = length(uv_) * ROOT2_INV;
	
	
	//uv_ = (_GradientMode > 0 ? color_uv : uv_) * _SampleOffsetScale.z;
	uv_ = lerp(uv_, color_uv, _GradientMode > 0) * _SampleOffsetScale.z;

	// _GradientMode -> Linear = 0, Circular = 1
	return fixed4(tex2D(tex, uv_));
}

fixed4 getGradientSimple(fixed2 uv, sampler2D tex)
{
	fixed2 uv_ = _OffsetScale.xy + _SampleOffsetScale.xy - uv;

	return tex2D(tex, uv_);
}

fixed4 textureTransform(in sampler2D tex, in fixed2 uv, in fixed2 direction, in fixed3 distortion)
{
	return fixed4(tex2D(tex, uv + direction * (distortion.r / 1)).r,
					tex2D(tex, uv + direction * (distortion.g / 6)).g,
					tex2D(tex, uv + direction * (distortion.b / 1)).b, 1.0);
}

inline float LinearToGamma(float c)
{
	float a = 0.055f;
	float thres = 0.0031308f;

	float result = c <= thres ? c * 12.92f : (1.0f + a) * pow(c, 1.0f / GAMMA) - a;

	return result;
}

inline float GammaToLinear(float c)
{
	float a = 0.055f;
	float thres = 0.04045f;

	float result = c <= thres ? c / 12.92f : pow((c + a) / (1.0f + a), GAMMA);

	return result;
}

inline fixed4 LinearToGammaColor(fixed4 c)
{
	c.a = c.a;
	c.r = LinearToGamma(c.r);
	c.g = LinearToGamma(c.g);
	c.b = LinearToGamma(c.b);

	return c;
	//return fixed4(pow(c.rgb, 1.0 / GAMMA), c.a);  

}

inline fixed3 LinearToGammaColor3(fixed3 c)
{
	c.r = LinearToGamma(c.r);
	c.g = LinearToGamma(c.g);
	c.b = LinearToGamma(c.b);

	return c;
	//return fixed4(pow(c.rgb, 1.0 / GAMMA), c.a);  

}

inline fixed4 GammaToLinearColor(fixed4 c)
{
	c.a = c.a;
	c.r = GammaToLinear(c.r);
	c.g = GammaToLinear(c.g);
	c.b = GammaToLinear(c.b);
	return c;


	//float pixelAlpha = c.a;  
	//return fixed4(pow(c.rgb, GAMMA), c.a);  
	
	// Or (cheaper, but assuming gamma of 2.0 rather than 2.2)  
	//return float4( sqrt( finalCol ), pixelAlpha ); 

}

inline fixed3 GammaToLinearColor3(fixed3 c)
{
	c.r = GammaToLinear(c.r);
	c.g = GammaToLinear(c.g);
	c.b = GammaToLinear(c.b);
	return c;
}

#endif
