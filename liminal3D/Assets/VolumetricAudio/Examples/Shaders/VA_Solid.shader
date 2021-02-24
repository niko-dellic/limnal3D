Shader "Volumetric Audio/Solid"
{
	Properties
	{
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color1("Color 1", Color) = (1.0, 0.5, 0.5, 1.0)
		_Color2("Color 2", Color) = (0.5, 0.5, 1.0, 1.0)
		_Rim("Rim", Float) = 1.0
		_Shift("Shift", Float) = 1.0
	}

	SubShader
	{
		Cull Off

		Tags
		{
			"Queue" = "Geometry"
			"DisableBatching" = "True"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#include "UnityCG.cginc"

			float4 _Color;
			float4 _Color1;
			float4 _Color2;
			float  _Rim;
			float  _Shift;

			struct a2v
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex    : SV_POSITION;
				float4 screenPos : TEXCOORD0;
				float3 normal    : TEXCOORD1;
				float3 direction : TEXCOORD2;
			};

			struct f2g
			{
				float4 color : SV_TARGET;
			};

			float Dither(float4 ScreenPosition)
			{
				float2 uv = ScreenPosition.xy * _ScreenParams.xy;
				float DITHER_THRESHOLDS[16] =
				{
					1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
				};
				uint index = (uint(uv.x) % 4) * 4 + uint(uv.y) % 4;
				return DITHER_THRESHOLDS[index];
			}

			void Vert(a2v i, out v2f o)
			{
				o.vertex    = UnityObjectToClipPos(i.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.normal    = mul((float3x3)unity_ObjectToWorld, i.normal);
				o.direction = _WorldSpaceCameraPos - mul(unity_ObjectToWorld, i.vertex).xyz;
			}

			void Frag(v2f i, out f2g o)
			{
				float  ang = abs(dot(normalize(i.direction), normalize(i.normal)));
				float  rim = _Shift - pow(saturate(1.0f - ang), _Rim);

				o.color = lerp(_Color1, _Color2, rim) * _Color;

				clip(o.color.a - Dither(i.screenPos / i.screenPos.w));
			}
			ENDCG
		} // Pass
	} // SubShader
} // Shader