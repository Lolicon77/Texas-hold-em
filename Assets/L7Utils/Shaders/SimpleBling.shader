// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SimpleBling"
{
	Properties
	{
		_MainColor ("MainColor", Color) = (1,0,0,1)
		_BlingColor ("BlingColor", Color) = (1,1,1,1)
		_Speed ("Speed",Range(0,50)) = 10
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			uniform fixed4 _MainColor;
			uniform fixed4 _BlingColor;
			uniform fixed _Speed;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed time = saturate (sin(_Time.y * _Speed));
				fixed4 col = lerp(_BlingColor,_MainColor,time);
				return col;
			}
			ENDCG
		}
	}
}
