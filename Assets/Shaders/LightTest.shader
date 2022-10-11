Shader "Unlit/LightTestMulti"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Gloss ("Gloss", Range(0, 1)) = 1
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}

        // Base pass.
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "FGLightning.cginc"
            ENDCG
        }

        // // Add pass.
        // Pass
        // {
        //     Tags {"LightMode" = "ForwardAdd"}

        //     Blend One One // src*1 + dst*1
        //     CGPROGRAM
        //     #pragma vertex vert
        //     #pragma fragment frag
        //     #pragma multi_compile_fwdadd

        //     #include "FGLightning.cginc"
        //     ENDCG
        // }
    }
}