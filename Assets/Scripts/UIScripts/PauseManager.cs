using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        
        private void Start()
        {
            Time.timeScale = 1;
            _pauseGroup = transform.GetChild(0).gameObject;
            _pauseGroup.SetActive(false);
            _myOptionsManager = transform.parent.GetChild(1).GetComponent<OptionsManager>();
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
                endPauseSound.PlayMySound();
            }
            else
            {
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
