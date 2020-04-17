#ifndef UNIVERSAL_UNLIT_INPUT_INCLUDED
#define UNIVERSAL_UNLIT_INPUT_INCLUDED

#include "SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
float4 _MainTex_ST;
half _Cutoff;
half _Glossiness;
half _Metallic;
CBUFFER_END

#endif