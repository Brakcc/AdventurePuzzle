using GameContent.PlayerScripts;
using GameContent.PlayerScripts.CutScenes;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates
{
    [RequireComponent(typeof(Collider))]
    public class CinematicTrigger : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            var p = other.GetComponent<PlayerStateMachine>();
            
            OnStartCinematic(p);
        }

        private void OnStartCinematic(PlayerStateMachine player)
        {
            switch (animationID)
            {
                case 0:
                    var cs0 = new CS00Start(player);
                    cs0.OnStartCutScene();
                    StartCoroutine(cs0.HandleCutScene());
                    break;
                case 1:
                    var cs1 = new CS01CompanionDeath(player, startPos, endPos);
                    cs1.OnStartCutScene();
                    StartCoroutine(cs1.HandleCutScene());
                    break;
                case 2:
                    var cs2= new CS02Ending(player);
                    cs2.OnStartCutScene();
                    StartCoroutine(cs2.HandleCutScene());
                    break;
            }
        }
        
        #endregion

        #region fields

        [SerializeField] private int animationID;

#pragma warning disable CS0414 // Field is assigned but its value is never used
        [SerializeField] private bool needSpecCoords;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        [ShowIfBoolTrue("needSpecCoords")] [SerializeField]
        private Transform startPos;
        
        [ShowIfBoolTrue("needSpecCoords")] [SerializeField]
        private Transform endPos;

        #endregion
    }
}