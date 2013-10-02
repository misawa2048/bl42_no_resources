Shader "Custom/MultiTexByMask" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BackTex ("Back (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "black" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry" }
//		ZTest  Always
//		ZWrite  Off
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _BackTex;
		sampler2D _MaskTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BackTex;
			float2 uv_MaskTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c0 = tex2D (_MainTex, IN.uv_MainTex);
			half4 c1 = tex2D (_BackTex, IN.uv_BackTex);
			half4 cm = tex2D (_MaskTex, IN.uv_MaskTex);
			o.Emission = c1.rgb * cm.a + c0.rgb * (1.0f-cm.a);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
