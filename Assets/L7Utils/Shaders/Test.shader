// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "L7/Test" {  //see https://www.shadertoy.com/view/4dsGzH  
  
    CGINCLUDE    
  
        #include "UnityCG.cginc"                
        #pragma target 3.0

        struct v2f {    
            float4 pos:SV_POSITION;    
            float4 srcPos:TEXCOORD0;   
        };  
  
        v2f vert(appdata_base v) {  
            v2f o;  
            o.pos = UnityObjectToClipPos (v.vertex);  
            o.srcPos = ComputeScreenPos(o.pos);  
            return o;  
        }  
  
        fixed4 frag(v2f i) : SV_Target {  
            return step(i.srcPos.z,0.25);  
        }  
  
    ENDCG    
  
    SubShader {    
        Pass {    
            CGPROGRAM    
  
            #pragma vertex vert    
            #pragma fragment frag    
            #pragma fragmentoption ARB_precision_hint_fastest     
  
            ENDCG    
        }    
  
    }     
    FallBack Off    
}  