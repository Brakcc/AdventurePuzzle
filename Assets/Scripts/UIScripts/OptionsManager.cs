using System.Globalization;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UIScripts
{
    public class OptionsManager : MonoBehaviour
    {
        private GameObject _optionsGroup;
        public TextAsset optionsFile;
        public TextAsset commandsFile;

        [SerializeField] private Slider sliderVolumePrincipal;
        [SerializeField] private Slider sliderVolumeMusique;
        [SerializeField] private Slider sliderVolumeSoundEffect;

        [Header("Référencer Pause Group si on est dans un niveau." + "\n" + "Main Menu Manager seulement si on est dans le Menu Principal.")]
        [SerializeField] private GameObject pauseGroup;
        [SerializeField] private MainMenuManager mainMenuManager;
        
        private void Start()
        {
            _optionsGroup = transform.GetChild(0).gameObject;
            _optionsGroup.SetActive(false);
        }

        public void ShowOptions()
        {
            _optionsGroup.SetActive(!_optionsGroup.activeSelf);
            if (pauseGroup is null)
            {
                mainMenuManager.optionsHere = _optionsGroup.activeSelf;
            }
            else 
            {
                pauseGroup.SetActive(!_optionsGroup.activeSelf);
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
            else if (whatOptionToChange is > 3 and < 10)
            {
                //Change CommandsFile.
                WriteFile(commandsFile, whatOptionToChange);
            }
            else
            {
                Debug.Log("Erreur. Mauvais renseignement dans les options. Regardez les commentaires de la fonction ChangeOptions pour plus d'informations.");
            }
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
