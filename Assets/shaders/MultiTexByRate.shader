Shader "Custom/MultiTexByRate" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BackTex ("Back (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
	    _Ratio ("Ratio", float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
//		ZTest  Always
//		ZWrite  Off
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		float4 _Color;
		sampler2D _MainTex;
		sampler2D _BackTex;
		sampler2D _MaskTex;
		float _Ratio;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BackTex;
			float2 uv_MaskTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c0 = tex2D (_MainTex, IN.uv_MainTex);
			half4 c1 = tex2D (_BackTex, IN.uv_BackTex);
			half4 cm = tex2D (_MaskTex, IN.uv_MaskTex);
			float rate01 = clamp(_SinTime.w, -0.5, 0.5)+0.5;
//			float rate01 = (2.0f * _SinTime.x * _CosTime.x +1.0f ) * 0.5f;
//			float rate01 = (3.0f * _SinTime.w - 4.0f * _SinTime.w * _SinTime.w  * _SinTime.w + 1.0f) *0.5f;
			o.Emission = (c0.rgb * rate01 + c1.rgb * (1.0f-rate01))*_Color.rgbb*2+max(half3(0,0,0),_Color.rgb-0.5)*2;
			o.Alpha = (c0.a * rate01 + c1.a * (1.0f-rate01))*cm.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
