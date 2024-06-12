using System.Collections;
using GameContent.PlayerScripts;
using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates
{
    public sealed class EnergySourceInter : BaseInterBehavior
    {
        #region properties

        public EnergyTypes EnergyType => baseType;

        public bool IsActivated => isActivated;

        #endregion
        
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();

            _lerpCoef = 0;

            _matBlock = new MaterialPropertyBlock();
            rend.GetPropertyBlock(_matBlock);
            _matBlock.SetFloat(EnergyFade, _lerpCoef);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            switch (isActivated)
            {
                case true when _lerpCoef < 1:
                    _lerpCoef += Time.deltaTime;
                    break;
                case false when _lerpCoef > 0:
                    _lerpCoef -= Time.deltaTime;
                    break;
                default:
                    return;
            }
            
            _matBlock.SetFloat(EnergyFade, _lerpCoef);
            rend.SetPropertyBlock(_matBlock);
        }

        public override void PlayerAction()
        {
            if (!isActivated)
                return;
            
            isActivated = false;
            OnActionAnim();

            if (PlayerEnergyM.EnergyType != EnergyTypes.None)
            {
                PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = new SourceDatas(this);
                PlayerEnergyM.OnSourceChangedDebug();
                return;
            }
            PlayerEnergyM.CurrentSource = new SourceDatas(this);
            PlayerEnergyM.OnSourceChangedDebug();
        }

        public override void InterAction()
        {
            if (isActivated)
                return;
            
            //mettre des lien renderer ou vfx pour montrer la libération de l'energie ?
            isActivated = true;

            if (currentDB is not null)
                Destroy(currentDB.gameObject);
            
            if (currentDP is not null)
                Destroy(currentDP.gameObject);
        }

        public void OnForceAbsorb()
        {
            if (!isActivated)
                return;
            
            isActivated = false;
            OnActionAnim();
        }
        
        #region anims et VFX
        
        private void OnActionAnim()
        {
            var position = transform.position;
            
            var db = Instantiate(deathball, position, Quaternion.identity);
            currentDB = db.GetComponent<ParticleSystem>();
            currentDB.Play();
            
            var dp = Instantiate(deathparts, position, Quaternion.identity);
            currentDP = dp.GetComponent<ParticleSystem>();
            currentDP.Play();
        }
        
        #endregion

        #endregion

        #region fields

        [SerializeField] private EnergyTypes baseType;

        [FieldCompletion] [SerializeField] private Animator animator;

        [SerializeField] private Renderer rend;

        [SerializeField] private GameObject deathball;

        [SerializeField] private GameObject deathparts;

        private ParticleSystem currentDB;
        
        private ParticleSystem currentDP;
        
        private MaterialPropertyBlock _matBlock;

        private float _actionBlockerThreshold;

        private bool _canInteract;

        private float _lerpCoef;
        
        private static readonly int EnergyFade = Shader.PropertyToID("_On_Energy_fade");

        #endregion
    }
}