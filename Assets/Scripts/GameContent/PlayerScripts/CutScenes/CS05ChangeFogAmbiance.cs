using System.Collections;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class CS05ChangeFogAmbiance : CutScene
    {
        #region constructor
        
        public CS05ChangeFogAmbiance(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes
        
        public override void OnStartCutScene()
        {
            _lowSkyBlock = new MaterialPropertyBlock();
            _highSkyBlock = new MaterialPropertyBlock();
            
            highRend.GetPropertyBlock(_highSkyBlock);
            lowRend.GetPropertyBlock(_lowSkyBlock);

            StartCoroutine(HandleCutScene());
        }

        public override IEnumerator HandleCutScene()
        {
            while (_lerpCoef < 1)
            {
                _lowSkyBlock.SetColor(Fog, Color.Lerp(lowColor, Color.clear, _lerpCoef));
                _highSkyBlock.SetColor(Fog, Color.Lerp(baseHighColor, newHighColor, _lerpCoef));
                
                lowRend.SetPropertyBlock(_lowSkyBlock);
                highRend.SetPropertyBlock(_highSkyBlock);

                _lerpCoef += Time.fixedDeltaTime;
                
                yield return new WaitForFixedUpdate();
            }
        }

        public override void OnEndCutScene()
        {
            
        }
        
        #endregion

        #region fields

        [SerializeField] private Renderer highRend;

        [SerializeField] private Renderer lowRend;

        [SerializeField] private Color lowColor;

        [SerializeField] private Color baseHighColor;
        
        [SerializeField] private Color newHighColor;
        
        private MaterialPropertyBlock _lowSkyBlock;
        
        private MaterialPropertyBlock _highSkyBlock;

        private float _lerpCoef = 0;

        private static readonly int Fog = Shader.PropertyToID("_Fog");

        #endregion
    }
}