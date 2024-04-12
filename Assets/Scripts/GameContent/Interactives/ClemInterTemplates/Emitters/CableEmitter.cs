using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class CableEmitter : EmitterInter
    {
        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            if (receptors.Length == 0)
                return;
            
            foreach (var r in receptors)
            {
                r.EmitRef = this;
            }
        }
        
        public override void InterAction()
        {
            for (var i = 0; i < _datas.Count; i++)
            {
                receptors[i].CurrentEnergyType = _datas[i].Type;
            }
            base.InterAction();
        }

        public override void PlayerAction()
        {
            
            base.PlayerAction();
        }

        public override void PlayerCancel()
        {
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [SerializeField] private ReceptorInter[] receptors;

        #endregion
    }
}