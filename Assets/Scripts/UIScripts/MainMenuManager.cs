using System.Diagnostics.Tracing;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class MainMenuManager : FileWriter
    {
        [SerializeField] private string audioKey = default;
        
        [HideInInspector] public bool optionsHere;

        private void Start()
        {
            Debug.Log(FMODUnity.RuntimeManager.HasBankLoaded("bank:/bankName"));
            optionsHere = false;
        }

        public void ChargeScene(string sceneName)
        {
            if (!optionsHere)
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public void QuitGame()
        {
            //var audioevent = RuntimeManager.CreateInstance(audioKey);
            //audioevent.start();
            if (!optionsHere)
            {
                Debug.Log("Quit");
                Application.Quit();
            }
        }
        
    }
}
