using GameContent.Interactives.ClemInterTemplates.Receptors;
using UnityEngine;

namespace GameContent.Interactives.ClemInterTemplates
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineConnector : MonoBehaviour
    {
        #region methodes

        private void Start()
        {
            _line = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (recepRef.CurrentEnergyType is not EnergyTypes.Blue || !recepRef.HasCableEnergy)
            {
                if (_line.enabled)
                    _line.enabled = false;
                return;
            }

            if (!_line.enabled)
                _line.enabled = true;
            
            _line.SetPositions(new []{startPos.position, targetPos.position});
        }

        #endregion

        #region fields

        [SerializeField] private ReceptorInter recepRef; 
            
        [SerializeField] private Transform startPos;

        [SerializeField] private Transform targetPos;
        
        private LineRenderer _line;

        #endregion
    }
}