﻿using System.Collections;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS05ChangeFogAmbiance : CutScene
    {
        #region constructor
        
        public CS05ChangeFogAmbiance(PlayerStateMachine playerMachine) : base(playerMachine)
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