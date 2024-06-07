using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class MainMenuManager : FileWriter
    {
        [HideInInspector] public bool optionsHere;

        private void Start()
        {
            Debug.Log(Application.persistentDataPath);
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
            if (!optionsHere)
            {
                Application.Quit();
            }
        }
        
    }
}
