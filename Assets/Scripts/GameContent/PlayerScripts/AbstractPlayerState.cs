using GameContent.PlayerScripts.PlayerDatas;
using UnityEngine;

namespace GameContent.PlayerScripts
{
    public abstract class AbstractPlayerState : MonoBehaviour
    {
        #region base methodes
        
        protected static float ClampSymmetric(float val, float clamper) => Mathf.Clamp(val, -clamper, clamper);

        public void SetRigidBody(Rigidbody rb) => _rb = rb;

        public void SetDatas(BasePlayerDatasSO datasSo) => _datasSo = datasSo;

        #endregion
        
        #region methodes to herit
        
        public virtual void OnUpdate() {}
        
        public virtual void OnFixedUpdate() {}

        public abstract void OnEnterState(PlayerStateMachine stateMachine);

        public abstract void OnExitState(PlayerStateMachine stateMachine);

        #endregion

        #region fields

        protected PlayerStateMachine _stateMachine;
        
        protected Rigidbody _rb;

        protected BasePlayerDatasSO _datasSo;
        
        protected Vector3 _currentDir;
        
        protected Vector3 _inputDir;

        #endregion
    }
}