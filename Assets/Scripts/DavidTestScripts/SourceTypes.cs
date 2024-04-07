using System.Collections.Generic;
using UnityEngine;

public class SourceTypes : MonoBehaviour
{
    public enum SourceType
    {
        Yellow,
        Green,
        Blue,
        NoEnergy
    };

    [SerializeField] private List<Material> myMaterials;
    public static List<Material> sourceMaterials;
    public static Material SetUpEnergyMaterial(SourceType mySourceType)
    {
        int textureIndicator = 0;
        switch (mySourceType)
        {
            case SourceType.Blue:
                textureIndicator = 1;
                break;
            case SourceType.Green:
                textureIndicator = 2;
                break;
            case SourceType.NoEnergy:
                textureIndicator = 3;
                break;
        }
        return sourceMaterials[textureIndicator];
    }
    private void Awake()
    {
        sourceMaterials = myMaterials;
    }
}
