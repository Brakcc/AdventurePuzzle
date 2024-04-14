using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    public abstract class EmitterInter : BaseInterBehavior
    {
        #region properties

        public List<SourceDatas> DatasList
        {
            get => _datas;
            set
            {
                _datas = value;
                InterAction();
            }
        }

        public SourceDatas this[int id]
        {
            get
            {
                if (id < 0 || id >= _datas.Count)
                    throw new ArgumentOutOfRangeException();

                return _datas[id];
            }
        }

        #endregion
        
        #region methodes
        
        protected override void OnInit()
        {
            _datas = new List<SourceDatas>();
            isActivated = true;
        }

        public override void PlayerAction()
        {
            Debug.Log($"player action {this}");
            //add une couleur et lancer une interaction direct dans les cas des cables
        }

        public override void PlayerCancel()
        {
            Debug.Log($"player cancel {this}");
            //retire une couleur et lance une interaction direct dans les cas des cables 
        }

        public override void InterAction()
        {
            Debug.Log($"inter action {this}");
            //Cahcnger les valeurs des receps
        }

        #endregion

        #region fields

        protected List<SourceDatas> _datas;

        #endregion
    }
}