
//probleme que j'avais eu: je n'avais pas mis _float apres le nom de la fonction
void GetLights_float (out float3 mainLightDirection, out float3 mainLightColor, out float3 secondaryLightDirection, out float3 secondaryLightColor) 
{
    #if defined(SHADERGRAPH_PREVIEW)
        mainLightDirection = float3(0.5, 0.5, 0);
        mainLightColor = float3(1, 1, 1);
        secondaryLightDirection = float3(0.5, 0.5, 0);
        secondaryLightColor = float3(1, 1, 1);
    #else
    
        
        float4 lightColor;
        float4 lightPos;
        

        GetMainLight( mainLightDirection, mainLightColor);

        
        GetSecondaryLight(0,  lightColor, lightPos);
        secondaryLightDirection = normalize(lightPos.xyz);
        secondaryLightColor = lightColor.rgb;
    #endif
}