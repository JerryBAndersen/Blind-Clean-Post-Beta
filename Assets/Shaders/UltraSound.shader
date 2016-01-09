Shader "Tesselation" {
    Properties {
        _Tess ("Tessellation", Range(1,32)) = 4
        _Phong ("Phong Strengh", Range(0,1)) = 0.5
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Secondary ("Secondary (RGB)", 2D) = "white" {}
        _DispTex ("Disp Texture", 2D) = "gray" {}
        _Displacement ("Displacement", Range(0, 1.0)) = 0.3
		_VisibleDistance ("Visibility Distance", float) = 1.0
		_Origin ("Origin", Vector) = (0.0,0.0,0.0)
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300
        
        CGPROGRAM
        #include "UnityPBSLighting.cginc"
        #pragma surface surf Standard vertex:disp finalcolor:mycolor tessellate:tessFixed tessphong:_Phong
        #pragma target 5.0
		#include "Tessellation.cginc"
		
        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;
        };

        float _Phong;
        float _Tess;

        float4 tessFixed()
        {
            return _Tess;
        }

        sampler2D _DispTex;
        float _Displacement;

        sampler2D _MainTex;
        sampler2D _Secondary;
		float3 worldPos;
		float _VisibleDistance;
		float3 _Origin;


        void disp ( inout appdata v)
        {			
			float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			v.vertex.xyz += v.normal * d;
        }
		
		
        struct Input {
            float2 uv_MainTex;
			float3 worldPos;
        };
		
		float3 Hue(float H)
		{
		    float R = abs(H * 6 - 3) - 1;
		    float G = 2 - abs(H * 6 - 2);
		    float B = 2 - abs(H * 6 - 4);
		    return saturate(float3(R,G,B));
		}

		void mycolor (Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			float2 coord = float2(0.5,(distance(_Origin, IN.worldPos) / _VisibleDistance));
			color = color*min(10.0 / distance(_WorldSpaceCameraPos, IN.worldPos),1.0) + (1.0 - 10.0 / distance(_WorldSpaceCameraPos, IN.worldPos)) * tex2Dlod (_Secondary, fixed4(coord.x,coord.y,0,0));
			//float3 rgbhue = Hue(sin(.8*distance(_Origin, IN.worldPos)));
			//color = color*0.8 + 0.2 * fixed4(1.0,1.0,1.0,1.0) * 1.0-(distance(_Origin, IN.worldPos) / _VisibleDistance) ;
			//if(distance(_Origin, IN.worldPos) < (_VisibleDistance*1.3) && distance(_Origin, IN.worldPos) > ((_VisibleDistance*1.3)-0.1)){
			//	color = fixed4(1.0,1.0,1.0,1.0);
			//}
		}

		
        void surf (Input IN, inout SurfaceOutputStandard o) {
			clip(distance(_Origin, IN.worldPos) > _VisibleDistance + 1.5 ? -1:1);
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;			
			float2 coord = float2(0.5, (distance(_Origin, IN.worldPos) / _VisibleDistance));
			//o.Albedo = o.Albedo * tex2Dlod(_Secondary, fixed4(coord.x, coord.y, 0, 0));
			//o.Albedo *= 1.0 / distance(_WorldSpaceCameraPos, IN.worldPos);
			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			//o.Alpha = c.a;
            //o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
