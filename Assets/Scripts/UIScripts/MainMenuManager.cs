using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [HideInInspector] public bool optionsHere = false;
        

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
