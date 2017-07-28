// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "L7/Stencil/OutLine"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("OutlineColor", Color) = (1,1,1,1)
		_OutlineWidth("OutlineWidth", Range(0.0, 1.0)) = 0.02
	}
	SubShader
	{

		CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		float _OutlineWidth;
		float4 _OutlineColor;

		struct appdata_uv
		{
			float4 vertex : POSITION;
			float2 uv 	  : TEXCOORD0; 
		};

		struct appdata_normal
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float2 uv  : TEXCOORD0; 
		};

		struct v2f_simple
		{
			float4 pos : SV_POSITION;
		};

		v2f vert_pass1 (appdata_uv v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}
		
		fixed4 frag_pass1 (v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex,i.uv); 
			return col;
		}

		v2f_simple vert_pass2 (appdata_normal v)
		{
			v2f_simple o;
			o.pos = UnityObjectToClipPos(v.vertex + float4(v.normal,0) * _OutlineWidth);
			o.pos.z = o.pos.z + _OutlineWidth;
			return o;
		}
		
		fixed4 frag_pass2 (v2f_simple i) : SV_Target
		{
			return _OutlineColor;
		}

		ENDCG


		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
            Stencil {  
                Ref 1        //参考值为2，stencilBuffer值默认为0  
                Comp greater            //stencil比较方式是永远通过  
                Pass replace           //pass的处理是替换，就是拿2替换buffer 的值  
            }  
			CGPROGRAM
			#pragma vertex vert_pass1
			#pragma fragment frag_pass1

			ENDCG
		}

		Pass
		{
            Stencil {  
                Comp equal            //stencil比较方式是永远通过  
            }  

			Cull Front 
			ZTest Less

			CGPROGRAM
			#pragma vertex vert_pass2
			#pragma fragment frag_pass2

			ENDCG
		}

	}
}
