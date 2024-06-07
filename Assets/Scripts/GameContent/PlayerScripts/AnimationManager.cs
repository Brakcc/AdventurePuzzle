using UnityEngine;

namespace GameContent.PlayerScripts
{
    public static class AnimationManager
    {
        #region mathodes

        public static void InitAnimationManager(Animator a) => PlayerAnimator = a;

        public static void SetAnims(int layerID, float weight)
        {
            
        }
        
        public static void SetAnims(int layerID, float weight, string paramName)
        {
            
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, bool paramState)
        {
            
        }
        
        public static void SetAnims(int layerID, float weight, string paramName, float paramState)
        {
            
        }
        
        #endregion

        #region fields

        private static Animator PlayerAnimator;

        #endregion
    }
}