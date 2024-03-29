﻿using GameContent.PlayerScripts.PlayerDatas.PlayerDatasSO;
using Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerDatas
{
    [CreateAssetMenu(fileName = "Controller", menuName = "PlayerDatasSO/Controller")]
    public class BasePlayerDatasSO : ScriptableObject, IPlayerDatas
    {
        [Header("Inputs")]
        [FieldCompletion] public InputActionReference moveInput;
        
        [FieldCompletion] public InputActionReference jumpInput;

        [FieldCompletion] public InputActionReference absorbInput;

        [FieldCompletion] public InputActionReference applyInput;
        
        [FieldCompletion(_checkedColor: FieldColor.Green)] public MoveDatasSO moveDatasSo;

        [FieldCompletion(_checkedColor: FieldColor.Green)] public JumpDatasSO jumpDatasSo;

        [FieldCompletion(_checkedColor: FieldColor.Green)] public GroundingDatasSO groundingDatasSo;
        
        [FieldCompletion(_checkedColor: FieldColor.Green)] public InteractDatasSO interactDatasSo;
    }
}