using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    public class CS06PlantDeath : CutScene
    {
        #region constructor
        
        public CS06PlantDeath(PlayerStateMachine playerMachine) : base(playerMachine)
        {
        }
        
        #endregion
        
        #region methodes

        public override void OnStartCutScene()
        {
            StartCoroutine(HandleCutScene());

            var eulerDroite = PorteCollisioneuseDeDroite.transform.eulerAngles;
            var eulerGauche = PorteCollisioneuseDeGauche.transform.eulerAngles;
            
            PorteCollisioneuseDeDroite.transform.DORotate(new Vector3(eulerDroite.x,
                eulerDroite.y + 90,
                eulerDroite.z),
                1);
            
            PorteCollisioneuseDeGauche.transform.DORotate(new Vector3(eulerGauche.x,
                    eulerGauche.y - 90,
                    eulerGauche.z),
                1);
            
            sphure.Play();
        }

        public override IEnumerator HandleCutScene()
        {
            yield return new WaitForSeconds(4.5f);
            
            sphure.Pause();
        }

        public override void OnEndCutScene()
        {
            
        }
        
        #endregion

        #region fields

        [SerializeField] private ParticleSystem sphure;

        [SerializeField] private GameObject PorteCollisioneuseDeDroite;
        
        [SerializeField] private GameObject PorteCollisioneuseDeGauche;

        #endregion
    }
}