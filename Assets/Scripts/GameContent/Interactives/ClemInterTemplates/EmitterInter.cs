using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public abstract class EmitterInter : BaseInterBehavior
    {
        #region properties

        protected List<SourceDatas> SourceDatasList { get; private set; }

        public int SourceCount => SourceDatasList.Count;
        
        protected SourceDatas this[int id]
        {
            get
            {
                if (id < 0 || id >= SourceDatasList.Count)
                {
                    //throw new ArgumentOutOfRangeException(nameof(id), id, "too bad");
                    return new SourceDatas(null);
                }

                return SourceDatasList[id];
            }
        }

        #endregion
        
        #region methodes
        
        protected override async void OnInit()
        {
            SourceDatasList = new List<SourceDatas>();
            debugTextLocal = debugMod.debugString;
            
            await Task.Delay(500);
            ForceAbsorbSources(preSetAbsorbSources);
            
        }

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
            //add une couleur et lancer une interaction direct dans les cas des cables
        }

        public override void PlayerCancel()
        {
            //Debug.Log($"player cancel {this}");
            //retire une couleur et lance une interaction direct dans les cas des cables 
        }

        public override void InterAction()
        {
            //Debug.Log($"inter action {this}");
            //Cahcnger les valeurs des receps
        }

        protected abstract void ForceAbsorbSources(EnergySourceInter[] sources);
        
        #endregion

        #region fields

        [SerializeField] private EnergySourceInter[] preSetAbsorbSources;

        #endregion
    }
}