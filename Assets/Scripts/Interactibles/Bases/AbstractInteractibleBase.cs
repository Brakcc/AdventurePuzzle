using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractInteractibleBase : MonoBehaviour
{
    #region Methodes a hériter

    public virtual void OnActivated(){}
    public virtual void OnDeactivated(){}
    public virtual void Effect(){}

    
    public void Activate()
    {
        OnActivated();
        Activated = true;
    }
    private void FixedUpdate()
    {
        bool iAmHere = false;
        if (theInteractibleChecker.interactiblesActivated.Any())
        {
            foreach (GameObject possibleMe in theInteractibleChecker.interactiblesActivated)
            {
                if (possibleMe == gameObject)
                {
                    if (!Activated)
                    {
                        Activate();
                    }
                    iAmHere = true;
                }
            }
        }

        if (!iAmHere)
        {
            if (Activated)
            {
                OnDeactivated();
            }
            Activated = false;
        }
        
        
        
        if (Activated)
        {
            Effect();
        }
    }

    #endregion
    
    #region Variables/Fields
    
    protected bool Activated = false;

    public InteractibleCheck theInteractibleChecker;

    #endregion
}
