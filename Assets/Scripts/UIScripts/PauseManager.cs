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
        
        [SerializeField] private TextAsset saveFile;

        private void Start()
        {
            _pauseGroup = transform.GetChild(0).gameObject;
            _pauseGroup.SetActive(false);
            
        }

        void Update()
        {
            if (Input.GetKeyDown(pauseTouch))
            {
                ShowPause();
            }
        }

        public void ShowPause()
        {
            pauseButton.SetActive(pauseEnabled);
            _pauseGroup.SetActive(!pauseEnabled);
            pauseEnabled = !pauseEnabled;
            Time.timeScale = pauseEnabled ? 0 : 1;
        }

        public void SaveAndQuit()
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
