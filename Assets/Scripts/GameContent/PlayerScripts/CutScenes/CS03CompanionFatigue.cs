using System.Collections;
using GameContent.Narration.Creature;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS03CompanionFatigue : CutScene
    {
        #region constructor
        
        public CS03CompanionFatigue(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnStartCutScene()
        {
            creatureMachine.IsSlower = true;
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