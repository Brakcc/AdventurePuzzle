using System;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Utilities;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace UIScripts
{
    public class FileWriter : MonoBehaviour
    {
        [HideInInspector] public float mainVolumeValue;
        [HideInInspector] public float musicVolumeValue;
        [HideInInspector] public float soundEffectVolumeValue;

        [HideInInspector] public string actionTouchName;
        [HideInInspector] public string pauseTouchName;

        [HideInInspector] public int saveChosen = 1;
        [HideInInspector] public Vector3 newPosCheckpoint;

        private void Start()
        {
            if (saveChosen == 0)
            {
                WriteData(4,null,null, 1, 0.5f,0.5f,0.5f);
                saveChosen = 1;
            }
        }

        protected void WriteData (int whichFileToChange, 
            [CanBeNull] float[] sliderValues = null, 
            [CanBeNull] string[] inputValues = null, 
            int saveFileNumber = 1, float xPos = 1f, float yPos = 1f, float zPos = 1f)
        {
            string path;
            switch (whichFileToChange)
            {
                case 1: //Sound
                    path = Application.persistentDataPath + "/soundData.data";
                    if (sliderValues is { Length: > 2 })
                    {
                        DataSound soundData = new(sliderValues[0], sliderValues[1], sliderValues[2]);
                        File.WriteAllText(path, soundData.generalVolume.ToString(CultureInfo.CurrentCulture) + ";"
                                                + "\n" + soundData.musicVolume.ToString(CultureInfo.CurrentCulture) + ";"
                                                + "\n" + soundData.soundEffectsVolume.ToString(CultureInfo.CurrentCulture));
                        
                        LoadData(1);
                    }
                    else
                    { Debug.Log("Missing Sound Reference"); }
                    break;
                
                case 2 : //Commands
                    path = Application.persistentDataPath + "/commandsData.data";
                    if (inputValues is { Length: 2 }) 
                    {
                        if (File.Exists(Application.persistentDataPath + "/commandsData.data"))
                        {
                            File.Delete(Application.persistentDataPath + "/commandsData.data");
                        }
                        DataCommands commandsData = new(inputValues[0], inputValues[1]);
                        File.WriteAllText(path, commandsData.actionCommandName + ";"
                                                + "\n" + commandsData.pauseCommandName);
                        
                        LoadData(2);
                    }
                    else{Debug.Log("Missing Commands Reference");}
                    break;
                case 3 : //Checkpoint
                    path = Application.persistentDataPath + "/saveData.data";
                    if (xPos != 1f && yPos != 1f && zPos != 1f)
                    {
                        DataSave saveData = new(saveFileNumber, new Vector3(xPos, yPos, zPos));
                        File.WriteAllText(path, saveData.saveChosen + ";"
                                                + "\n" + saveData.save1X + ";" + "\n" + saveData.save1Y + ";" + "\n" + saveData.save1Z + ";" 
                                                + "\n" + saveData.save2X + ";" + "\n" + saveData.save2Y + ";" + "\n" + saveData.save2Z + ";"
                                                + "\n" + saveData.save3X + ";" + "\n" + saveData.save3Y + ";" + "\n" + saveData.save3Z );
                        
                        LoadData(3);
                        
                    }
                    else{Debug.Log("Missing Position or SaveFile Reference");}
                    
                    break;
                case 4 :
                    path = Application.persistentDataPath + "/saveData.data";
                    File.WriteAllText(path, saveFileNumber + ";"
                                                            + "\n" + float.Parse(LoadValues(3)[1]) + ";" + "\n" + float.Parse(LoadValues(3)[2]) + ";" + "\n" + float.Parse(LoadValues(3)[3]) + ";" 
                                                            + "\n" + float.Parse(LoadValues(3)[4]) + ";" + "\n" +float.Parse(LoadValues(3)[5]) + ";" + "\n" + float.Parse(LoadValues(3)[6]) + ";"
                                                            + "\n" + float.Parse(LoadValues(3)[7]) + ";" + "\n" + float.Parse(LoadValues(3)[8]) + ";" + "\n" + float.Parse(LoadValues(3)[9]) );
                    LoadData(3);
                    break;
            }
            
        }

        protected string[] LoadValues(int whichValuesToLoad)
        {
            string path = "";
            switch (whichValuesToLoad)
            {
                case 1 :
                    path = Application.persistentDataPath + "/soundData.data";
                    break;
                case 2 :
                    path = Application.persistentDataPath + "/commandsData.data";
                    break;
                case 3 :
                    path = Application.persistentDataPath + "/saveData.data";
                    break;
            }

            if (File.Exists(path))
            {
                string data;
                string[] dataArray;
                data = File.ReadAllText(path);
                
                dataArray = Fonctions.UnpackData(data);
                return dataArray;
            }
            else
            {
                return null;
            }

        } 
        
        protected void LoadData(int whichFileToLoad)
        {
            if (LoadValues(whichFileToLoad) != null)
            {
                switch (whichFileToLoad)
                {
                    case 1 : //Sound
                        mainVolumeValue = float.Parse(LoadValues(whichFileToLoad)[0]);
                        musicVolumeValue = float.Parse(LoadValues(whichFileToLoad)[1]);
                        soundEffectVolumeValue = float.Parse(LoadValues(whichFileToLoad)[2]);
                        break;
                    
                    case 2 : //Command
                        actionTouchName = LoadValues(whichFileToLoad)[0];
                        pauseTouchName = LoadValues(whichFileToLoad)[1];
                        break;
                    
                    case 3 : //Saves
                        saveChosen = int.Parse(LoadValues(whichFileToLoad)[0]);
                        switch (saveChosen)
                        {
                            case 1 :
                                newPosCheckpoint = new Vector3(
                                    float.Parse(LoadValues(whichFileToLoad)[1]),
                                    float.Parse(LoadValues(whichFileToLoad)[2]),
                                    float.Parse(LoadValues(whichFileToLoad)[3]));
                                break;
                            case 2 :
                                newPosCheckpoint = new Vector3(
                                    float.Parse(LoadValues(whichFileToLoad)[4]),
                                    float.Parse(LoadValues(whichFileToLoad)[5]),
                                    float.Parse(LoadValues(whichFileToLoad)[6]));
                                break;
                            case 3 :
                                newPosCheckpoint = new Vector3(
                                    float.Parse(LoadValues(whichFileToLoad)[7]),
                                    float.Parse(LoadValues(whichFileToLoad)[8]),
                                    float.Parse(LoadValues(whichFileToLoad)[9]));
                                break;
                        }
                        break;
                }
            }
        }
    }
}
