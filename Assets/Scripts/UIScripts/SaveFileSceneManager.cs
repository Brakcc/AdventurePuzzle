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
            /*
            numberOfTheSaveFile += 10;
            
            switch (numberOfTheSaveFile)
            {
                case 11:
                    ChangeFile(10, ReadFile(11, saveFilesFile));
                    ChangeFile(14,"1");
                    break;
                case 12 :
                    ChangeFile(10, ReadFile(12, saveFilesFile));
                    ChangeFile(14,"2");
                    break;
                case 13 :
                    ChangeFile(10, ReadFile(13, saveFilesFile));
                    ChangeFile(14,"3");
                    break;
                default:
                    Debug.Log("Error. Badly Referenced Line to Change. See OptionsManager for more info. - Dav");
                    break;
            }
            */
        }
    }
}
