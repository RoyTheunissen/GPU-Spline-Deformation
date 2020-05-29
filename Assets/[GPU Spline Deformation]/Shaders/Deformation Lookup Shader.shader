Shader "Custom/Deformation Lookup Shader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        [Space]
        _DeformationAlongSplineTex ("Deformation Along Spline", 2D) = "black" {}
        _ZStart ("Z Start", Float) = 0
        _ZEnd ("Z End", Float) = 1
        _Speed ("Speed", Float) = 0
        _Amount ("Amount", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _DeformationAlongSplineTex;

        struct Input
        {
            float2 uv_MainTex;
            float influence;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        float _ZStart;
        float _ZEnd;
        float _Speed;
        float _Amount;
        float4x4 _TestMatrix;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)
        
        float4x4 GetMatrix(float x)
        {
            float offset = 0; 
            float4 row0 = tex2Dlod(_DeformationAlongSplineTex, float4(x, 0, 0, 0));
            float4 row1 = tex2Dlod(_DeformationAlongSplineTex, float4(x, 1.0 / 3.0, 0, 0));
            float4 row2 = tex2Dlod(_DeformationAlongSplineTex, float4(x, 2.0 / 3.0, 0, 0));
            float4 row3 = tex2Dlod(_DeformationAlongSplineTex, float4(x, 1, 0, 0));
            return float4x4(row0, row1, row2, row3);
        }
        
        void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
        
            float coordinate = saturate((v.vertex.z - _ZStart) / (_ZEnd - _ZStart));
            coordinate = coordinate + _Time.y * _Speed;
            
            float4x4 m = GetMatrix(coordinate);
            v.vertex = lerp(v.vertex, mul(m, float4(v.vertex.x, v.vertex.y, 0, v.vertex.w)), _Amount);
            v.normal = lerp(v.normal, normalize(mul(m, float4(v.normal.xyz, 0)).xyz), _Amount);
            o.influence = coordinate;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
