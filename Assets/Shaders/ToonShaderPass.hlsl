#ifndef MY_TOON_SHADER_INCLUDE
#define MY_TOON_SHADER_INCLUDE


#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
// See ShaderVariablesFunctions.hlsl in com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl



///////////////////////////////////////////////////////////////////////////////
//                      CBUFFER                                              //
///////////////////////////////////////////////////////////////////////////////

/*
Unity URP requires us to set up a CBUFFER
(or "Constant Buffer") of Constant Variables.

These should be the same variables we set up 
in the Properties.

This CBUFFER is REQUIRED for Unity
to correctly handle per-material changes
as well as batching / instancing.

Don't skip it :)
*/

CBUFFER_START(UnityPerMaterial)
    TEXTURE2D(_ColorMap);
    SAMPLER(sampler_ColorMap);
    float4 _ColorMap_ST;
    float3 _Color;
    float _LightIntensity;
    float _Halftone;
    float _HalftoneFactor;
    float _Light0Intensity;
    float _Light1Intensity;
    float _Light2Intensity;
    float _Smoothness;
    float _RimSharpness;
    float3 _RimColor;
    float3 _WorldColor;
CBUFFER_END


///////////////////////////////////////////////////////////////////////////////
//                      STRUCTS                                              //
///////////////////////////////////////////////////////////////////////////////

/*
Our attributes struct is simple.
It contains the Object-Space Position
and Normal Direction as well as the 
UV0 coordinates for the mesh.

The Attributes struct is passed 
from the GPU to the Vertex function.
*/

struct Attributes
{
    float4 positionOS : POSITION;
    float3 normalOS   : NORMAL;
    float2 uv         : TEXCOORD0;
    
    // This line is required for VR SPI to work.
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

/*
The Varyings struct is also straightforward.
It contains the Clip Space Position, the UV, and 
the World-Space Normals.

The Varyings struct is passed from the Vertex
function to the Fragment function.
*/
struct Varyings
{
    float4 positionHCS     : SV_POSITION;
    float2 uv              : TEXCOORD0;
    float3 positionWS      : TEXCOORD1;
    float3 normalWS        : TEXCOORD2;
    float3 viewDirectionWS : TEXCOORD3;
    
    // This line is required for VR SPI to work.
	UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};


///////////////////////////////////////////////////////////////////////////////
//                      Common Lighting Transforms                           //
///////////////////////////////////////////////////////////////////////////////

// This is a global variable, Unity sets it for us.
float3 _LightDirection;


/*
This is a simple lighting transformation.
Normally, we just return the WorldToHClip position.
During the Shadow Pass, we want to make sure that Shadow Bias is baked 
in to the shadow map. To accomplish this, we use the ApplyShadowBias
method to push the world-space positions in their normal direction by the bias amount.
We define SHADOW_CASTER_PASS during the setup for the Shadow Caster pass.
*/

float4 GetClipSpacePosition(float3 positionWS, float3 normalWS)
{
    #if defined(SHADOW_CASTER_PASS)
        float4 positionHCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
        
        #if UNITY_REVERSED_Z
            positionHCS.z = min(positionHCS.z, positionHCS.w * UNITY_NEAR_CLIP_VALUE);
        #else
            positionHCS.z = max(positionHCS.z, positionHCS.w * UNITY_NEAR_CLIP_VALUE);
        #endif
        
        return positionHCS;
    #endif
    
    return TransformWorldToHClip(positionWS);
}


/*
These two functions give us the shadow coordinates 
depending on whether screen shadows are enabled or not.
We have two methods here, one with two args (positionWS
and positionHCS), and one with just positionWS.
The two-arg method is faster when you have 
already calculated the positionHCS variable.
*/

float4 GetMainLightShadowCoord(float3 positionWS, float4 positionHCS)
{
    #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)
        return ComputeScreenPos(positionHCS);
    #else
        return TransformWorldToShadowCoord(positionWS);
    #endif
}

float4 GetMainLightShadowCoord(float3 PositionWS)
{
    #if defined(_MAIN_LIGHT_SHADOWS_SCREEN)
        float4 clipPos = TransformWorldToHClip(PositionWS);
        return ComputeScreenPos(clipPos);
    #else
		return TransformWorldToShadowCoord(PositionWS);
    #endif
}


/*
This method gives us the main light as an out parameter.
The Light struct is defined in 
"Packages/com.unity.render-pipelines.universal/ShaderLibrary/RealtimeLights.hlsl",
so you can reference it there for more details on its fields.
This version of the GetMainLight method doesn't account for Light Cookies.

To account for Light cookies, you need to add the following line to your shader pass:
#pragma multi_compile _ _LIGHT_COOKIES

and also call a different GetMainLight method:
GetMainLight(float4 shadowCoord, float3 positionWS, half4 shadowMask)
*/

void GetMainLightData(float3 PositionWS, out Light light)
{
    float4 shadowCoord = GetMainLightShadowCoord(PositionWS);
    light = GetMainLight(shadowCoord);
}

void GetAdditionalLightData(float3 PositionWS, out Light addLight)
{
    int count = GetAdditionalLightsCount();
    addLight = GetAdditionalLight(0, PositionWS);
    for (int i = 1; i < count; i++)
    {
        Light add = GetAdditionalLight(i, PositionWS);
        addLight.direction += add.direction;
        //if (add.direction != 0)
        //{
        addLight.color += add.color;
        addLight.distanceAttenuation += add.distanceAttenuation;
        addLight.shadowAttenuation += add.shadowAttenuation;
        //}
    }
}
    
///////////////////////////////////////////////////////////////////////////////
//                      Helper Functions                                     //
///////////////////////////////////////////////////////////////////////////////
    
float easysmoothstep(float min, float x)
{
    return smoothstep(min, min + 0.01, x);
}

bool checksmoothstep(float min, float x)
{
    return min < x && x < min + 0.01;
}

float lightingCalc(float NoL, float divFactor, float halftoneValue)
{
    if (NoL >= divFactor + _HalftoneFactor)
    {
        return easysmoothstep(divFactor + _HalftoneFactor - 0.01, NoL);
    }
    if (NoL > divFactor)
    {
        // if (checksmoothstep(divFactor, NoL))
        // {
        //     return easysmoothstep(divFactor + 0.01, NoL);
        // }
        return divFactor + halftoneValue;
    }
    return easysmoothstep(divFactor, NoL);
}

float lightIntensityCalc(float lighting)
{
    if (lighting == 0 && _LightIntensity < 0)
    {
        return 0;
    }
    return _LightIntensity;
}

///////////////////////////////////////////////////////////////////////////////
//                      Functions                                            //
///////////////////////////////////////////////////////////////////////////////

/*
The Vertex function is responsible 
for generating and manipulating the 
data for each vertex of the mesh.
*/

Varyings Vertex(Attributes IN)
{
    Varyings OUT = (Varyings)0;
    
    // These macros are required for VR SPI compatibility
    UNITY_SETUP_INSTANCE_ID(IN);
	UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
    
    
    // Set up each field of the Varyings struct, then return it.
    OUT.positionWS = mul(unity_ObjectToWorld, IN.positionOS).xyz;
    OUT.normalWS = NormalizeNormalPerPixel(TransformObjectToWorldNormal(IN.normalOS));
    OUT.positionHCS = GetClipSpacePosition(OUT.positionWS, OUT.normalWS);
    OUT.viewDirectionWS = normalize(GetWorldSpaceViewDir(OUT.positionWS));
    OUT.uv = TRANSFORM_TEX(IN.uv, _ColorMap);
    
    return OUT;
}


/*
The FragmentDepthOnly function is responsible 
for handling per-pixel shading during the 
DepthOnly and ShadowCaster passes.
*/

float FragmentDepthOnly(Varyings IN) : SV_Target
{
    // These macros are required for VR SPI compatibility
    UNITY_SETUP_INSTANCE_ID(IN);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    
    return 0;
}


/*
The FragmentDepthNormalsOnly function is responsible 
for handling per-pixel shading during the 
DepthNormalsOnly pass. This pass is less common, but
can be required by some post-process effects such as SSAO.
*/

float4 FragmentDepthNormalsOnly(Varyings IN) : SV_Target
{
    // These macros are required for VR SPI compatibility
    UNITY_SETUP_INSTANCE_ID(IN);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    return float4(normalize(IN.normalWS), 0);
}


/*
The Fragment function is responsible 
for handling per-pixel shading during the Forward 
rendering pass. We use the ForwardOnly pass, so this works
by default in both Forward and Deferred paths.
*/

float3 Fragment(Varyings IN) : SV_Target
{
    // These macros are required for VR SPI compatibility
	UNITY_SETUP_INSTANCE_ID(IN);
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
    
    IN.normalWS = normalize(IN.normalWS);
    IN.viewDirectionWS = normalize(IN.viewDirectionWS);
    
    Light light;
    Light addLight, addLight0, addLight1, addLight2;
    GetMainLightData(IN.positionWS, light);
    if (GetAdditionalLightsCount() > 0)
    {
        GetAdditionalLightData(IN.positionWS, addLight);
    }
    
    float NoL = dot(IN.normalWS, light.direction);
    //float addNoL = dot(IN.normalWS, addLight.direction); //add
    //float addToonLighting = easysmoothstep(_OtherLightsIntensity, addNoL); //add
    
    //float toonLighting = easysmoothstep(0, NoL);
    float toonLighting = lightingCalc(NoL, 0, _Halftone);
    //toonLighting = smoothstep(0, 1, NoL);
    float toonShadows = easysmoothstep(0.5, light.shadowAttenuation);
    
    float3 halfVector = normalize(light.direction + IN.viewDirectionWS);
    float NoH = max(dot(IN.normalWS, halfVector), 0);
    float specularTerm = pow(NoH, _Smoothness * _Smoothness);
    specularTerm *= toonLighting * toonShadows;
    
    //disables specular in halftone area if halftone is turned on
    if (NoL > 0 && NoL < _HalftoneFactor && _Halftone != 1)
    {
        specularTerm = 0; 
    } else
    {
        specularTerm = easysmoothstep(0.01, specularTerm);
    }
    
    float NoV = max(dot(IN.normalWS, IN.viewDirectionWS), 0);
    float rimTerm = pow(1.0 - NoV, _RimSharpness);
    rimTerm *= toonLighting * toonShadows;
    rimTerm = easysmoothstep(0.01, rimTerm);
    
    float3 surfaceColor = _Color * SAMPLE_TEXTURE2D(_ColorMap, sampler_ColorMap, IN.uv);

    //float3 addLighting = addToonLighting * addLight.color;
    float3 directionalLighting = toonLighting * toonShadows * light.color;
    directionalLighting += lightIntensityCalc(directionalLighting);
    float3 specularLighting = specularTerm * light.color;
    float3 rimLighting = rimTerm * _RimColor;
    
    float3 finalLighting = float3(0,0,0);
    finalLighting += _WorldColor;
    finalLighting += directionalLighting;
    finalLighting += specularLighting;
    finalLighting += rimLighting;

    float3 addLighting0, addLighting1, addLighting2;
    if (GetAdditionalLightsCount() > 0)
    {
        addLight0 = GetAdditionalLight(0, IN.positionWS);
        float addNoL = dot(IN.normalWS, addLight0.direction); //add
        float addToonLighting = easysmoothstep(_Light0Intensity, addNoL); //add
        float addToonShadows = easysmoothstep(0, addLight0.shadowAttenuation);
        addLighting0 = addToonLighting * addToonShadows * addLight0.color;
        finalLighting += addLighting0;
        
        if (GetAdditionalLightsCount() > 1)
        {
            addLight1 = GetAdditionalLight(1, IN.positionWS);
            float addNoL = dot(IN.normalWS, addLight1.direction); //add
            float addToonLighting = easysmoothstep(_Light1Intensity, addNoL); //add
            float addToonShadows = easysmoothstep(0, addLight1.shadowAttenuation);
            addLighting1 = addToonLighting * addToonShadows * addLight1.color;
            finalLighting += addLighting1;

            if (GetAdditionalLightsCount() > 2)
            {
                addLight2 = GetAdditionalLight(1, IN.positionWS);
                float addNoL = dot(IN.normalWS, addLight2.direction); //add
                float addToonLighting = easysmoothstep(_Light2Intensity, addNoL); //add
                float addToonShadows = easysmoothstep(0, addLight2.shadowAttenuation);
                addLighting2 = addToonLighting * addToonShadows * addLight2.color;
                finalLighting += addLighting2;
            }
        }
    }
    
    //finalLighting += addLighting;
    
    return surfaceColor * finalLighting;
}
#endif