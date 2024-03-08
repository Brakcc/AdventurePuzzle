using UnityEngine;
using UnityEngine.InputSystem;
using Utilities.CustomAttributes;
using Utilities.CustomAttributes.FieldColors;

namespace GameContent.PlayerScripts.PlayerStates
{
    public class PlayerRotation : MonoBehaviour
    {
        #region methodes

        public void Start()
        {
            Cursor.visible = false;

            _currentDir = Vector3.forward;
            _lastDir = Vector3.forward;
        }

        public void Update()
        {
            var tempInput = moveInput.action.ReadValue<Vector2>();
            
            if (tempInput.magnitude <= Constants.MinMoveInputValue)
                return;
            
            _lastDir = new Vector3(tempInput.x, 0, tempInput.y).normalized;
        }

        public void FixedUpdate()
        {
            var angle = Vector3.Dot(_lastDir, _currentDir) / (_currentDir.magnitude * _lastDir.magnitude);
            if (Mathf.Acos(angle) > Constants.MinPlayerRotationAngle)
            {
                _currentDir = Vector3.MoveTowards(_currentDir, _lastDir, rotaSpeedCoef * Time.fixedDeltaTime);
            }
            
            transform.rotation = Quaternion.LookRotation(_currentDir);
        }

        /*#region rota methodes

        private void OnUpdateRotationDir(Vector3 tempInput)
        {
            if (tempInput.magnitude <= Constants.MinMoveInputValue)
                return;

            _lastDir = new Vector3(tempInput.x, 0, tempInput.y).normalized;
        }

        private void OnUpdateRotation()
        {
            var angle = Vector3.Dot(_lastDir, _currentDir) / (_currentDir.magnitude * _lastDir.magnitude);
            if (Mathf.Acos(angle) > Constants.MinPlayerRotationAngle)
            {
                _currentDir = Vector3.MoveTowards(_currentDir, _lastDir, rotaSpeedCoef * Time.fixedDeltaTime);
            }

            transform.rotation = Quaternion.LookRotation(_currentDir);
        }

        #endregion*/
        
        #endregion

        #region fields

        [FieldCompletion] [SerializeField] private InputActionReference moveInput;
        
        [FieldColorLerp(FieldColor.Orange, FieldColor.Cyan, 1, 15)] [Range(1, 15)] [SerializeField] private float rotaSpeedCoef;

        private Vector3 _lastDir;

        private Vector3 _currentDir;

        #endregion
    }
}