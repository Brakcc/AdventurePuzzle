using System.Collections;
using GameContent.Narration.Creature;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS04CompanionMeet : CutScene
    {
        #region constructor
        
        public CS04CompanionMeet(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes
        
        public override void OnStartCutScene()
        {
            creatureMachine.CurrentState = 1;
        }

        public override IEnumerator HandleCutScene()
        {
            yield return null;
        }

        public override void OnEndCutScene()
        {
            
        }
        
        #endregion

        #region fields

        [SerializeField] private CreatureStateMachine creatureMachine;

        #endregion
    }
}