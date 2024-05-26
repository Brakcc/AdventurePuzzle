using System;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using TMPro;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class Distributor : BaseInterBehavior
    {
        #region properties

        public EnergyTypes IncomingCollectedEnergy
        {
            get => _incomingCollectedEnergy;
            set
            {
                _incomingCollectedEnergy = value;
                InterAction();
            }
        }

        public EnergyTypes TransmittedEnergy
        {
            get => _transmittedEnergy;
            set
            {
                _transmittedEnergy = value;
            }
        }
        
        public short CurrentOrientationLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                levelText.text = _currentLevel.ToString();
                _currentDistributionOrientation = GetOrientationArray(nodeMode, _currentLevel);
                InterAction();
            }
        }

        public sbyte[] CurrentDistribution => _currentDistributionOrientation;

        #endregion

        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            _currentLevel = 0;
            _currentDistributionOrientation = GetOrientationArray(nodeMode, 0);
        }

        public override void InterAction()
        {
            if (CurrentDistribution is null)
                return;
            
            TransmittedEnergy = IncomingCollectedEnergy;
            
            
            if (CurrentDistribution[0] == 0)
            {
                TransmittedEnergy = EnergyTypes.None;
                //ResetNetwork();
                EnergyDistribution();
                return;
            }
            //Debug.Log($"{name}  {TransmittedEnergy}  {CurrentDistribution[0]},{CurrentDistribution[1]},{CurrentDistribution[2]},{CurrentDistribution[3]}");
            /*if (TransmittedEnergy is EnergyTypes.None)
            {
                ResetNetwork();
                return;
            }*/

            EnergyDistribution();
        }

        #region reset
        
        private void ResetNetwork()
        {
            foreach (var n in nodeDatas)
            {
                switch(n.dendrite)
                {
                    case DentriteType.Receptor:
                        n.receptorRef.CurrentEnergyType = EnergyTypes.None;
                        break;
                    case DentriteType.Distributor:
                        n.distributorRef.IncomingCollectedEnergy = EnergyTypes.None;
                        break;
                    case DentriteType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n.dendrite, "mais voila mais c'etait sur en fait");
                }
            }
        }

        #endregion
        
        private void EnergyDistribution()
        {
            foreach (var n in nodeDatas)
            {
                var tempE = CurrentDistribution[n.ConnectionID] == 1 ? TransmittedEnergy : EnergyTypes.None;
                //Debug.Log($"{n.dendrite} {n.receptorRef?.name} {n.distributorRef?.name} {n.ConnectionID}");
                switch(n.dendrite)
                {
                    case DentriteType.Receptor:
                        n.receptorRef.CurrentEnergyType = tempE;
                        break;
                    case DentriteType.Distributor:
                        n.distributorRef.IncomingCollectedEnergy = tempE;
                        break;
                    case DentriteType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n.dendrite, "mais voila mais c'etait sur en fait");
                }
            }
        }
        
        private static sbyte[] GetOrientationArray(CableNodeMode mode, short orientLevel)
        {
            switch (mode)
            {
                //i dont even remember why i called them sb smthg
                case CableNodeMode.L:
                    var sbl = new sbyte[] { 0, 0 ,0 ,0 };
                    sbl[orientLevel] = 1;
                    sbl[(orientLevel + 1) % Constants.OrientationNumber] = 1;
                    return sbl;
                case CableNodeMode.I:
                    var sbi = new sbyte[] { 0, 0 ,0 ,0 };
                    sbi[orientLevel] = 1;
                    sbi[(orientLevel + 2) % Constants.OrientationNumber] = 1;
                    return sbi;
                case CableNodeMode.T:
                    var sbt = new sbyte[] { 0, 0 ,0 ,0 };
                    sbt[orientLevel] = 1;
                    sbt[(orientLevel + 1) % Constants.OrientationNumber] = 1;
                    sbt[(orientLevel + 3) % Constants.OrientationNumber] = 1;
                    return sbt;
                case CableNodeMode.X:
                    return Constants.XShapeNodeArray;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, "guez un peu");
            }
        }

        public void SetRef(EmitterInter emit)
        {
            if (nodeDatas.Length == 0)
                return;
            
            foreach (var n in nodeDatas)
            {
                switch (n.dendrite)
                {
                    case DentriteType.Receptor:
                        n.receptorRef.EmitsRef.Add(emit);
                        break;
                    case DentriteType.Distributor:
                        n.distributorRef.SetRef(emit);
                        break;
                    case DentriteType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(n), n.dendrite, "cheh");
                }
            }
        }
        
        #endregion
        
        #region fields

        [SerializeField] private NodeDatas[] nodeDatas;

        [SerializeField] private TMP_Text levelText;

        [SerializeField] private CableNodeMode nodeMode;

        private EnergyTypes _incomingCollectedEnergy;

        private EnergyTypes _transmittedEnergy;
        
        private sbyte[] _currentDistributionOrientation;
        
        private short _currentLevel;

        #endregion
    }
}