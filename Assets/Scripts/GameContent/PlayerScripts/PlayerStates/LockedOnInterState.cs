using System;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnInterState : AbstractPlayerState
    {
        #region properties

        #region TR
        
        private bool PlayerHittingTR => Physics.Linecast(
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector, 
                                             _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             _cc.bounds.extents.z), 
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            -_cc.bounds.extents.z),
            _datasSo.collisionDatasSo.blockMask) || 
                                         Physics.Linecast(
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             -_cc.bounds.extents.z), 
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            _cc.bounds.extents.z), 
            _datasSo.collisionDatasSo.blockMask);
        
        #endregion
        
        #region TL
        
        private bool PlayerHittingTL => Physics.Linecast(
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             _cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector), 
            _cc.bounds.center + new Vector3(-_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            _cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector),
            _datasSo.collisionDatasSo.blockMask) || 
                                        Physics.Linecast(
            _cc.bounds.center + new Vector3(-_cc.bounds.extents.x,
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             _cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector), 
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            _cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector), 
            _datasSo.collisionDatasSo.blockMask);
        
        #endregion
        
        #region BR
        
        private bool PlayerHittingBR => Physics.Linecast(
            _cc.bounds.center + new Vector3(-_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             -(_cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector)), 
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            -(_cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector)),
            _datasSo.collisionDatasSo.blockMask) ||
                                            Physics.Linecast(
            _cc.bounds.center + new Vector3(_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             -(_cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector)), 
            _cc.bounds.center + new Vector3(-_cc.bounds.extents.x, 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            -(_cc.bounds.extents.z + _datasSo.collisionDatasSo.widthCorrector)),
            _datasSo.collisionDatasSo.blockMask);

        #endregion
        
        #region BL
        
        private bool PlayerHittingBL => Physics.Linecast(
            _cc.bounds.center + new Vector3(-(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector), 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             _cc.bounds.extents.z), 
            _cc.bounds.center + new Vector3(-(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector), 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            -_cc.bounds.extents.z),
            _datasSo.collisionDatasSo.blockMask) || 
                                           Physics.Linecast(
            _cc.bounds.center + new Vector3(-(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector), 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                             -_cc.bounds.extents.z), 
            _cc.bounds.center + new Vector3(-(_cc.bounds.extents.x + _datasSo.collisionDatasSo.widthCorrector), 
                                            _datasSo.collisionDatasSo.midHeightCorrector * _cc.bounds.extents.y, 
                                            _cc.bounds.extents.z),
            _datasSo.collisionDatasSo.blockMask);
        
        #endregion
        
        #endregion
        
        #region constructor
        
        public LockedOnInterState(GameObject go) : base(go)
        {
        }
        
        #endregion

        #region methodes

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnterState(PlayerStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            
            _interRef = _checker.InterRef as ReceptorInter;
            
            _recepRefRb = _interRef!.GetComponent<Rigidbody>();
            _recepRefRb.constraints = ReceptorInter.GetRBConstraints(RBCMode.Rota);
            
            _tempDistFromPlayer = _interRef.DistFromPlayer;
            _fallCounter = 0;
            
            _relativePos = GetRelativePos();
            _directionMode = GetBaseMoveDir(_relativePos);
        }

        public override void OnExitState(PlayerStateMachine stateMachine)
        {
            _tempDistFromPlayer = 0;
            _recepRefRb.constraints = ReceptorInter.GetRBConstraints(_interRef.IsOnTop ? RBCMode.Rota : RBCMode.RotaPlan);
            
            _stateMachine = null;
            _interRef = null;
            _recepRefRb = null;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            OnFall();
            GetHoldInput();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            OnMove();
            DistFromInterCheck();
        }

        #region holding methodes

        private void GetHoldInput()
        {
            if (_datasSo.interactInput.action.IsPressed())
                return;
            
            _stateMachine.OnSwitchState("move");
        }

        private void DistFromInterCheck()
        {
            //Debug.Log($"{_interRef.name} and {_interRef.DistFromPlayer} and {_tempDistFromPlayer + Constants.GrabGabThreshold}" );
            
            if (_interRef.DistFromPlayer >= _tempDistFromPlayer + Constants.GrabGapThreshold ||
                !_checker.InRangeInter.Contains(_interRef))
                _stateMachine.OnSwitchState("move");
        }
        
        private static LockDirectionMode GetBaseMoveDir(Vector3 fromVec)
        {
            //can be fromVec = _goRef.transform.position - _interRef.Pivot
            var tempAngle = Vector3.Angle(Vector3.right, fromVec);
            
            return tempAngle switch
            {
                <= Constants.FirstQuarterAngleValue or >= Constants.ThirdQuarterAngleValue => LockDirectionMode.BLToTR,
                < Constants.ThirdQuarterAngleValue or > Constants.FirstQuarterAngleValue => LockDirectionMode.TLToBR,
                _ => LockDirectionMode.None
            };
        }

        private static LockDirectionMode GetBaseMoveDir(RelativeInterPos pos) => pos switch
        {
            RelativeInterPos.TR => LockDirectionMode.BLToTR,
            RelativeInterPos.BR => LockDirectionMode.TLToBR,
            RelativeInterPos.TL => LockDirectionMode.TLToBR,
            RelativeInterPos.BL => LockDirectionMode.BLToTR,
            _ => throw new ArgumentOutOfRangeException(nameof(pos), pos, "et bah non")
        };

        private RelativeInterPos GetRelativePos()
        {
            var tempDotX = Vector2.Dot(new Vector2(_interRef.transform.right.x, _interRef.transform.right.z),
                                       new Vector2((_goRef.transform.position - _interRef.Pivot).x,
                                                   (_goRef.transform.position - _interRef.Pivot).z)) /
                           new Vector2((_goRef.transform.position - _interRef.Pivot).x,
                                       (_goRef.transform.position - _interRef.Pivot).z).magnitude;
            
            var tempDotY = Vector2.Dot(new Vector2(_interRef.transform.forward.x, _interRef.transform.forward.z),
                                       new Vector2((_goRef.transform.position - _interRef.Pivot).x,
                                                   (_goRef.transform.position - _interRef.Pivot).z)) / 
                           new Vector2((_goRef.transform.position - _interRef.Pivot).x,
                                       (_goRef.transform.position - _interRef.Pivot).z).magnitude;

            return (tempDotX, tempDotY) switch
            {
                (>= Constants.PiByFourRadVal, <= Constants.PiByFourRadVal) => RelativeInterPos.TR,
                (<= Constants.PiByFourRadVal, >= Constants.PiByFourRadVal) => RelativeInterPos.TL,
                (<= Constants.PiByFourRadVal, <= -Constants.PiByFourRadVal) => RelativeInterPos.BR,
                (<= -Constants.PiByFourRadVal, <= Constants.PiByFourRadVal) => RelativeInterPos.BL,
                _ => RelativeInterPos.TR
            };
        }
        
        #endregion
        
        #region move methodes

        private void OnMove()
        {
            if (_inputDir.magnitude <= Constants.MinMoveInputValue)
                return;

            _currentDir = _relativePos switch
            {
                #region TR
                
                RelativeInterPos.TR when _inputDir is { x: > 0, z: > 0 } =>
                    _interRef.IsHittingTopRight || PlayerHittingTR
                        ? Vector3.zero
                        : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                RelativeInterPos.TR when _inputDir is { x: < 0, z: < 0 } => _interRef.IsHittingBottomLeft
                    ? Vector3.zero
                    : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                #endregion
                
                #region BR
                
                RelativeInterPos.BR when _inputDir is { x: > 0, z: < 0 } =>
                    _interRef.IsHittingBottomRight || PlayerHittingBR
                        ? Vector3.zero
                        : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                RelativeInterPos.BR when _inputDir is { x: < 0, z: > 0 } => _interRef.IsHittingTopLeft
                    ? Vector3.zero
                    : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                #endregion
                
                #region TL
                    
                RelativeInterPos.TL when _inputDir is { x: > 0, z: < 0 } => _interRef.IsHittingBottomRight
                    ? Vector3.zero
                    : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                RelativeInterPos.TL when _inputDir is { x: < 0, z: > 0 } =>
                    _interRef.IsHittingTopLeft || PlayerHittingTL
                        ? Vector3.zero
                        : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                #endregion
                
                #region BL
                
                RelativeInterPos.BL when _inputDir is { x: > 0, z: > 0 } => _interRef.IsHittingTopRight
                    ? Vector3.zero
                    : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                RelativeInterPos.BL when _inputDir is { x: < 0, z: < 0 } =>
                    _interRef.IsHittingBottomLeft || PlayerHittingBL
                        ? Vector3.zero
                        : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                #endregion
                
                _ => Vector3.zero
            };

            /*_currentDir = _directionMode switch
            {
                LockDirectionMode.BLToTR when _inputDir is { x: > 0, z: > 0 } => _interRef.IsHittingTopRight ? 
                    Vector3.zero : 
                    (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                LockDirectionMode.BLToTR when _inputDir is { x: < 0, z: < 0 } => _interRef.IsHittingBottomLeft ? 
                    Vector3.zero : 
                    (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                LockDirectionMode.TLToBR when _inputDir is { x: > 0, z: < 0 } => _interRef.IsHittingBottomRight ? 
                    Vector3.zero : 
                    (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                LockDirectionMode.TLToBR when _inputDir is { x: < 0, z: > 0 } => _interRef.IsHittingTopLeft ? 
                    Vector3.zero : 
                    (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                
                _ => Vector3.zero

            };*/
            
            _cc.SimpleMove(_currentDir * (_datasSo.moveDatasSo.holdingRecepMoveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
            Debug.Log(PlayerHittingTL);
            //obj move
            var tempDir = _currentDir * (Time.deltaTime * Constants.RecepMoveSpeedMultiplier);
            _recepRefRb.position += tempDir;
            foreach (var r in _interRef.TopReceps)
            {
                r.TempDir = tempDir;
                r.MoveSolid(r.TempDir);
            }
        }
        
        #endregion

        #region fall switchers
        
        private void OnFall()
        {
            if (!IsGrounded)
                _fallCounter += Time.deltaTime;
            
            if (_fallCounter >= Constants.MaxFallCounterWhileGrabThreshold)
                _stateMachine.OnSwitchState("fall");
        }

        #endregion

        #endregion

        #region fields

        private ReceptorInter _interRef;

        private Rigidbody _recepRefRb;

        private LockDirectionMode _directionMode;

        private RelativeInterPos _relativePos;

        private float _tempDistFromPlayer;

        private float _fallCounter;

        #endregion
    }

    internal enum LockDirectionMode
    {
        TLToBR,
        BLToTR,
        None
    }

    internal enum RelativeInterPos
    {
        TR,
        BR,
        TL,
        BL
    }
}