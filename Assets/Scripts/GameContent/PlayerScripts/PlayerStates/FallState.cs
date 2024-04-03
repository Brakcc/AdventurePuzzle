using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class FallState : AbstractPlayerState
    {
        #region constructor

        public FallState(GameObject go) : base(go)
        {
        }

        #endregion
        
        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion

        #region fields

        

        #endregion
    }
}