Shader "Edirlei/Dissolve"{
	Properties{
		_Color("Color", Color) = (0,0,0,1)
		_MainTex("Texture", 2D) = "white" {}
		_DissolveTex("Dissolve Texture" , 2D )= "white"{}
		_DissolveCutoff("Dissolve cutoff", Range(0,1)) = 1
		
		_Speed("Speed", Range(0,10)) = 1
			_amplitude_x("Amplitude x", Range(0,10)) = 1
			_amplitude_y("Amplitude y", Range(0,10)) = 1
	}
		SubShader{
			Pass{
				CGPROGRAM
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmentProgram
				#include "UnityCG.cginc"
				float4 _Color;
				sampler2D _MainTex, _DissolveTex;
				float4 _MainTex_ST, _DissolveTex_ST;
				float _DissolveCutoff;
				float _Speed;
				float _amplitude_x;
				float _amplitude_y;

				struct VertexData {
					float4 position:POSITION;
					float2 uv : TEXCOORD0;
				};
				struct VertexToFragment {
					float4 position: SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uvDissolve: TEXCOORD1;

				};


				VertexToFragment MyVertexProgram(VertexData vertex){
					VertexToFragment v2f;
					vertex.position.x += sin((_Time.y * _Speed) + vertex.position.y * _amplitude_y) * _amplitude_x;
					v2f.uv = vertex.uv * _MainTex_ST.xy + _MainTex_ST.zw;
					v2f.uvDissolve = vertex.uv * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
					v2f.position = UnityObjectToClipPos(vertex.position);
					return v2f;
					}

				float4 MyFragmentProgram(VertexToFragment v2f) : SV_TARGET {
					float4 texturecolor = tex2D(_MainTex,v2f.uv) * _Color;
					float4 dissolvecolor = tex2D(_DissolveTex,v2f.uvDissolve);

					clip(dissolvecolor.rgb - _DissolveCutoff);
					//color *= tex2D(_DetailTex, v2f.uvDetail)* unity_ColorSpaceDouble;
					return texturecolor;
					}

			ENDCG
		}
	}
}