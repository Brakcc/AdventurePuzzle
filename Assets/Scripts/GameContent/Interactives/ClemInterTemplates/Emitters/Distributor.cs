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
        
        public sbyte CurrentOrientationLevel
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

        public Transform Pivot => pivot;

        public float BaseYRota => transform.rotation.eulerAngles.y;

        public sbyte StartingLevel => (sbyte)(startingLevel - 1);

        #endregion

        #region methodes

        protected override void OnInit()
        {
            base.OnInit();
            CurrentOrientationLevel = StartingLevel;
        }

        public override void InterAction()
        {
            if (CurrentDistribution is null)
                return;
            
            TransmittedEnergy = IncomingCollectedEnergy;
            
            
            if (CurrentDistribution[0] == 0)
            {
                TransmittedEnergy = EnergyTypes.None;
                EnergyDistribution();
                return;
            }
            //Debug.Log($"{name}  {TransmittedEnergy}  {CurrentDistribution[0]},{CurrentDistribution[1]},{CurrentDistribution[2]},{CurrentDistribution[3]}");

            EnergyDistribution();
        }
        
        private void EnergyDistribution()
        {
            foreach (var n in nodeDatas)
            {
                var tempE = CurrentDistribution[n.ConnectionID] == 1 ? TransmittedEnergy : EnergyTypes.None;
                switch(n.dendrite)
                {
                    case DentriteType.Receptor when tempE is EnergyTypes.None && !n.receptorRef.HasWaveEnergy:
                        n.receptorRef.HasCableEnergy = false;
                        n.receptorRef.CurrentEnergyType = tempE;
                        break;
                    case DentriteType.Receptor when tempE is EnergyTypes.None && n.receptorRef.HasWaveEnergy:
                        break;
                    case DentriteType.Receptor when tempE is not EnergyTypes.None:
                        n.receptorRef.HasCableEnergy = true;
                        n.receptorRef.HasWaveEnergy = false;
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
        
        private static sbyte[] GetOrientationArray(CableNodeMode mode, sbyte orientLevel)
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

        [Range(1, 4)] [SerializeField] private sbyte startingLevel;

        [SerializeField] private NodeDatas[] nodeDatas;

        [SerializeField] private TMP_Text levelText;

        [SerializeField] private CableNodeMode nodeMode;
        
        [SerializeField] private Transform pivot;

        private EnergyTypes _incomingCollectedEnergy;

        private EnergyTypes _transmittedEnergy;
        
        private sbyte[] _currentDistributionOrientation;
        
        private sbyte _currentLevel;

        #endregion
    }
}