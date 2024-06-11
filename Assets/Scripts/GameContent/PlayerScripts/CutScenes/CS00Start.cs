using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS00Start : CutScene
    {
        #region constructor
        
        public CS00Start(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        private void Awake()
        {
            StartCoroutine(HandleCutScene());
        }

        public override void OnStartCutScene()
        {
            playerMachine.Machine.ForceState("cineIdle");
        }

        public override IEnumerator HandleCutScene()
        {
            yield return new WaitForSeconds(2f);
            OnStartCutScene();
            
            blackScreen.DOColor(Color.clear, 1);
            yield return new WaitForSeconds(4f);

            plan1.DOMove(new Vector3(-1920, 540, 0), 2);
            yield return new WaitForSeconds(5f);
            
            plan2.DOMove(new Vector3(-1920, 540, 0), 2);
            yield return new WaitForSeconds(5f);
            
            plan3.DOMove(new Vector3(-1920, 540, 0), 2);
            yield return new WaitForSeconds(5f);
            
            plan4.DOMove(new Vector3(-1920, 540, 0), 2);
            yield return new WaitForSeconds(5f);
            
            OnEndCutScene();
        }

        public override void OnEndCutScene()
        {
            playerMachine.Machine.ForceState("idle");
        }
        
        #endregion

        #region fields

        [SerializeField] private Image blackScreen;

        [SerializeField] private RectTransform plan1;
        
        [SerializeField] private RectTransform plan2;
        
        [SerializeField] private RectTransform plan3;
        
        [SerializeField] private RectTransform plan4;

        #endregion
    }
}