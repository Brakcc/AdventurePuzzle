using GameContent.Interactives.ClemInterTemplates;
using GameContent.PlayerScripts;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DavidTestScripts
{
    public class TeleAltPush : MonoBehaviour
    {
        private Transform _myPlayerWhoPush;
        private bool _canBeMoved;
        private TeleporterInter _myTelepData;
        [SerializeField] private InputActionAsset pushAction;
        private bool _buttonPressed;
        private Transform _myParent;
        private bool _lockZorX;
        private float _valueLock;
        private bool _takeFirstValues;
        
        void Start()
        {
            _myTelepData = GetComponent<TeleporterInter>();
            _myParent = transform.parent;
            _myPlayerWhoPush = null;
        }

        void FixedUpdate()
        {
            if (_myPlayerWhoPush != null)
            {
                MoveFunction();
            }
        }

        void MoveFunction()
        {
            _canBeMoved = pushAction["Interact"].IsPressed();
            
            if (_canBeMoved && _myTelepData.blue)
            {
                /*if (_lockZorX)
                {
                    var position = _myPlayerWhoPush.position;
                    position = new Vector3(position.x, position.y , _valueLock);
                    _myPlayerWhoPush.position = position;
                }
                else
                {
                    var position = _myPlayerWhoPush.position;
                    position = new Vector3(_valueLock, position.y , position.z);
                    _myPlayerWhoPush.position = position;
                }*/
                
                _myPlayerWhoPush.GetComponent<PlayerStateMachine>().CurrentState = ControllerState.grab;
                _myPlayerWhoPush.GetChild(1).GetComponent<InterCheckerState>().InterRef = GetComponent<TeleporterInter>();

                transform.parent = _myPlayerWhoPush;
            }
            else
            {
                transform.parent = _myParent.transform;
            }
        }

        private void OnTriggerEnter(Collider playerCheckBox)
        {
            if (playerCheckBox.GetComponent<InterCheckerState>())
            {
                _myPlayerWhoPush = playerCheckBox.gameObject.transform.parent;
                Debug.Log(_myPlayerWhoPush.name);
            }
        }

        private void OnTriggerExit(Collider playerCheckBox)
        {
            if (playerCheckBox.GetComponent<InterCheckerState>())
            {
                _myPlayerWhoPush = null;
            }
        }
    }
}
