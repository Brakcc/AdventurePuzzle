using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class SaveFileSceneManager : FileWriter
    {
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("TitleScreen");
        }

        public void ChangeSaveFile(int numberOfTheSaveFile)
        {
            switch (numberOfTheSaveFile)
            {
                case 1:
                    WriteData(3, null, null, 1, default);
                    break;
                case 2 :
                    WriteData(3, null, null, 2, default);
                    break;
                case 3 :
                    WriteData(3, null, null, 3, default);
                    break;
                default:
                    Debug.Log("Error. Badly Referenced Line to Change. See OptionsManager for more info. - Dav");
                    break;
            }
        }
    }
}
