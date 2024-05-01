using GameContent.PlayerScripts;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class CableEmitter : EmitterInter
    {
        #region methodes

        private void FixedUpdate()
        {
            //_line.SetPosition(1, PlayerEnergyM.instance.transform.position);
        }

        protected override void OnInit()
        {
            base.OnInit();
            if (receptors.Length == 0)
                return;
            
            foreach (var r in receptors)
            {
                r.EmitRef = this;
            }
            
            //_line = GetComponent<LineRenderer>();
            //_line.SetPositions(new []{transform.position, PlayerEnergyM.instance.transform.position});
        }
        
        public override void InterAction()
        {
            for (var i = 0; i < Count; i++)
            {
                receptors[i].CurrentEnergyType = this[i].Type;
            }
        }

        public override void PlayerAction()
        {
            if (SourceDatasList.Count >= receptors.Length)
                return;
            
            if (PlayerEnergyM.EnergyType == EnergyTypes.None)
                return;
            
            SourceDatasList.Add(PlayerEnergyM.CurrentSource);
            PlayerEnergyM.CurrentSource = new SourceDatas();
            PlayerEnergyM.OnSourceChangedDebug();
            
            InterAction();
        }

        public override void PlayerCancel()
        {
            if (Count <= 0)
                return;

            if (PlayerEnergyM.GetEnergyBack)
            {
                if (PlayerEnergyM.EnergyType != EnergyTypes.None)
                    PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = SourceDatasList[Count - 1];
                PlayerEnergyM.OnSourceChangedDebug();
            }
            else 
                SourceDatasList[Count - 1].Source.InterAction();
            
            receptors[Count - 1].OnReset();
            SourceDatasList.RemoveAt(Count - 1);
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [FieldCompletion(FieldColor.Red, FieldColor.Green)] 
        [SerializeField] private ReceptorInter[] receptors;

        //private LineRenderer _line;

        #endregion
    }
}