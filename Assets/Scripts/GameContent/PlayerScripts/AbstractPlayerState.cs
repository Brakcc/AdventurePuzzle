using GameContent.PlayerScripts.PlayerDatas;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public abstract class AbstractPlayerState
    {
        #region properties
        
        protected bool IsGrounded => _cc.isGrounded;

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

        #endregion
        
        #region methodes to herit

        public virtual void OnInit() {}
        
        public virtual void OnUpdate() {}
        
        public virtual void OnFixedUpdate() {}

        public abstract void OnEnterState(PlayerStateMachine stateMachine);

        public abstract void OnExitState(PlayerStateMachine stateMachine);

        #endregion

        #region fields

        protected PlayerStateMachine _stateMachine;

        protected GameObject _goRef;
        
        protected CharacterController _cc;

        protected BasePlayerDatasSO _datasSo;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        protected readonly Vector3 _isoRightDir = new(1, 0, -1);
        
        protected readonly Vector3 _isoForwardDir = new(1, 0, 1);
        
        // protected bool IsGrounded => Physics.Raycast(_goRef.transform.position, -_goRef.transform.up,
        //     Constants.PlayerHeight / 2 + Constants.GroundCheckSupLength, _datasSo.groundingDatasSo.groundLayer);

        #endregion
    }
}