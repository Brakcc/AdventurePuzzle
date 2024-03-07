using System;
using UnityEngine;

public abstract class AbstractInteractibleBase : MonoBehaviour
{
    #region methodes

    private void Start() => isActivated = false;

    #endregion
    
    #region Methodes a hériter
    
    public abstract void OnSubscribe();
    public abstract void OnUnSubscribe();
    protected virtual void Effect() => isActivated = !isActivated; 
    
    #endregion
    
    
    #region Variables/Fields
    
    protected bool isActivated;

    #endregion
}
