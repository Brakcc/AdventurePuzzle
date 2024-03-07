using System;
using UnityEngine;

public class InteractibleCheck : MonoBehaviour
{
    public AbstractInteractibleBase interactiblesActivated;
    private void OnCollisionEnter(Collision possibleInteractible)
    {
        if (possibleInteractible.transform.CompareTag("Interactible"))
        {
            interactiblesActivated = possibleInteractible.gameObject.GetComponent<AbstractInteractibleBase>();
            interactiblesActivated.OnSubscribe();
        }
    }

    private void OnCollisionExit(Collision possibleInteractible)
    {
        if (possibleInteractible.transform.CompareTag("Interactible"))
        {
            interactiblesActivated.OnUnSubscribe();
            interactiblesActivated = null;
        }
    }
}
