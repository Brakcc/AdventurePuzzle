using GameContent.Interactives.ClemInterTemplates.Receptors;
using GameContent.PlayerScripts;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class CableEmitter : EmitterInter
    {
        #region methodes

        //protected override void OnFixedUpdate()
        //{
        //    _line.SetPosition(1, PlayerEnergyM.instance.transform.position);
        //}

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
            //ici action du cable
            //
            //
            //
            
            for (var i = 0; i < SourceCount; i++)
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
            if (SourceCount <= 0)
                return;

            if (PlayerEnergyM.GetEnergyBack)
            {
                if (PlayerEnergyM.EnergyType != EnergyTypes.None)
                    PlayerEnergyM.CurrentSource.Source.InterAction();
                PlayerEnergyM.CurrentSource = SourceDatasList[SourceCount - 1];
                PlayerEnergyM.OnSourceChangedDebug();
            }
            else 
                SourceDatasList[SourceCount - 1].Source.InterAction();
            
            receptors[SourceCount - 1].OnReset();
            SourceDatasList.RemoveAt(SourceCount - 1);
            base.PlayerCancel();
        }

        #endregion

        #region fields
        
        [FieldCompletion(FieldColor.Yellow, FieldColor.Green)] 
        [SerializeField] private ReceptorInter[] receptors;

        //private LineRenderer _line;

        #endregion
    }
}