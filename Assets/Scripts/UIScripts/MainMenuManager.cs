using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UIScripts.Sounds;

namespace UIScripts
{
    public class MainMenuManager : FileWriter
    {
        [HideInInspector] public bool optionsHere;
        [SerializeField] private PlaySound startSceneButtonSound;
        [SerializeField] private PlaySound quitButtonSound;
        [SerializeField] private PlaySound normalSound; 
        
        private void Start()
        {
            optionsHere = false;
        }

        public void ChargeScene(string sceneName)
        {
            if (!optionsHere)
            {
                if (sceneName == "SaveFileScene")
                {
                    normalSound.PlayMySound();
                }
                else{startSceneButtonSound.PlayMySound();}
                SceneManager.LoadScene(sceneName);
            }
        }

        public void QuitGame()
        {
            if (!optionsHere)
            {
                Debug.Log("Quit");
                quitButtonSound.PlayMySound();
                Application.Quit();
            }
        }
        
    }
}
