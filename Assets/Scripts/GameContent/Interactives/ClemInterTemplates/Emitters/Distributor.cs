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
                levelText.text = (_currentLevel + 1).ToString();
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

            _lerpCoefs = new float[]{0,0,0,0,0};

            #region VFX

            _cableMats = new MaterialPropertyBlock[nodeDatas.Length + 1];
            for (var i = 0; i < _cableMats.Length; i++)
            {
                _cableMats[i] = new MaterialPropertyBlock();
            }

            selfRend.GetPropertyBlock(_cableMats[0]);
            _cableMats[0].SetFloat(EmissionFade, 0);
            _cableMats[0].SetFloat(GreenBlue, 0);
            selfRend.SetPropertyBlock(_cableMats[0]);
            
            for (var n = 1; n < nodeDatas.Length; n++)
            {
                foreach (var r in nodeDatas[n].cableRends)
                {
                    r.GetPropertyBlock(_cableMats[n]);
                    _cableMats[n].SetFloat(EmissionFade, 0);
                    _cableMats[n].SetFloat(GreenBlue, 0);
                    r.SetPropertyBlock(_cableMats[n]);
                }
            }
            
            #endregion
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            SetLerpCoefs();

            //Debug.Log($"{_lerpCoefs[0]}_{_lerpCoefs[1]}_{_lerpCoefs[2]}_{_lerpCoefs[3]}_{_lerpCoefs[4]}_");
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
                    case DentriteType.Distributor when tempE is EnergyTypes.None:
                        n.distributorRef.IncomingCollectedEnergy = tempE;
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
            
            switch (TransmittedEnergy)
            {
                case EnergyTypes.None:
                    break;
                case EnergyTypes.Yellow:
                    break;
                case EnergyTypes.Green:
                    for (var i = 0; i < nodeDatas.Length; i++)
                    {
                        _cableMats[i + 1].SetFloat(GreenBlue, 1);
                        nodeDatas[i].SetProperties(_cableMats[i + 1]);
                    }
                    _cableMats[0].SetFloat(GreenBlue, 1);
                    selfRend.SetPropertyBlock(_cableMats[0]);
                    break;
                case EnergyTypes.Blue:
                    for (var i = 0; i < nodeDatas.Length; i++)
                    {
                        _cableMats[i + 1].SetFloat(GreenBlue, 0);
                        nodeDatas[i].SetProperties(_cableMats[i + 1]);
                    }
                    _cableMats[0].SetFloat(GreenBlue, 0);
                    selfRend.SetPropertyBlock(_cableMats[0]);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        private void SetLerpCoefs()
        {
            if (CurrentDistribution[0] == 0 || TransmittedEnergy is EnergyTypes.None)
            {
                for (var i = 1; i < _lerpCoefs.Length; i++)
                {
                    if (_lerpCoefs[i] > 0)
                        _lerpCoefs[i] -= Time.deltaTime;
                }
                if (_lerpCoefs[0] > 0)
                    _lerpCoefs[0] -= Time.deltaTime;
                goto SetColors;
            }
            if (CurrentDistribution[0] != 0 && TransmittedEnergy is not EnergyTypes.None)
            {
                if (_lerpCoefs[0] < 1)
                    _lerpCoefs[0] += Time.deltaTime;
            }

            for (var i = 1; i < _lerpCoefs.Length; i++)
            {
                switch (CurrentDistribution[i - 1])
                {
                    case 0 when _lerpCoefs[i] > 0:
                        _lerpCoefs[i] -= Time.deltaTime;
                        break;
                    case 1 when _lerpCoefs[i] < 1:
                        _lerpCoefs[i] += Time.deltaTime;
                        break;
                }
            }

            SetColors:
            
            for (var i = 0; i < nodeDatas.Length; i++)
            {
                _cableMats[i + 1].SetFloat(EmissionFade, _lerpCoefs[nodeDatas[i].ConnectionID + 1]);
                nodeDatas[i].SetProperties(_cableMats[i + 1]);
            }
            _cableMats[0].SetFloat(EmissionFade,_lerpCoefs[0]);
            selfRend.SetPropertyBlock(_cableMats[0]);
        }
        
        #endregion
        
        #region fields

        [Range(1, 4)] [SerializeField] private sbyte startingLevel;

        [SerializeField] private NodeDatas[] nodeDatas;

        [SerializeField] private Renderer selfRend;

        [SerializeField] private TMP_Text levelText;

        [SerializeField] private CableNodeMode nodeMode;
        
        [SerializeField] private Transform pivot;

        private MaterialPropertyBlock[] _cableMats;
        
        private EnergyTypes _incomingCollectedEnergy;

        private EnergyTypes _transmittedEnergy;
        
        private sbyte[] _currentDistributionOrientation;
        
        private sbyte _currentLevel;

        private float[] _lerpCoefs;

        private static readonly int EmissionFade = Shader.PropertyToID("_On_Energy_fade");
        
        private static readonly int GreenBlue = Shader.PropertyToID("_On_Green_Off_Blue");

        #endregion
    }
}