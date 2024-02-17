using GameContent.PlayerScripts.PlayerDatas;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts
{
    public class PlayerStateMachine : MonoBehaviour
    {
        #region methodes

        private void Awake()
        {
            if (playerStates.Length == 0)
                return;

            var tempRb = GetComponent<Rigidbody>();
            foreach (var state in playerStates)
            {
                state.SetRigidBody(tempRb);
                state.SetDatas(datas);
            }
            
            _currentState = playerStates[0];
        }

        private void Start()
        {
            if (playerStates.Length == 0)
                return;
            
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            _currentState.OnUpdate();
        }

        private void FixedUpdate()
        {
            _currentState.OnFixedUpdate();
        }

        public void OnSwitchState(BasePlayerState newState)
        {
            _currentState.OnExitState(this);
            _currentState = newState;
            _currentState.OnEnterState(this);
        }
        
        #endregion
        
        #region fields

        public BasePlayerState[] playerStates;
        
        [FieldCompletion] [SerializeField] protected BasePlayerDatas datas;

        private BasePlayerState _currentState;

        #endregion
    }
}