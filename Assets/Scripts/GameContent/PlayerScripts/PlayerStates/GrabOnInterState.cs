using System;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class GrabOnInterState : AbstractPlayerState
    {
        #region properties

        #region TR
        
        private bool PlayerHittingTR => Physics.BoxCast(_cc.bounds.center + new Vector3(
                                                         _cc.bounds.extents.x + 0.05f, 
                                                         0, 
                                                         0),
                                                        new Vector3(
                                                                    0.075f / 2, 
                                                                    _cc.bounds.extents.y - 0.1f, 
                                                                    _cc.bounds.extents.z),
                                                        Vector3.right, 
                                                        Quaternion.identity, 
                                                        0.1f, 
                                                        _datasSo.collisionDatasSo.blockMask,
                                                        QueryTriggerInteraction.Ignore);
        
        #endregion
        
        #region TL
        
        private bool PlayerHittingTL => Physics.BoxCast(_cc.bounds.center + new Vector3(
                                                         0, 
                                                         0, 
                                                         _cc.bounds.extents.z + 0.05f),
                                                        new Vector3(
                                                                    _cc.bounds.extents.x, 
                                                                    _cc.bounds.extents.y - 0.1f, 
                                                                    0.075f / 2),
                                                        Vector3.forward, 
                                                        Quaternion.identity, 
                                                        0.1f, 
                                                        _datasSo.collisionDatasSo.blockMask,
                                                        QueryTriggerInteraction.Ignore);
        
        #endregion
        
        #region BR
        
        private bool PlayerHittingBR => Physics.BoxCast(_cc.bounds.center + new Vector3(
                                                         0, 
                                                         0, 
                                                         -(_cc.bounds.extents.z + 0.05f)),
                                                        new Vector3(
                                                                    _cc.bounds.extents.x, 
                                                                    _cc.bounds.extents.y - 0.1f, 
                                                                    0.075f / 2),
                                                        Vector3.back, 
                                                        Quaternion.identity, 
                                                        0.1f, 
                                                        _datasSo.collisionDatasSo.blockMask,
                                                        QueryTriggerInteraction.Ignore);

        #endregion
        
        #region BL
        
        private bool PlayerHittingBL => Physics.BoxCast(_cc.bounds.center + new Vector3(
                                                         -(_cc.bounds.extents.x + 0.05f), 
                                                         0, 
                                                         0),
                                                        new Vector3(
                                                                    0.075f / 2, 
                                                                    _cc.bounds.extents.y - 0.1f, 
                                                                    _cc.bounds.extents.z),
                                                        Vector3.left, 
                                                        Quaternion.identity, 
                                                        0.1f, 
                                                        _datasSo.collisionDatasSo.blockMask,
                                                        QueryTriggerInteraction.Ignore);
        
        #endregion
        
        #endregion
        
        #region constructor
        
        public GrabOnInterState(GameObject go, ControllerState state, PlayerStateMachine pM) : base(go, state, pM)
        {
        }
        
        #endregion

        #region methodes

        public override void OnEnterState()
        {
            _interRef = _checker.InterRef as ReceptorInter;
            
            _tempDistFromPlayer = _interRef!.DistFromPlayer;
            _fallCounter = 0;
            _blockFallCounter = 0;
            
            _relativePos = GetRelativePos((IsoForwardDir + IsoRightDir).normalized, (IsoForwardDir - IsoRightDir).normalized);
            _absolutePos = GetRelativePos(Vector3.right, Vector3.forward);
            _absoluteAngle = getAbsoluteAngle();
            
            _directionMode = GetBaseMoveDir(_relativePos);
            
            AnimationManager.SetAnims("isGrabbing", true);
        }

        public override void OnExitState()
        {
            _tempDistFromPlayer = 0;
            
            _interRef = null;
            
            AnimationManager.SetAnims("isGrabbing", false);
        }

        public override sbyte OnUpdate()
        {
            base.OnUpdate();
            
            var input = _datasSo.moveInput.action.ReadValue<Vector2>();
            _inputDir = new Vector3(input.x, 0, input.y).normalized;
            
            OnFall();
            GetHoldInput();

            return 0;
        }

        public override sbyte OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            OnMove();
            DistFromInterCheck();
            BlockFall();
            
            return 0;
        }

        #region holding methodes

        private void GetHoldInput()
        {
            if (_datasSo.interactInput.action.IsPressed())
                return;
            
            stateMachine.SwitchState("move");
        }

        private void DistFromInterCheck()
        {
            if (_interRef.DistFromPlayer >= _tempDistFromPlayer + Constants.GrabGapThreshold ||
                !_checker.InRangeInter.Contains(_interRef))
                stateMachine.SwitchState("move");
            
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

        private AbsoluteAngle getAbsoluteAngle()
        {
            if (Vector3.Dot(IsoForwardDir, new Vector3(1, 0, -1)) > 0.9f)
                return AbsoluteAngle.a135;
            
            if (Vector3.Dot(IsoForwardDir, new Vector3(-1, 0, 1)) > 0.9f)
                return AbsoluteAngle.am45;
            
            if (Vector3.Dot(IsoForwardDir, new Vector3(-1, 0, -1)) > 0.9f)
                return AbsoluteAngle.am135;
            
            return AbsoluteAngle.a45;
        }
        
        private RelativeInterPos GetRelativePos(Vector3 right, Vector3 forward)
        {
            var position = _goRef.transform.position;
            
            var tempDotX = Vector2.Dot(new Vector2(right.x, right.z),
                               new Vector2((position - _interRef.Pivot).x,
                                   (position - _interRef.Pivot).z)) /
                           new Vector2((position - _interRef.Pivot).x,
                               (position - _interRef.Pivot).z).magnitude;

            var tempDotY = Vector2.Dot(new Vector2(forward.x, forward.z),
                               new Vector2((position - _interRef.Pivot).x,
                                   (position - _interRef.Pivot).z)) / 
                           new Vector2((position - _interRef.Pivot).x,
                               (position - _interRef.Pivot).z).magnitude;
            
            return (tempDotX, tempDotY) switch
            {
                (>= Constants.PiByFourRadVal, <= Constants.PiByFourRadVal) => RelativeInterPos.TR,
                (<= Constants.PiByFourRadVal, >= Constants.PiByFourRadVal) => RelativeInterPos.TL,
                (<= Constants.PiByFourRadVal, <= -Constants.PiByFourRadVal) => RelativeInterPos.BR,
                (<= -Constants.PiByFourRadVal, <= Constants.PiByFourRadVal) => RelativeInterPos.BL,
                _ => RelativeInterPos.TR
            };
        }

        #region blocks Boundaries

        private bool GetBlockTR(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => _interRef.IsHittingTopRight,
            AbsoluteAngle.am45 => _interRef.IsHittingTopLeft,
            AbsoluteAngle.a135 => _interRef.IsHittingBottomRight,
            AbsoluteAngle.am135 => _interRef.IsHittingBottomLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };

        private bool GetBlockTL(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => _interRef.IsHittingTopLeft,
            AbsoluteAngle.am45 => _interRef.IsHittingBottomLeft,
            AbsoluteAngle.a135 => _interRef.IsHittingTopRight,
            AbsoluteAngle.am135 => _interRef.IsHittingBottomRight,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };

        private bool GetBlockBR(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => _interRef.IsHittingBottomRight,
            AbsoluteAngle.am45 => _interRef.IsHittingTopRight,
            AbsoluteAngle.a135 => _interRef.IsHittingBottomLeft,
            AbsoluteAngle.am135 => _interRef.IsHittingTopLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };

        private bool GetBlockBL(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => _interRef.IsHittingBottomLeft,
            AbsoluteAngle.am45 => _interRef.IsHittingBottomRight,
            AbsoluteAngle.a135 => _interRef.IsHittingTopLeft,
            AbsoluteAngle.am135 => _interRef.IsHittingTopRight,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };

        #endregion

        #region player Boundaries

        private bool GetPlayerTR(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => PlayerHittingTR,
            AbsoluteAngle.am45 => PlayerHittingTL,
            AbsoluteAngle.a135 => PlayerHittingBR,
            AbsoluteAngle.am135 => PlayerHittingBL,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };
        
        private bool GetPlayerTL(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => PlayerHittingTL,
            AbsoluteAngle.am45 => PlayerHittingBL,
            AbsoluteAngle.a135 => PlayerHittingTR,
            AbsoluteAngle.am135 => PlayerHittingBR,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };
        
        private bool GetPlayerBR(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => PlayerHittingBR,
            AbsoluteAngle.am45 => PlayerHittingTR,
            AbsoluteAngle.a135 => PlayerHittingBL,
            AbsoluteAngle.am135 => PlayerHittingTL,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };
        
        private bool GetPlayerBL(AbsoluteAngle a) => a switch
        {
            AbsoluteAngle.a45 => PlayerHittingBL,
            AbsoluteAngle.am45 => PlayerHittingBR,
            AbsoluteAngle.a135 => PlayerHittingTL,
            AbsoluteAngle.am135 => PlayerHittingTR,
            _ => throw new ArgumentOutOfRangeException(nameof(a), a, "aha CHEEEEEH")
        };

        #endregion
        
        #endregion
        
        #region move methodes

        private void OnMove()
        {
            if (_inputDir.magnitude <= Constants.MinMoveInputValue)
                return;

            switch (_relativePos)
            {
                #region TR
                
                case RelativeInterPos.TR when _inputDir is { x: > 0, z: > 0 }:
                    _currentDir = GetBlockTR(_absoluteAngle) || GetPlayerTR(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir + IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", true);
                    AnimationManager.SetAnims("isPushing", false);
                    break;
                case RelativeInterPos.TR when _inputDir is { x: < 0, z: < 0 }:
                    _currentDir = GetBlockBL(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir + IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", false);
                    AnimationManager.SetAnims("isPushing", true);
                    break;
                
                #endregion
                
                #region BR
                
                case RelativeInterPos.BR when _inputDir is { x: > 0, z: < 0 }:
                    _currentDir = GetBlockBR(_absoluteAngle) || GetPlayerBR(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir - IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", true);
                    AnimationManager.SetAnims("isPushing", false);
                    break;
                case RelativeInterPos.BR when _inputDir is { x: < 0, z: > 0 }:
                    _currentDir = GetBlockTL(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir - IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", false);
                    AnimationManager.SetAnims("isPushing", true);
                    break;
                
                #endregion
                
                #region TL
                
                case RelativeInterPos.TL when _inputDir is { x: > 0, z: < 0 }:
                    _currentDir = GetBlockBR(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir - IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", false);
                    AnimationManager.SetAnims("isPushing", true);
                    break;
                case RelativeInterPos.TL when _inputDir is { x: < 0, z: > 0 }:
                    _currentDir = GetBlockTL(_absoluteAngle) || GetPlayerTL(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir - IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", true);
                    AnimationManager.SetAnims("isPushing", false);
                    break;
                
                #endregion
                
                #region BL
                
                case RelativeInterPos.BL when _inputDir is { x: > 0, z: > 0 }:
                    _currentDir = GetBlockTR(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir + IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", false);
                    AnimationManager.SetAnims("isPushing", true);
                    break;
                case RelativeInterPos.BL when _inputDir is { x: < 0, z: < 0 }:
                    _currentDir = GetBlockBL(_absoluteAngle) || GetPlayerBL(_absoluteAngle)
                        ? Vector3.zero
                        : ((IsoForwardDir + IsoRightDir) * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x)))
                        .normalized;
                    AnimationManager.SetAnims("isPulling", true);
                    AnimationManager.SetAnims("isPushing", false);
                    break;
                
                #endregion
                
                default:
                    _currentDir = Vector3.zero;
                    AnimationManager.SetAnims("isPulling", false);
                    AnimationManager.SetAnims("isPushing", false);
                    break;
            }

            _cc.SimpleMove(_currentDir * (_datasSo.moveDatasSo.holdingRecepMoveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
            
            //obj move
            var tempDir = _currentDir * (Time.deltaTime * Constants.RecepMoveSpeedMultiplier);
            _interRef.transform.position += tempDir;
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
                stateMachine.SwitchState("fall");
        }

        private void BlockFall()
        {
            if(_interRef is null)
                return;
            
            if (!_interRef.IsHittingGround)
                _blockFallCounter += Time.deltaTime;
            
            if (_blockFallCounter > Constants.MaxBlockFallCounterThreshold)
                stateMachine.SwitchState("move");
        }

        #endregion

        #endregion

        #region fields

        private ReceptorInter _interRef; //Likely change

        private LockDirectionMode _directionMode;

        private RelativeInterPos _relativePos;

        private RelativeInterPos _absolutePos;

        private AbsoluteAngle _absoluteAngle;
        
        private Vector3 _fInput;

        private float _tempDistFromPlayer;

        private float _fallCounter;

        private float _blockFallCounter;

        #endregion
    }

    internal enum LockDirectionMode
    {
        TLToBR,
        BLToTR,
        None
    }

    internal enum AbsoluteAngle
    {
        a45,
        a135,
        am45,
        am135
    }
    
    internal enum RelativeInterPos
    {
        TR,
        BR,
        TL,
        BL
    }
}