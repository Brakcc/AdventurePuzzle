void GetLights_float (out half3 DirectionMain, out half3 ColorMain, out half3 DirectionSecondary, out half3 ColorSecondary)
{
#if defined(SHADERGRAPH_PREVIEW)
    DirectionMain = half3(0.5, 0.5, 0);
    ColorMain = half3(1, 1, 1);
    DirectionSecondary = half3(0.5, 0.5, 0);
    ColorSecondary = half3(1, 1, 1);
#else
    Light light = GetMainLight();
    DirectionMain = light.direction;
    ColorMain = light.color;


    float4 lightColor;
    float4 lightPos;
    GetSecondaryLight(0,  lightColor, lightPos);
        DirectionSecondary = normalize(lightPos.xyz);
        ColorSecondary = lightColor.rgb;
#endif
}