using UIScripts.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIScripts
{
    public class SaveFileSceneManager : FileWriter
    {
        public PlaySound normalClick;
        public PlaySound quitClick;

        void Start()
        {
            SoundManager.SoundInstance.SetUpVolumes(mainVolumeValue, musicVolumeValue, soundEffectVolumeValue);
        }
        public void ReturnToMainMenu()
        {
            quitClick.PlayMySound();
            SceneManager.LoadScene("TitleScreen");
        }

        public void ChangeSaveFile(int numberOfTheSaveFile)
        {
            normalClick.PlayMySound();
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
