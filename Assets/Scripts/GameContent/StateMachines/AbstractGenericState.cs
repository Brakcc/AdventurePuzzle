using GameContent.PlayerScripts;
using UnityEngine;

namespace GameContent.StateMachines
{
    public abstract class AbstractGenericState
    {
        #region constructor

        protected AbstractGenericState(GameObject go)
        {
            _goRef = go;
        }

        #endregion

        #region methodes

        public abstract void OnInit(GenericStateMachine machine);

        public abstract void OnEnterState();

        public abstract sbyte OnUpdate();

        public abstract sbyte OnFixedUpdate();

        public abstract void OnExitState();

        #endregion
        
        #region fields

        protected GenericStateMachine newStateMachine;
        
        protected GameObject _goRef;

        #endregion
    }
}