using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS02Ending : CutScene
    {
        #region constructor
        
        public CS02Ending(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnStartCutScene()
        {
            playerMachine.Machine.ForceState("cineIdle");
        }

        public override IEnumerator HandleCutScene()
        {
            blackScreen.DOColor(Color.black, 1);
            yield return new WaitForSeconds(4.1f);

            endScreen.DOColor(Color.white, 1);
            yield return new WaitForSeconds(6.1f);
            
            blackScreen.color = Color.black;
            endScreen.DOColor(Color.clear, 1);
            yield return new WaitForSeconds(4.1f);
            
            OnEndCutScene();
        }

        public override void OnEndCutScene()
        {
            SceneManager.LoadScene("TitleScreen");
        }
        
        #endregion

        #region fields

        [SerializeField] private Image blackScreen;

        [SerializeField] private Image endScreen;

        #endregion
    }
}