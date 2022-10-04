// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/NoAdditiveAlphaBlendTest"
{
    Properties {
        _MainTex ("Texture to blend", 2D) = "white" {}
		_Color ("Main Color", Color) = (58,58,58,220)
    }

    SubShader {
		
	    Tags { "Queue"="Transparent" }
	     
		Pass {
		    Stencil {
		        Ref 2
		        Comp NotEqual
		        Pass Replace
		    }
			ColorMask RGB
		    Blend SrcAlpha OneMinusSrcAlpha     
	 
			 CGPROGRAM
			 #pragma vertex vert
			 #pragma fragment frag
			 #include "UnityCG.cginc"
			 
			 uniform sampler2D _MainTex;
			 uniform half4 _Color;
			 struct v2f {
			     half4 pos : POSITION;
			     half2 uv : TEXCOORD0;
			 };
			 
			 v2f vert(appdata_img v) {
			     v2f o;
			     o.pos = UnityObjectToClipPos (v.vertex);
			     half2 uv = MultiplyUV( UNITY_MATRIX_TEXTURE0, v.texcoord );
			     o.uv = uv;
			     return o;
			 }

			 half4 frag (v2f i) : COLOR {
				half4 color = tex2D(_MainTex, i.uv);
				if (color.a == 0.0)
					discard;
				else
					color = _Color;
				return color;
			}


			 ENDCG
		}

	}
 
	Fallback off
}
