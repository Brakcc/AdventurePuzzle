using System;
using GameContent.Interactives.ClemInterTemplates.Receptors;
using TMPro;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates.Emitters
{
    public sealed class Distributor : BaseInterBehavior
    {
        #region properties

        public EnergyTypes IncomingCollectedEnergy { get; set; }
        
        public EnergyTypes TransmittedEnergy { get; set; }
        
        public short CurrentOrientationLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                levelText.text = _currentLevel.ToString();
                _currentDistributionOrientation = GetOrientationArray(nodeMode, _currentLevel);
            }
        }

        public sbyte[] CurrentDistribution => _currentDistributionOrientation;

        #endregion

        #region methodes

        public override void InterAction()
        {
            
        }

        private void EnergyDistribution()
        {
            
        }
        
        private static sbyte[] GetOrientationArray(CableNodeMode mode, short orientLevel)
        {
            switch (mode)
            {
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

        #endregion
        
        #region fields

        [SerializeField] private NodeDatas[] nodeDatas;

        [SerializeField] private TMP_Text levelText;

        [SerializeField] private CableNodeMode nodeMode;

        private sbyte[] _currentDistributionOrientation;
        
        private short _currentLevel;

        #endregion
    }
}