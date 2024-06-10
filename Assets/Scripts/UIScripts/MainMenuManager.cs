using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UIScripts.Sounds;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace UIScripts
{
    public class MainMenuManager : FileWriter
    {
        [HideInInspector] public bool optionsHere;
        [SerializeField] private PlaySound startSceneButtonSound;
        [SerializeField] private PlaySound quitButtonSound;
        [SerializeField] private PlaySound normalSound;

        void Awake()
        {
            StopAllSounds();
        }
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
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    startSceneButtonSound.PlayMySound();
                    StartCoroutine(WaitAVerySmallTimeBeforeKillingTheSounds(sceneName));
                }
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

        IEnumerator WaitAVerySmallTimeBeforeKillingTheSounds(string sceneName)
        {
            yield return new WaitForSeconds(1.1f);
            StopAllSounds();
            SceneManager.LoadScene(sceneName);
        }
        void StopAllSounds()
        {
            RuntimeManager.GetBus("Bus:/").stopAllEvents(STOP_MODE.IMMEDIATE);
        }
        
    }
}
