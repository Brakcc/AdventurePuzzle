using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates.ForcedStates
{
    public class CinematicPlayerTurn : AbstractPlayerState
    {
        #region constructor
        
        public CinematicPlayerTurn(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnEnterState()
        {
            AnimationManager.SetAnims("look");
        }

        public override void OnExitState()
        {
            
        }
        
        #endregion
    }
}