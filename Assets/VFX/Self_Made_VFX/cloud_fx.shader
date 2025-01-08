Shader "URP/Clouds"
{
    Properties
    {
        _Color("Tint", Color) = (1, 1, 1, 1)
        _Noise("Noise (RGB)", 2D) = "white" {}
        _TColor("Cloud Top Color", Color) = (1, 0.6, 1, 1)
        _CloudColor("Cloud Base Color", Color) = (0.6, 1, 1, 1)
        _RimColor("Rim Color", Color) = (0.6, 1, 1, 1)
        _RimPower("Rim Power", Range(0, 40)) = 20
        _Scale("World Scale", Range(0, 0.1)) = 0.004
        _AnimSpeedX("Animation Speed X", Range(-2, 2)) = 1
        _AnimSpeedY("Animation Speed Y", Range(-2, 2)) = 1
        _AnimSpeedZ("Animation Speed Z", Range(-2, 2)) = 1
        _Height("Noise Height", Range(0, 2)) = 0.8
        _Strength("Noise Emission Strength", Range(0, 2)) = 0.3
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalRenderPipeline" "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_Noise);
            SAMPLER(sampler_Noise);

            float4 _Color;
            float4 _CloudColor;
            float4 _TColor;
            float4 _RimColor;
            float _Scale, _Strength, _RimPower, _Height;
            float _AnimSpeedX, _AnimSpeedY, _AnimSpeedZ;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float4 noiseComb : TEXCOORD3;
                float4 col : TEXCOORD4;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                float3 worldNormal = TransformObjectToWorldNormal(v.normalOS);
                float3 worldPos = TransformObjectToWorld(v.positionOS.xyz);

                float movementSpeedX = _Time.y * _AnimSpeedX;
                float movementSpeedY = _Time.y * _AnimSpeedY;
                float movementSpeedZ = _Time.y * _AnimSpeedZ;

                float4 xy = float4((worldPos.x * _Scale) - movementSpeedX, (worldPos.y * _Scale) - movementSpeedY, 0, 0);
                float4 xz = float4((worldPos.x * _Scale) - movementSpeedX, (worldPos.z * _Scale) - movementSpeedZ, 0, 0);
                float4 yz = float4((worldPos.y * _Scale) - movementSpeedY, (worldPos.z * _Scale) - movementSpeedZ, 0, 0);

                float4 noiseXY = SAMPLE_TEXTURE2D_LOD(_Noise, sampler_Noise, xy.xy, 0);
                float4 noiseXZ = SAMPLE_TEXTURE2D_LOD(_Noise, sampler_Noise, xz.xy, 0);
                float4 noiseYZ = SAMPLE_TEXTURE2D_LOD(_Noise, sampler_Noise, yz.xy, 0);

                float3 worldNormalS = saturate(pow(worldNormal * 1.4, 4));
                o.noiseComb = lerp(noiseXY, noiseXZ, worldNormalS.y);
                o.noiseComb = lerp(o.noiseComb, noiseYZ, worldNormalS.x);

                v.positionOS.xyz += (v.normalOS * (o.noiseComb * _Height)).xyz;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.worldPos = worldPos;
                o.viewDirWS = GetCameraPositionWS() - worldPos;
                o.normalWS = worldNormal;
                o.col = lerp(_CloudColor, _TColor, v.positionOS.y);

                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half rim = 1.0 - saturate(dot(normalize(i.viewDirWS), i.normalWS * (i.noiseComb * _Strength)));
                half3 emission = _RimColor.rgb * pow(rim, _RimPower);
                half3 albedo = i.col.rgb * _Color.rgb;
                return half4(albedo + emission, 1.0);
            }

            ENDHLSL
        }
    }
}
