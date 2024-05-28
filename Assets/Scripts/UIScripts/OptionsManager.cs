using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UIScripts
{
    public class OptionsManager : MonoBehaviour
    {
        [SerializeField] private GameObject optionsGroup;
        public TextAsset optionsFile;
        public TextAsset commandsFile;

        [SerializeField] private Slider sliderVolumePrincipal;
        [SerializeField] private Slider sliderVolumeMusique;
        [SerializeField] private Slider sliderVolumeSoundEffect;
        
        private void Start()
        {
            optionsGroup.SetActive(false);
        }

        public void ShowOptions()
        {
            optionsGroup.SetActive(!optionsGroup.activeSelf);
            if (SceneManager.GetActiveScene().name == "TitleScreen")
            {
                GetComponent<MainMenuManager>().optionsHere = optionsGroup.activeSelf;
            }
        }
        public void ChangeOptions(int whatOptionToChange)
        {
            /*
             * Options : General Volume (1), Music Volume (2), Sound Effect Volume (3)
             * Controls : Controles (up : 4, down : 5, left : 6, right : 7, action : 8, pause : 9)
             */
            
            if (whatOptionToChange is < 4 and > 0)
            {
                //Change optionsFile.
                WriteFile(optionsFile, whatOptionToChange);
            }
        }

        void ReadFile(TextAsset fileWhoNeedsToBeRead, int lineNumber, int lenght)
        {
            string path = AssetDatabase.GetAssetPath(fileWhoNeedsToBeRead);
            StreamReader reader = new StreamReader(path);
            string lineString = null;
            
            for (int i = 0; i < lenght; i++)
            {
                if (i == lineNumber)
                {
                    lineString = reader.ReadLine();
                }
                else
                {
                    reader.ReadLine();
                }
            }
            Debug.Log(lineString);
            reader.Close();
        }

        void WriteFile(TextAsset fileWhoNeedsToBeEdited, int line)
        {
            
            string path = AssetDatabase.GetAssetPath(fileWhoNeedsToBeEdited);
            int numberOfLines = GetNumberOfLines(path);
            
            

            string newValue = "0";
            switch (line)
            {
                case 1:
                    newValue = sliderVolumePrincipal.value.ToString(CultureInfo.CurrentCulture);
                    break;
                case 2:
                    newValue = sliderVolumeMusique.value.ToString(CultureInfo.CurrentCulture);
                    break;
                case 3:
                    newValue = sliderVolumeSoundEffect.value.ToString(CultureInfo.CurrentCulture);
                    break;
            }
            
            StreamReader reader = new StreamReader(path);
            string[] arrLines = new string[numberOfLines];
            
            for (int i = 0; i < numberOfLines; i++)
            {
                if (i+1 == line)
                {
                    arrLines[i] = newValue;
                    reader.ReadLine();
                }
                else
                {
                    arrLines[i] = reader.ReadLine();
                }
            }
            reader.Close();
            using StreamWriter writer = new StreamWriter(path);
            
            for (int i = 0; i < numberOfLines; i++)
            {
                Debug.Log(arrLines[i]);
                writer.WriteLine(arrLines[i]);
            }
            writer.Close();
            
            
        }

        int GetNumberOfLines(string myPath)
        {
            //Get Number of Lines in a Reader
            int count = 0;
            StreamReader countReader = new StreamReader(myPath);
            while ((countReader.ReadLine()) != null)
            {
                count++;
            }
            countReader.Close();
            return count;
        }
    }
}
