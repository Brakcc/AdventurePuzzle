using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("CHARA")]
    [field: SerializeField] public EventReference compagnonVoix { get; private set; }
    [field: SerializeField] public EventReference cosmosVoix { get; private set; }
    [field: SerializeField] public EventReference cosmosGrab { get; private set; }
    [field: SerializeField] public EventReference cosmosPas { get; private set; }
    [field: SerializeField] public EventReference compagnonPas { get; private set; }
    [field: SerializeField] public EventReference cosmosChute { get; private set; }
    [field: SerializeField] public EventReference compagnonChute { get; private set; }
    
    
    [field: Header("ENERGIE")]
    [field: SerializeField] public EventReference energieRecup { get; private set; }
    [field: SerializeField] public EventReference energieDeposee { get; private set; }
    [field: SerializeField] public EventReference energieJetee { get; private set; }
    [field: SerializeField] public EventReference palierRemplissage { get; private set; }
    [field: SerializeField] public EventReference palierVidange { get; private set; }
    [field: SerializeField] public EventReference preOnde { get; private set; }
    [field: SerializeField] public EventReference onde { get; private set; }
    
    
    [field: Header("ELEMENTS")]
    [field: SerializeField] public EventReference TPemprunte { get; private set; }
    [field: SerializeField] public EventReference manivelleTourneeDroite { get; private set; }
    [field: SerializeField] public EventReference manivelleTourneeGauche { get; private set; }
    [field: SerializeField] public EventReference elementChute { get; private set; }
    [field: SerializeField] public EventReference elementDeplace { get; private set; }
    
    
    [field: Header("MENUS")]
    [field: SerializeField] public EventReference UIclick { get; private set; }
    [field: SerializeField] public EventReference UIclickStart { get; private set; }
    [field: SerializeField] public EventReference UIpauseOuvre { get; private set; }
    [field: SerializeField] public EventReference UIpauseFerme { get; private set; }
    [field: SerializeField] public EventReference carnetPageTourne { get; private set; }
    
    
    [field: Header("AMBIANCE")]
    [field: SerializeField] public EventReference atmoForêt { get; private set; }
    [field: SerializeField] public EventReference atmoSerre { get; private set; }
    [field: SerializeField] public EventReference atmoSerrePostMort { get; private set; }
    [field: SerializeField] public EventReference atmoForetPostMort { get; private set; }
    [field: SerializeField] public EventReference riviere { get; private set; }
    [field: SerializeField] public EventReference vaisseauAtterrissage { get; private set; }
    [field: SerializeField] public EventReference vaisseauDecollage { get; private set; }
    [field: SerializeField] public EventReference recupPlanteMachineShutDown { get; private set; }
    
    
    [field: Header("MUSIQUES")]
    [field: SerializeField] public EventReference musiqueMenu { get; private set; }
    [field: SerializeField] public EventReference musiqueZones { get; private set; }
    [field: SerializeField] public EventReference jingleRecupPlante { get; private set; }
    [field: SerializeField] public EventReference cinéCarnetDébut { get; private set; }
    [field: SerializeField] public EventReference cinéCarnetFinCrédits { get; private set; }
    
    
    [field: Header("EFFETS")]
    [field: SerializeField] public EventReference effectGamePaused { get; private set; }
    [field: SerializeField] public EventReference effectEchoSerre { get; private set; }
    
    
    
    public static FMODEvents instance { get; private set; }


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(("euhhhhhhhhhhh... voilà c'était mon mot"));
        }
        instance = this;
    }
}
