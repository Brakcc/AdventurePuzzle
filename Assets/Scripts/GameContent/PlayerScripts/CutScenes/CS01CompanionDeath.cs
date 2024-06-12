using System.Collections;
using DG.Tweening;
using GameContent.Narration.Creature;
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
            playerMachine.Machine.cine = true;
            playerMachine.Machine.ForceState("cineMove");
        }

        public override IEnumerator HandleCutScene()
        {
            //Continue walking
            playerMachine.transform.rotation = Quaternion.LookRotation(targetPos.position - playerMachine.transform.position, Vector3.up);
            upperRec.DOLocalMoveY(200, 1);
            lowerRec.DOLocalMoveY(-200, 1);
            
            //Crea die while walking
            creatureMachine.IsDedge = true;
            creatureMachine.SetAnims("isMoving", false);
            while (Vector3.Distance(playerMachine.transform.position, targetPos.position) >= 1f)
            {
                playerMachine.transform.position = Vector3.MoveTowards(playerMachine.transform.position, 
                                                                       targetPos.position, 
                                                                       Time.fixedDeltaTime * 5);
                yield return new WaitForFixedUpdate();
            }
            
            //Switch To idle
            creatureMachine.OnDie();
            playerMachine.Machine.ForceState("cineIdle");
            yield return new WaitForSeconds(2.5f);
            
            //Turn back
            playerMachine.Machine.ForceState("cineMove");
            playerMachine.transform.rotation = Quaternion.LookRotation(startPos.position - playerMachine.transform.position, Vector3.up);
            playerMachine.transform.position = Vector3.MoveTowards(playerMachine.transform.position, 
                                                                   startPos.position, 
                                                                   Time.fixedDeltaTime * 5);
            //Wait
            playerMachine.Machine.ForceState("cineIdle");
            yield return new WaitForSeconds(2f);
            
            //Go back to see creature
            playerMachine.Machine.ForceState("cineMove");
            while (Vector3.Distance(playerMachine.transform.position, startPos.position) >= 1f)
            {
                playerMachine.transform.position = Vector3.MoveTowards(playerMachine.transform.position, 
                                                                       startPos.position, 
                                                                       Time.fixedDeltaTime * 5);
                yield return new WaitForFixedUpdate();
            }
            
            //Wait and look
            playerMachine.Machine.ForceState("cineIdle");
            yield return new WaitForSeconds(2.5f);
            
            upperRec.DOLocalMoveY(250, 1);
            lowerRec.DOLocalMoveY(-250, 1);
            
            OnEndCutScene();
        }

        public override void OnEndCutScene()
        {
            playerMachine.Machine.ForceState("idle");
            playerMachine.Machine.cine = false;
        }
        
        #endregion

        #region fields

        [SerializeField] private CreatureStateMachine creatureMachine;
        
        [SerializeField] private Transform startPos;

        [SerializeField] private Transform targetPos;

        [SerializeField] private RectTransform upperRec;
        
        [SerializeField] private RectTransform lowerRec;

        #endregion
    }
}