using UnityEngine;

namespace GameContent.PlayerScripts
{
    public static class AnimationManager
    {
        #region mathodes

        public static void InitAnimationManager(Animator a) => PlayerAnimator = a;

        public static void SetLayerWeight(int layerID, float weight)
        {
            PlayerAnimator.SetLayerWeight(layerID, weight);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName)
        {
            PlayerAnimator.SetLayerWeight(layerID, weight);
            PlayerAnimator.SetTrigger(paramName);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, bool paramState)
        {
            PlayerAnimator.SetLayerWeight(layerID, weight);
            PlayerAnimator.SetBool(paramName, paramState);
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, float paramState)
        {
            PlayerAnimator.SetLayerWeight(layerID, weight);
            PlayerAnimator.SetFloat(paramName, paramState);
        }
        
        #endregion

        #region fields

        private static Animator PlayerAnimator;

        #endregion
    }
}