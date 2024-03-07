using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;

public class Interactible : AbstractInteractibleBase
{
    //Ceci est un intéractible test pas un modèle
    
    #region Les Methodes

    public override void OnSubscribe()
    {
        Debug.Log("Entered");
        ApplyState.OnApply += Effect;
    }

    protected override void Effect()
    {
        Debug.Log(isActivated ? "eee" : "aaa");
        base.Effect();
    }

    public override void OnUnSubscribe()
    {
        Debug.Log("Deactivated");
        ApplyState.OnApply -= Effect;
    }
    
    #endregion
    
    #region Les Variables/Fields
    #endregion
}
