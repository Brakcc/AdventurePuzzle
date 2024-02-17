using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;

namespace GameContent.PlayerScripts.PlayerDatas
{
    [CreateAssetMenu(fileName = "PlayerDatas", menuName = "PlayerDatas/Controller")]
    public class BasePlayerDatas : ScriptableObject, IPlayerDatas
    {
        [Header("Inputs")]
        [FieldCompletion] public InputActionReference moveInput;
        
        [FieldCompletion] public InputActionReference jumpInput;

        public MoveDatas moveDatas;

        public JumpDatas jumpDatas;

        public GroundingDatas groundingDatas;
        
        [System.Serializable]
        public class MoveDatas
        {
            public float moveSpeed;
        }
        
        [System.Serializable]
        public class JumpDatas
        {
            public float jumpForce;

            public float coyoteTime;

            public float jumpBuffer;

            [FieldColorLerp(0, 1)] [Range(0, 1)] public float airControlCoef;
        }

        [System.Serializable]
        public class GroundingDatas
        {
            public LayerMask groundLayer;
            
            public float dragSpeed;
        }
    }
}