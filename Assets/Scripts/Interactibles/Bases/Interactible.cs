using UnityEngine;

public class Interactible : AbstractInteractibleBase
{
    //Ceci est un intéractible test pas un modèle
    
    #region Les Methodes

    public override void OnActivated()
    {
        Debug.Log("Entered");
    }
    
    public override void Effect()
    {
        Debug.Log("Effective");
    }
    
    public override void OnDeactivated()
    {
        Debug.Log("Deactivated");
    }
    
    #endregion
    
    #region Les Variables/Fields
    #endregion
}
