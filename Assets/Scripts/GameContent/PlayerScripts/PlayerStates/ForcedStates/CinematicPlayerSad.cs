using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates.ForcedStates
{
    public class CinematicPlayerSad : AbstractPlayerState
    {
        #region constructor
        
        public CinematicPlayerSad(GameObject go, ControllerState state, PlayerStateMachine playerMachine) : base(go, state, playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnEnterState()
        {
            AnimationManager.SetAnims("isWalking", false);
            AnimationManager.SetAnims("sad");
        }

        public override void OnExitState()
        {
            
        }
        
        #endregion
    }
}