Shader "Custom/FlyShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Textur", 2D) = "white" {}
		_BumpMap("BumpMap", 2D) = "bump" {}
		//Inout für glow und strength
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(1.0, 10.0)) = 3.0
		_Fade("Interner Fade Faktor", float) = 0.0
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 200

		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
	#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
	#pragma target 5.0

		

	struct Input {
		float4 color : Color;
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
		
	};

		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _Color;
		float4 _RimColor;
		float _RimPower;
		float _Fade;


	void surf(Input IN, inout SurfaceOutputStandard o) {
		// Albedo comes from a texture tinted by color
		
		IN.color = _Color;
		o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * IN.color;

		

		half rim = _Fade - saturate(dot(normalize(IN.viewDir),o.Normal));
		o.Emission = _RimColor.rgb * pow(rim,_RimPower);
		o.Alpha = 0;


	}
	ENDCG
	}
		FallBack "Diffuse"
}