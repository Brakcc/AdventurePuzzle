using GameContent.PlayerScripts.PlayerDatas;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public abstract class AbstractPlayerState
    {
        #region properties
        
        protected bool IsGrounded => _cc.isGrounded;

        protected float Velocity => (_nextPos - _prevPos).magnitude;

        #endregion
        
        #region constructor

        protected AbstractPlayerState(GameObject go)
        {
            _goRef = go;
            _currentDir = _isoForwardDir;
        }
             
        #endregion
        
        #region base methodes
        
        protected static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        public void SetGameObject(GameObject go) => _goRef = go;
        
        public void SetCharaCont(CharacterController cc) => _cc = cc;

        public void SetDatas(BasePlayerDatasSO datasSo) => _datasSo = datasSo;

        public void SetChecker(InterCheckerState checker) => _checker = checker;

        #endregion
        
        #region methodes to herit

        public virtual void OnInit()
        {
            _prevPos = _goRef.transform.position;
            _nextPos = _goRef.transform.position;
        }

        public virtual void OnUpdate() {}
        
        public virtual void OnFixedUpdate()
        {
            _prevPos = _nextPos;
            _nextPos = _goRef.transform.position;
        }

        public abstract void OnEnterState(PlayerStateMachine stateMachine);

        public abstract void OnExitState(PlayerStateMachine stateMachine);

        #endregion

        #region fields

        protected PlayerStateMachine _stateMachine;

        protected GameObject _goRef;
        
        protected CharacterController _cc;

        protected BasePlayerDatasSO _datasSo;

        protected InterCheckerState _checker;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        protected readonly Vector3 _isoRightDir = new(1, 0, -1);
        
        protected readonly Vector3 _isoForwardDir = new(1, 0, 1);

        private Vector3 _prevPos;

        private Vector3 _nextPos;

        #endregion
    }
}