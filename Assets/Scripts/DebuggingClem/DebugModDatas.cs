using TMPro;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace DebuggingClem
{
    [System.Serializable]
    public class DebugModDatas
    {
        [FieldCompletion] public TMP_Text debugText;
        [TextArea] public string debugString;

        public bool hasLight;
        [ShowIfBoolTrue("hasLight")] 
        [FieldCompletion(FieldColor.Yellow, FieldColor.Green)] 
        public Light debugLight;
    }
}