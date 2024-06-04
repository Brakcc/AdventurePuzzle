using System.IO;
using UnityEditor;
using UnityEngine;

namespace UIScripts
{
    public class FileWriter : MonoBehaviour
    {
        public TextAsset optionsFile;
        public TextAsset commandsFile;
        [SerializeField] public TextAsset saveFilesFile;
        
        public void ChangeFile(int whatOptionToChange, string newValue)
        {
            /*
             * Options : General Volume (1), Music Volume (2), Sound Effect Volume (3)
             * Controls : Controles (up : 4, down : 5, left : 6, right : 7, action : 8, pause : 9)
             * SaveFiles : SaveFiles (CurrentScene : 10, Save1 : 11, Save2 : 12, Save3 : 13, CurrentSaveNumber : 14)
             */
            
            if (whatOptionToChange is < 4 and > 0)
            {
                //Change optionsFile.
                WriteFile(optionsFile, whatOptionToChange, newValue);
            }
            else if (whatOptionToChange is > 3 and < 10)
            {
                //Change CommandsFile.
                WriteFile(commandsFile, whatOptionToChange-3, newValue);
            }
            else if (whatOptionToChange is > 9 and < 24)
            {
                //Change SaveFile.
                WriteFile(saveFilesFile, whatOptionToChange-9, newValue);
            }
            else
            {
                Debug.Log("Erreur. Mauvais renseignement dans les options. Regardez les commentaires de la fonction ChangeOptions pour plus d'informations.");
            }
        }
        
        void WriteFile(TextAsset fileWhoNeedsToBeEdited, int line, string valueToChange)
        {
            //var path = Resources.Load<TextAsset>(fileWhoNeedsToBeEdited.ToString());
            var path = fileWhoNeedsToBeEdited.ToString();
            int numberOfLines = GetNumberOfLines(path);
            
            StreamReader reader = new StreamReader(path);
            string[] arrLines = new string[numberOfLines];
            
            for (int i = 0; i < numberOfLines; i++)
            {
                if (i+1 == line)
                {
                    arrLines[i] = valueToChange;
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
        
        
        protected string ReadFile(int lineNumber, TextAsset fileToRead)
        {
            lineNumber -= 10;
            var path = fileToRead.ToString();
            StreamReader reader = new StreamReader(path);
            string lineString = null;

            int lenght = GetNumberOfLines(path);
            
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
            reader.Close();
            return lineString;
        }
    }
}
