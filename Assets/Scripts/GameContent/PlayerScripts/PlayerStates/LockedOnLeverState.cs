using System;
using GameContent.Interactives.ClemInterTemplates.Levers;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnLeverState : AbstractPlayerState
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
            if (_leverRef!.LeverOrientationMode == LeverOrientationMode.Horizontal)
                _leverRef.ImageD.SetActive(true);
            else
                _leverRef.ImageF.SetActive(true);

            _canManip = false;
            _canReloadManip = true;
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _stateMachine = null;

            _leverRef.ImageD.SetActive(false);
            _leverRef.ImageF.SetActive(false);
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
            if (_canManip && _inputDir.magnitude > Constants.MinLeverInputThreshold)
            {
                _canManip = false;
                _canReloadManip = true;

                switch (_leverRef.LeverOrientationMode)
                {
                    case LeverOrientationMode.Horizontal:
                        switch (_inputDir.x)
                        {
                            case >= Constants.MinLeverInputThreshold:
                                _leverRef.Level++;
                                break;
                            case <= -Constants.MinLeverInputThreshold:
                                _leverRef.Level--;
                                break;
                        }
                        break;
                    case LeverOrientationMode.Vertical:
                        switch (_inputDir.z)
                        {
                            case >= Constants.MinLeverInputThreshold:
                                _leverRef.Level++;
                                break;
                            case <= -Constants.MinLeverInputThreshold:
                                _leverRef.Level--;
                                break;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{_leverRef.LeverOrientationMode} ah", "well shit...");
                }
            
                //Debug.Log($"{_leverRef.name} + {_leverRef.Level}");
            }

            
            if (!_canReloadManip || !(_inputDir.magnitude <= Constants.ReloadLeverManipThreshold))
                return;
            
            _canManip = true;
            _canReloadManip = false;
        }
        
        #endregion
        
        #endregion

        #region fields
        
        private LeverInter _leverRef;

        private bool _canManip;

        private bool _canReloadManip;

        #endregion
    }
}