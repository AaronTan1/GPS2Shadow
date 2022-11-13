// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/NoAdditiveAlphaBlendTest"
{
    Properties {
        _MainTex ("Texture to blend", 2D) = "white" {}
		_Color ("Main Color", Color) = (58,58,58,220)
		_ColorEx ("Exclusion Color", Color) = (58,58,58,220)
		_RefEx ("Exclusion reference", Color) = (65,255,56,255)
    }

    SubShader {
		
	    Tags { "Queue"="Transparent" }
	     
		Pass {
		    Stencil {
		        Ref 2
		        Comp NotEqual
		        Pass Replace
		    }

			Cull Off
			Lighting Off
			//ZWrite Off
			ColorMask RGB
		    Blend SrcAlpha OneMinusSrcAlpha     
	 
			 CGPROGRAM
			 #pragma vertex vert
			 #pragma fragment frag
			 #include "UnityCG.cginc"
			 
			 uniform sampler2D _MainTex;
			 uniform half4 _Color;
			 uniform half4 _ColorEx;
			 uniform half4 _RefEx;

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
				else if (color.r == 0.0 && color.g != 0.0)
					color = _ColorEx;
				else
					color = _Color;
				return color;
			}


			 ENDCG
		}

	}
 
	Fallback off
}
