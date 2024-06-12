using System;
using GameContent.Interactives.ClemInterTemplates.Levers;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnLeverState : AbstractPlayerState
    {
        #region constructor
        
        public LockedOnLeverState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }
        
        #endregion

        #region methodes
        
        public override void OnEnterState()
        {
            AnimationManager.SetAnims("isLevier");
            
            _leverRef = _checker.InterRef as LeverInter;
            /*if (_leverRef!.LeverOrientationMode == LeverOrientationMode.Horizontal)
                _leverRef.ImageD.SetActive(true);
            else
                _leverRef.ImageF.SetActive(true);*/

            _canManip = false;
            _canReloadManip = true;
            _unNormalizedInput = Vector2.zero;
        }

        public override void OnExitState()
        {
            /*_leverRef.ImageD.SetActive(false);
            _leverRef.ImageF.SetActive(false);*/
            _leverRef = null;
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            _unNormalizedInput = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(_unNormalizedInput.x, 0, _unNormalizedInput.y).normalized;
            
            GetHoldInput();

            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            OnLeverManip();

            return 0;
        }

        #region holding methodes

        private void GetHoldInput()
        {
            if (_datasSo.interactInput.action.IsPressed())
                return;
            
            stateMachine.SwitchState("move");
        }

        private void OnLeverManip()
        {
            if (_canManip && _unNormalizedInput.magnitude > Constants.MinLeverInputThreshold)
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

        private Vector2 _unNormalizedInput;
        
        private bool _canManip;

        private bool _canReloadManip;

        #endregion
    }
}