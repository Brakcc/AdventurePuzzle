using System;
using GameContent.PlayerScripts;

namespace GameContent.StateMachines
{
    public sealed class GenericStateMachine
    {
        #region properties

        

        #endregion

        #region constructor

        public GenericStateMachine(int stateNumber)
        {
            _initStates = new Action<GenericStateMachine>[stateNumber];
            _enterStates = new Action[stateNumber];
            _updateStates = new Func<sbyte>[stateNumber];
            _fixedUpdateStates = new Func<sbyte>[stateNumber];
            _exitStates = new Action[stateNumber];
        }

        #endregion

        #region methodes

        public void SetCallBacks(int stateID, Action<GenericStateMachine> init, Action enter, Func<sbyte> update, Func<sbyte> fixedUpdate, Action exit)
        {
            _initStates[stateID] = init;
            _enterStates[stateID] = enter;
            _updateStates[stateID] = update;
            _fixedUpdateStates[stateID] = fixedUpdate;
            _exitStates[stateID] = exit;
        }

        public void UpdateMachine()
        {
            
        }

        public void FixedUpdateMachine()
        {
            
        }
        
        public void SwitchState(int toState)
        {
            
        }

        public void SwitchState(string toState)
        {
            
        }

        public static implicit operator int(GenericStateMachine m) => m.currentState;
        
        #endregion

        #region fields

        public int currentState;
        
        private readonly Action<GenericStateMachine>[] _initStates;

        private readonly Action[] _enterStates;

        private readonly Func<sbyte>[] _updateStates;

        private readonly Func<sbyte>[] _fixedUpdateStates;

        private readonly Action[] _exitStates;

        #endregion
    }
}