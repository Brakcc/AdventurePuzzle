using GameContent.Interactives.ClemInterTemplates;
using GameContent.PlayerScripts;
using GameContent.PlayerScripts.PlayerStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UIScripts
{
    public class TeleAltPush : MonoBehaviour
    {
        private Transform _myPlayerWhoPush;
        private Vector3 _stockedPos;
        private bool _canBeMoved;
        private bool _direction; //True = X, False = Z.
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
                    
                if (_myPlayerWhoPush.position != _stockedPos)
                {
                    MoveTele();
                }
            }
            else
            {
                transform.parent = _myParent.transform;
            }
        }

        private bool GetDirection(Transform player)
        {
            //Returns True if movement X
            //Returns False if Movement Z
            Vector3 dir = (player.position - transform.position);

            var dirX = dir.x;
            if (dirX < 0)
            {
                dirX *= -1;
            }

            var dirZ = dir.z;
            if (dir.z < 0)
            {
                dirZ *= -1;
            }
            
            if (dirX > dirZ)
            {
                //Movement X
                return true;
            }
            //Else, movement Z
            return false;
        }

        void MoveTele()
        {
            var position1 = _myPlayerWhoPush.position;
            
            //bool onTheSameLevel = !((position1.y - _stockedPos.y) > 1 || (position1.y - _stockedPos.y) < -1);
            bool onTheSameLevel = true; //

            //Debug.Log(onTheSameLevel);
            /*if (onTheSameLevel)
            {
                var position = transform.position;
                if (_direction)
                {
                    transform.position = new Vector3(
                        position.x - (position1.x - _stockedPos.x),
                        position.y,
                        position.z);
                }
                else
                {
                    transform.position = new Vector3(
                        position.x,
                        position.y,
                        position.z - (position1.z - _stockedPos.z));
                }
            }*/
        }

        private void OnTriggerEnter(Collider playerCheckBox)
        {
            if (playerCheckBox.GetComponent<InterCheckerState>())
            {
                _myPlayerWhoPush = playerCheckBox.gameObject.transform.parent;
                _direction = GetDirection(_myPlayerWhoPush);
                _stockedPos = _myPlayerWhoPush.position;
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
