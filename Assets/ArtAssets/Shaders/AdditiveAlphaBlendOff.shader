Shader "Hidden/AdditiveAlphaBlendOff"
{
    Properties {
        _MainTex ("Texture to blend", 2D) = "black" {}
    }

    SubShader {
        Cull Off ZWrite Off ZTest Always
        Tags { 
            "RenderType" = "Opaque"
            "Queue" = "Transparent" 
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            SetTexture [_MainTex] { combine texture }
        }
    }
}
