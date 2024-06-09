using UnityEngine;

namespace GameContent.PlayerScripts
{
    public static class AnimationManager
    {
        #region properties

        public static Animator PlayerAnimator => playerAnimator;

        #endregion
        
        #region mathodes

        public static void InitAnimationManager(Animator a) => playerAnimator = a;

        public static void SetLayerWeight(int layerID, float weight)
        {
            playerAnimator.SetLayerWeight(layerID, weight);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName)
        {
            playerAnimator.SetLayerWeight(layerID, weight);
            playerAnimator.SetTrigger(paramName);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, bool paramState)
        {
            playerAnimator.SetLayerWeight(layerID, weight);
            playerAnimator.SetBool(paramName, paramState);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, float paramState)
        {
            playerAnimator.SetLayerWeight(layerID, weight); 
            playerAnimator.SetFloat(paramName, paramState);
        }
        
        public static void SetAnims(string paramName)
        {
            playerAnimator.SetTrigger(paramName);
        }
        
        public static void SetAnims(string paramName, bool paramState)
        {
            playerAnimator.SetBool(paramName, paramState);
        }
        
        public static void SetAnims(string paramName, float paramState)
        {
            playerAnimator.SetFloat(paramName, paramState);
        }
        
        #endregion

        #region fields

        private static Animator playerAnimator;

        #endregion
    }
}