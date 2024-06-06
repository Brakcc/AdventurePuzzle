using UnityEngine;
using Utilities.CustomAttributes;

namespace GameContent.Interactives.ClemInterTemplates.Receptors
{
    public class TeleporterRecep : ReceptorInter
    {
        #region properties

        public override EnergyTypes CurrentEnergyType
        {
            get => base.CurrentEnergyType;
            set
            {
                base.CurrentEnergyType = value;
                teleporterRef.SetCurrentEnergyType(CurrentEnergyType);
            }
        }

        protected override bool HasElectricity
        {
            get => base.HasElectricity;
            set
            {
                base.HasElectricity = value;
                teleporterRef.hasElectricity = hasElectricity;
            }
        }

        #endregion
        
        #region methodes

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !HasElectricity || _isReceiving)
                return;

            teleporterRef._isReceiving = true;
            _isSending = true;

            other.transform.position = teleporterRef.transform.position;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player") || _isSending || !teleporterRef._isSending || !_isReceiving)
                return;

            _isReceiving = false;
            teleporterRef._isSending = false;
        }

        private void SetCurrentEnergyType(EnergyTypes type)
        {
            base.CurrentEnergyType = type;
            topPoser.isTrigger = type is EnergyTypes.Green;
        }

        #endregion

        #region fields

        [FieldCompletion]
        [SerializeField]
        private TeleporterRecep teleporterRef;

        [SerializeField] private Collider topPoser;

        private bool _isReceiving;

        private bool _isSending;

        #endregion
    }
}