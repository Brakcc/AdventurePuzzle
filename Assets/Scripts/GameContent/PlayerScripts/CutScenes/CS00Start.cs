using System.Collections;

namespace GameContent.PlayerScripts.CutScenes
{
    public sealed class CS00Start : CutScene
    {
        #region constructor
        
        public CS00Start(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnStartCutScene()
        {
            
        }

        public override IEnumerator HandleCutScene()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEndCutScene()
        {
            
        }
        
        #endregion
    }
}