using GameContent.Interactives.ClemInterTemplates;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class LockedOnLeverState : AbstractPlayerState
    {
        #region constructor
        
        public LockedOnLeverState(GameObject go) : base(go)
        {
        }
        
        #endregion

        #region methodes
        
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
            _leverRef = _checker.InterRef as LeverInter;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;

            _leverRef = null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            GetHoldInput();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            OnLeverManip();
        }

        #region holding methodes

        private void GetHoldInput()
        {
            if (_datasSo.interactInput.action.IsPressed())
                return;
            
            _stateMachine.OnSwitchState("move");
        }

        private void OnLeverManip()
        {
            
        }
        
        #endregion
        
        #endregion

        #region fields

        private LeverInter _leverRef;

        #endregion
    }
}