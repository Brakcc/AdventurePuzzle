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
            //Say LoadSave to load the selected save.
            
            if (!optionsHere)
            {
                SceneManager.LoadScene("David3Scene");
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
