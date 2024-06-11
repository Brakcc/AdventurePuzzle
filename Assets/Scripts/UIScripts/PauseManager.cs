using FMOD.Studio;
using FMODUnity;
using UIScripts.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace UIScripts
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseButton;
        public KeyCode pauseTouch;
        private OptionsManager _myOptionsManager;
        private GameObject _pauseGroup;
        public bool pauseEnabled;
        public Transform firstTransformPos;

        
        [SerializeField] PlaySound openPauseSound;
        [SerializeField] PlaySound endPauseSound;
        [SerializeField] PlaySound backToCheckPointSound;
        [SerializeField] PlaySound quitSound;

        [SerializeField] private EventReference echoPause;
        private EventInstance _echoInstance;
        
        
        private void Start()
        {
            Time.timeScale = 1;
            _pauseGroup = transform.GetChild(0).gameObject;
            _pauseGroup.SetActive(false);
            _myOptionsManager = transform.parent.GetChild(1).GetComponent<OptionsManager>();
            
            
            //Set Up Pause Echo Sound
            _echoInstance = RuntimeManager.CreateInstance(echoPause);
            _echoInstance.setParameterByName("GamePaused", 1);
            
            _echoInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
        }

        void Update()
        {
            if (_myOptionsManager.myInputAction["Pause"].triggered)
            {
                ShowPause();
            }
        }

        public void ShowPause()
        {
            if (!pauseEnabled)
            {
                _echoInstance.stop(STOP_MODE.IMMEDIATE);
                endPauseSound.PlayMySound();
            }
            else
            {
                _echoInstance.start();
                openPauseSound.PlayMySound();
            }
            pauseButton.SetActive(pauseEnabled);
            _pauseGroup.SetActive(!pauseEnabled);
            pauseEnabled = !pauseEnabled;
            Time.timeScale = pauseEnabled ? 0 : 1;
        }

        public void SaveAndQuit()
        {
            quitSound.PlayMySound();
            
            SceneManager.LoadScene("TitleScreen");
        }

        public void GoBackToPreviousCheckPoint()
        {
            backToCheckPointSound.PlayMySound();
            if (pauseEnabled){ShowPause();}

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
