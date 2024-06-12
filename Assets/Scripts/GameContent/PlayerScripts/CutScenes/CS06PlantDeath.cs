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
        }

        public override IEnumerator HandleCutScene()
        {
            var i = 0;

            while (i < 1000)
            {
                sphure.localScale += new Vector3(1, 0, 1);
                i++;
                yield return new WaitForEndOfFrame();
            }
        }

        public override void OnEndCutScene()
        {
            
        }
        
        #endregion

        #region fields

        [SerializeField] private Transform sphure;

        [SerializeField] private GameObject PorteCollisioneuseDeDroite;
        
        [SerializeField] private GameObject PorteCollisioneuseDeGauche;

        #endregion
    }
}