using System;
using System.Collections.Generic;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class CableEmitter : EmitterInter
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

        private readonly Comparison<int> Compare = (a, b) => (int)Mathf.Sign(b - a);

        #endregion
    }
}