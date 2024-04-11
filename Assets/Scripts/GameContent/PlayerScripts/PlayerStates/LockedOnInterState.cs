using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnInterState : AbstractPlayerState
    {
        public LockedOnInterState(GameObject go) : base(go)
        {
        }

        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            throw new System.NotImplementedException();
        }
    }
}