using UnityEngine;

namespace GameContent.PlayerScripts
{
    public abstract class BasePlayerState : MonoBehaviour
    {
        #region base methodes

        private void Start() => OnStart();
        
        private void Update() => OnUpdate();

        private void FixedUpdate() => OnFixedUpdate();

        private void LateUpdate() => OnLateUpdate();

        #endregion
        
        #region methodes to herit
        
        protected virtual void OnStart() {}
        
        protected virtual void OnUpdate() {}
        
        protected virtual void OnFixedUpdate() {}
        
        protected virtual void OnLateUpdate() {}
        
        #endregion
    }
}