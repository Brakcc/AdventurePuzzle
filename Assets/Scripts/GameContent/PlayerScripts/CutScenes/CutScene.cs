using System.Collections;

namespace GameContent.PlayerScripts.CutScenes
{
    public abstract class CutScene
    {
        #region constructor
        
        protected CutScene(PlayerStateMachine playerMachine)
        {
            this.playerMachine = playerMachine;
        }
        
        #endregion

        #region methodes

        public abstract void OnStartCutScene();

        public abstract IEnumerator HandleCutScene();

        public abstract void OnEndCutScene();

        #endregion
        
        #region fiedls

        protected PlayerStateMachine playerMachine;

        #endregion
    }
}