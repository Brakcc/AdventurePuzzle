using GameContent.Interactives;
using UnityEngine;

namespace GameContent.PlayerScripts.CutScenes
{
    [RequireComponent(typeof(Collider))]
    public class PlanteDeLaMuerta : BaseInterBehavior
    {
        #region methodes

        public override void PlayerAction()
        {
            deathCutScene.OnStartCutScene();
        }

        #endregion

        #region fields

        [SerializeField] private CutScene deathCutScene;

        #endregion
    }
}