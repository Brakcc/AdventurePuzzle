using System.Collections.Generic;
using UnityEngine;
using static SourceTypes;

public class Emetteur : MonoBehaviour
{
    [SerializeField] private List<GameObject> recepteur = new List<GameObject>();
    
    
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && other.CompareTag("Player"))
        {
            SourceType playerType = other.GetComponent<PlayerEnergy>().playerSourceType;
            if (playerType != SourceType.NoEnergy)
            {
                GetComponent<Renderer>().material = SetUpEnergyMaterial(playerType);
                AffectObject(playerType);
                
                other.GetComponent<PlayerEnergy>().playerSourceType = SourceType.NoEnergy;
                other.GetComponent<Renderer>().material = SetUpEnergyMaterial(SourceType.NoEnergy);
            }
        }
    }

    void AffectObject(SourceType typeOfEnergy)
    {
        foreach (var objectToAffect in recepteur)
        {
            objectToAffect.GetComponent<BoxCollider>().enabled = typeOfEnergy != SourceType.Green;
            objectToAffect.GetComponent<Renderer>().material = SetUpEnergyMaterial(typeOfEnergy);
            objectToAffect.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            objectToAffect.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            objectToAffect.GetComponent<Rigidbody>().constraints = typeOfEnergy is not SourceType.Blue ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.FreezePositionY;
            
            //Ajouter Activation pour Jaune
        }
       
    }
    
}
