using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractibleCheck : MonoBehaviour
{
    public List<GameObject> interactiblesActivated = new List<GameObject>();
    private void OnCollisionEnter(Collision possibleInteractible)
    {
        if (possibleInteractible.transform.CompareTag("Interactible"))
        {
            interactiblesActivated.Add(possibleInteractible.gameObject);
        }
    }
    
    private void OnCollisionExit(Collision possibleInteractible)
    {
        if (possibleInteractible.transform.CompareTag("Interactible"))
        {
            interactiblesActivated.Remove(possibleInteractible.gameObject);
        }
    }
}
