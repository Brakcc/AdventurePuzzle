using UnityEngine;
using static GameContent.PlayerScripts.PlayerStates.ApplyState;
using static GameContent.PlayerScripts.PlayerStates.AbsorbState;

namespace GameContent.Interactives
{
    public abstract class BaseInterBehavior : MonoBehaviour
    {
        #region methodes
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            
            OnApply += TestActionApply;
            OnAbsorb += TestActionAbsorb;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            OnApply -= TestActionApply;
            OnAbsorb -= TestActionAbsorb;
        }

        private static void TestActionAbsorb() => Debug.Log("absorb");

        private static void TestActionApply() => Debug.Log("apply");

        #endregion
    }
}