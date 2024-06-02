using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    [RequireComponent(typeof(Collider))]
    public class ReceptorInter : BaseInterBehavior
    {
        #region properties

        #region Inner Infos
        
        public virtual EnergyTypes CurrentEnergyType
        {
            get => _currentAppliedEnergy;
            set
            {
                _currentAppliedEnergy = value;
                InterAction();
            }
        }

        public List<EmitterInter> EmitsRef { get; } = new();

        [Obsolete]
        public float DistFromEmit
        {
            get
            {
                if (EmitsRef is null || EmitsRef.Count == 0)
                    return 0;

                var i = SortList(EmitsRef);
                return Vector3.Distance(EmitsRef[i].transform.position, transform.position);
            }
        }
        
        public Vector3 Pivot => pivot.position;
        
        public Vector3 TempDir
        {
            get => _tempDir;
            set => _tempDir = SetDir(value);
        }

        protected virtual bool HasElectricity
        {
            get => hasElectricity;
            set
            {
                hasElectricity = value;
            }
        }
        
        public bool IsMovable
        {
            get => _isMovable;
            protected set => _isMovable = value;
        }

        public bool HasWaveEnergy
        {
            get => _hasWaveEnergy;
            set
            {
                _hasWaveEnergy = value;
                _waveEnergyCounter = _hasWaveEnergy ? Constants.WaveEnergyDuration : -1;
            }
            
        }

        public bool HasCableEnergy
        {
            get => _hasCableEnergy;
            set
            {
                _hasCableEnergy = value;
            }
        }

        protected Collider Collid => _col;
        
        #endregion
        
        #region IsHittingTR
        
        public bool IsHittingTopRight => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                          _col.bounds.extents.x + Constants.BoxCastBounds.SideBoxPosDeport, 
                                                          0, 
                                                          0),
                                                         new Vector3(
                                                                     Constants.BoxCastBounds.SideBoxHalfExtent / 2, 
                                                                     _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                     _col.bounds.extents.z - Constants.BoxCastBounds.SideBoxLengthCut),
                                                         Vector3.right,
                                                         Quaternion.identity, 
                                                         Constants.BoxCastBounds.SideCastDist, 
                                                         blockMask);
        
        #endregion
        
        #region IsHittingTL
        
        public bool IsHittingTopLeft => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                         0, 
                                                         0, 
                                                         _col.bounds.extents.z + Constants.BoxCastBounds.SideBoxPosDeport),
                                                        new Vector3(
                                                                    _col.bounds.extents.x - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                    _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                    Constants.BoxCastBounds.SideBoxHalfExtent / 2),
                                                        Vector3.forward, 
                                                        Quaternion.identity, 
                                                        Constants.BoxCastBounds.SideCastDist, 
                                                        blockMask);
        
        #endregion
        
        #region IsHittingBR
        
        public bool IsHittingBottomRight => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                             0, 
                                                             0, 
                                                             -(_col.bounds.extents.z + Constants.BoxCastBounds.SideBoxPosDeport)),
                                                            new Vector3(
                                                                        _col.bounds.extents.x - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                        _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                        Constants.BoxCastBounds.SideBoxHalfExtent / 2),
                                                            Vector3.back, 
                                                            Quaternion.identity, 
                                                            Constants.BoxCastBounds.SideCastDist, 
                                                            blockMask);
        
        #endregion
        
        #region IsHittingBL
        
        public bool IsHittingBottomLeft => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                            -(_col.bounds.extents.x + Constants.BoxCastBounds.SideBoxPosDeport), 
                                                            0, 
                                                            0),
                                                           new Vector3(
                                                                       Constants.BoxCastBounds.SideBoxHalfExtent / 2, 
                                                                       _col.bounds.extents.y - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                       _col.bounds.extents.z - Constants.BoxCastBounds.SideBoxLengthCut),
                                                           Vector3.left, 
                                                           Quaternion.identity, 
                                                           Constants.BoxCastBounds.SideCastDist, 
                                                           blockMask);

        #endregion

        #region IsHittingGround
        
        public bool IsHittingGround => Physics.BoxCast(_col.bounds.center + new Vector3(
                                                            0, 
                                                            -(_col.bounds.extents.y - Constants.BoxCastBounds.DownBoxPosDeport), 
                                                            0),
                                                           new Vector3(
                                                                       _col.bounds.extents.x - Constants.BoxCastBounds.SideBoxLengthCut, 
                                                                       Constants.BoxCastBounds.DownBoxHalfExtent / 2, 
                                                                       _col.bounds.extents.z - Constants.BoxCastBounds.SideBoxLengthCut),
                                                           Vector3.down, 
                                                           Quaternion.identity, 
                                                           Constants.BoxCastBounds.DownCastDist, 
                                                           blockMask);

        #endregion
        
        #region HasBlueAbove

        public List<ReceptorInter> TopReceps => _grabber.RecepRefs;

        #endregion
        
        #endregion

        #region methodes

        protected override void OnInit()
        {
            HasElectricity = false;
            _col = GetComponent<Collider>();
            _grabber = GetComponentInChildren<RecepsTopBlockGrabber>();

            _tempDir = Vector3.zero;
            _fallCurveCounter = 0;
            
            OnReset();

            _vFXLerpCoef = 0;
            _vFXGreenOn = 0;
            _matBlock = new MaterialPropertyBlock();
            rend.GetPropertyBlock(_matBlock);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            
            SetBlockEffect();
            
            if (!HasWaveEnergy)
                return;

            if (_waveEnergyCounter <= 0)
            {
                HasWaveEnergy = false;
                CurrentEnergyType = EnergyTypes.None;
                return;
            }

            _waveEnergyCounter -= Time.deltaTime;
        }

        protected override void OnFixedUpdate()
        {
            SolidFall();
        }

        #region Actions

        public override void PlayerAction()
        {
            //Debug.Log($"player action {this}");
            //link this sur le player pour allow les actions
        }

        public override void PlayerCancel()
        {
            //Debug.Log($"player cancel {this}");
            //delink this du player pour lacher this
        }

        public override void InterAction()
        {
            switch (CurrentEnergyType)
            {
                case EnergyTypes.None:
                    //_col.isTrigger = false; //Si pas de Stay en Green
                    HasElectricity = true;
                    _isMovable = true;
                    break;
                case EnergyTypes.Yellow:
                    //_col.isTrigger = false; //Si pas de Stay en Green
                    HasElectricity = true;
                    _isMovable = true;
                    break;
                case EnergyTypes.Green:
                    _col.isTrigger = true;
                    HasElectricity = false;
                    _isMovable = false;
                    if (HasCheckerRef)
                        RemoveSelf();
                    break;
                case EnergyTypes.Blue:
                    //_col.isTrigger = false; //Si pas de Stay en Green
                    HasElectricity = false;
                    _isMovable = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_currentAppliedEnergy), _currentAppliedEnergy,
                        "how did that happened wtf ???");
            }
        }
        
        #endregion

        #region Inner Actions

        private void OnTriggerExit(Collider other)
        {
            if (CurrentEnergyType is EnergyTypes.Green)
                return;
            
            if (other.CompareTag("Player"))
                return;
            
            _canSwitch = true;
            _col.isTrigger = false;
        }

        private Vector3 SetDir(Vector3 dir) => (dir.x, dir.z) switch
        {
            (>= Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingTopRight ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, >= Constants.MinBlockMoveInputThreshold) => IsHittingTopLeft ? Vector3.zero : dir,
            (<= Constants.MinBlockMoveInputThreshold, <= -Constants.MinBlockMoveInputThreshold) => IsHittingBottomRight ? Vector3.zero : dir,
            (<= -Constants.MinBlockMoveInputThreshold, <= Constants.MinBlockMoveInputThreshold) => IsHittingBottomLeft ? Vector3.zero : dir,
            _ => Vector3.zero
        };

        private void SolidFall()
        {
            if (!_isMovable)
                return;
            
            if (IsHittingGround)
            {
                if (_fallCurveCounter > 0)
                    _fallCurveCounter = 0;
                return;
            }

            _fallCurveCounter += Time.fixedDeltaTime;
            transform.position -= new Vector3(0, fallCurve.Evaluate(_fallCurveCounter) * Time.fixedDeltaTime, 0);
        }
        
        public void MoveSolid(Vector3 dir) => transform.position += dir;
        
        public virtual void OnReset()
        {
            CurrentEnergyType = EnergyTypes.None;
        }

        public float GetDistFromEmit(EmitterInter emit) => Vector3.Distance(emit.transform.position, transform.position);

        private int SortList(IReadOnlyList<EmitterInter> receps)
        {
            var baseDist = Vector3.Distance(receps[0].transform.position, transform.position);
            var i = 0;
            for (var j = 0 ; j < receps.Count ; j++)
            {
                var tempDist = Vector3.Distance(receps[j].transform.position, transform.position);
                if (!(tempDist <= baseDist))
                    continue;
                
                baseDist = tempDist;
                i = j;
            }

            return i;
        }

        #region VFX
        
        private void SetBlockEffect()
        {
            if (!_canSwitch)
                return;
            
            if (CurrentEnergyType is EnergyTypes.Blue && _vFXLerpCoef < 1f)
            {
                _vFXLerpCoef += Time.fixedDeltaTime;
                
                if (_vFXLerpCoef > 0 && Mathf.Approximately(_vFXGreenOn, 1))
                {
                    _vFXGreenOn = 0;
                    _matBlock.SetFloat(GreenE, Mathf.RoundToInt(_vFXGreenOn));
                }
                
                _matBlock.SetFloat(FadeE, Mathf.Abs(_vFXLerpCoef));
                rend.SetPropertyBlock(_matBlock);
            }
            
            if (CurrentEnergyType is EnergyTypes.Green && _vFXLerpCoef > -1f)
            {
                _vFXLerpCoef -= Time.fixedDeltaTime;
                
                if (_vFXLerpCoef < 0 && Mathf.Approximately(_vFXGreenOn, 0))
                {
                    _vFXGreenOn = 1;
                    _matBlock.SetFloat(GreenE, Mathf.RoundToInt(_vFXGreenOn));
                }

                if (Mathf.Approximately(_vFXLerpCoef, -1f))
                    _canSwitch = false;
                
                _matBlock.SetFloat(FadeE, Mathf.Abs(_vFXLerpCoef));
                rend.SetPropertyBlock(_matBlock);
            }
            
            if (CurrentEnergyType is EnergyTypes.None && Mathf.Abs(_vFXLerpCoef) > 0f)
            {
                switch (_vFXLerpCoef)
                {
                    case > 0f:
                        _vFXLerpCoef -= Time.fixedDeltaTime;
                        break;
                    case < 0f:
                        _vFXLerpCoef += Time.fixedDeltaTime;
                        break;
                }

                if (Mathf.Abs(_vFXLerpCoef) is < 0.01f and > 0f)
                    _vFXLerpCoef = 0;
                
                _matBlock.SetFloat(FadeE, Mathf.Abs(_vFXLerpCoef));
                rend.SetPropertyBlock(_matBlock);
            }
        }
        
        #endregion
        
        public static RigidbodyConstraints GetRBConstraints(RBCMode mode) => mode switch
        {
            RBCMode.Rota => RigidbodyConstraints.FreezeRotation,
            RBCMode.RotaPlan => (RigidbodyConstraints)Constants.BitFlagRBConstraintRotaPlan,
            RBCMode.Full => RigidbodyConstraints.FreezeAll,
            RBCMode.None => RigidbodyConstraints.None,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "nice try my guy")
        };
        
        #endregion
        
        #region Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            var bounds = _col.bounds;
            
            #region TR
            
            //Top Right
            Gizmos.DrawWireCube(bounds.center + new Vector3(bounds.extents.x + 0.1f, 0, 0),
                                new Vector3(0.15f, bounds.size.y - 0.2f, bounds.size.z - 0.2f));
            
            #endregion
            
            #region BL
            
            //Bottom Left
            Gizmos.DrawWireCube(bounds.center + new Vector3(-(bounds.extents.x + 0.1f), 0, 0),
                                new Vector3(0.15f, bounds.size.y - 0.2f, bounds.size.z - 0.2f));
            
            #endregion
            
            #region TL
            
            //Top Left
            Gizmos.DrawWireCube(bounds.center + new Vector3(0, 0, bounds.extents.z + 0.1f),
                                new Vector3(bounds.size.x - 0.2f, bounds.size.y - 0.2f, 0.15f));
            
            #endregion
            
            #region BR
            
            //Bottom Right
            Gizmos.DrawWireCube(bounds.center + new Vector3(0, 0, -(bounds.extents.z + 0.1f)),
                                new Vector3(bounds.size.x - 0.2f, bounds.size.y - 0.2f, 0.15f));
            #endregion

            #region GC

            Gizmos.DrawWireCube(bounds.center + new Vector3(0, -(bounds.extents.y/* - 0.03f*/), 0), 
                new Vector3(bounds.size.x - 0.2f, 0.15f, bounds.size.z - 0.2f));

            #endregion
        }

        #endregion
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private Transform pivot;

        [SerializeField] private LayerMask blockMask;

        [SerializeField] private AnimationCurve fallCurve;
        
        [FieldCompletion(FieldColor.Orange)]
        [SerializeField] private Collider _col;

        [SerializeField] private Renderer rend;

        private MaterialPropertyBlock _matBlock;

        private RecepsTopBlockGrabber _grabber;

        private EnergyTypes _currentAppliedEnergy;

        private Vector3 _tempDir;

        private bool _isMovable;

        private bool _hasWaveEnergy;

        private bool _hasCableEnergy;

        private float _waveEnergyCounter;
        
        protected bool hasElectricity;

        private float _fallCurveCounter;

        private float _vFXLerpCoef;

        private float _vFXGreenOn;

        private bool _canSwitch;

        private static readonly int GreenE = Shader.PropertyToID("_On_Green_Off_Blue");

        private static readonly int FadeE = Shader.PropertyToID("_On_Energy_fade");

        #endregion
    }
}