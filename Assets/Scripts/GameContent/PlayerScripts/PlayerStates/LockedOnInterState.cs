using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.PlayerScripts.PlayerStates
{
    public sealed class LockedOnInterState : AbstractPlayerState
    {
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
            
            _directionMode = GetBaseMoveDir();
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
        
        private LockDirectionMode GetBaseMoveDir()
        {
            var tempAngle = Vector3.Angle(Vector3.right, _goRef.transform.position - _interRef.Pivot);
            
            return tempAngle switch
            {
                <= 45 or >= 135 => LockDirectionMode.BLToTR,
                < 135 or > 45 => LockDirectionMode.TLToBR,
                _ => LockDirectionMode.None
            };
        }

        #endregion
        
        #region move methodes

        private void OnMove()
        {
            if (_inputDir.magnitude <= Constants.MinMoveInputValue)
                return;
            
            _currentDir = _directionMode switch
            {
                LockDirectionMode.BLToTR when _inputDir is { x: > 0, z: > 0 } => _interRef.IsHittingTopRight ? Vector3.zero : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                LockDirectionMode.BLToTR when _inputDir is { x: < 0, z: < 0 } => _interRef.IsHittingBottomLeft ? Vector3.zero : (Vector3.right * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                LockDirectionMode.TLToBR when _inputDir is { x: > 0, z: < 0 } => _interRef.IsHittingBottomRight ? Vector3.zero : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                LockDirectionMode.TLToBR when _inputDir is { x: < 0, z: > 0 } => _interRef.IsHittingTopLeft ? Vector3.zero : (Vector3.forward * (_inputDir.x * _inputDir.z * Mathf.Sign(_inputDir.x))).normalized,
                _ => Vector3.zero
            };
            
            //Debug.Log($"{_interRef.name} = {_interRef.IsHittingTopRight}  {_interRef.IsHittingTopLeft}  {_interRef.IsHittingBottomRight}  {_interRef.IsHittingBottomLeft}");
            
            _cc.SimpleMove(_currentDir * (_datasSo.moveDatasSo.holdingRecepMoveSpeed * Constants.SpeedMultiplier * Time.deltaTime));
            
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
}