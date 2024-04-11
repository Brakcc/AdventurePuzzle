using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public class EmitterInter : BaseInterBehavior
    {
        #region methodes

        public override void PlayerAction()
        {
            Debug.Log($"player action {this}");
        }

        public override void PlayerCancel()
        {
            Debug.Log($"player cancel {this}");
        }

        public override void InterAction()
        {
            Debug.Log($"inter action {this}");
            //Cahcnger les valeurs des receps
        }

        #endregion

        #region fields

        [SerializeField] private ReceptorInter[] receptors;
        
        

        #endregion
    }
}