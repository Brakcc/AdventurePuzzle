using System.Collections;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    public sealed class CS01CompanionDeath : CutScene
    {
        #region constructor
        
        public CS01CompanionDeath(PlayerStateMachine playerMachine, Transform playerStartPos, Transform playerTargetPos) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnStartCutScene()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator HandleCutScene()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEndCutScene()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
    }
}