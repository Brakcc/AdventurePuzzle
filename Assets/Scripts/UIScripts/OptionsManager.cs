using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UIScripts
{
    public class OptionsManager : FileWriter
    {
        private GameObject _optionsGroup;

        [SerializeField] private Slider sliderVolumePrincipal;
        [SerializeField] private Slider sliderVolumeMusique;
        [SerializeField] private Slider sliderVolumeSoundEffect;

        [Header("Référencer Pause Group si on est dans un niveau." + "\n" + "Main Menu Manager seulement si on est dans le Menu Principal.")]
        [SerializeField] private GameObject pauseGroup;
        [SerializeField] private MainMenuManager mainMenuManager;

        [SerializeField] private GameObject waitForInputGroup;
        private bool _newKeyPressed;
        private bool _waitForInput;
        private KeyCode _theNewKeyCode;

        [SerializeField] private InputActionAsset myInputAction;
        
        private void Start()
        {
            waitForInputGroup.SetActive(false);
            _theNewKeyCode = KeyCode.None;
            _optionsGroup = transform.GetChild(0).gameObject;
            _optionsGroup.SetActive(false);
            
        }

        private void Update()
        {
            if (_waitForInput)
            {
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key))
                    {
                        _theNewKeyCode = key;
                        Debug.Log(key);
                        _newKeyPressed = true;
                        _waitForInput = false;
                    }
                }
            }
        }

        public void ShowOptions()
        {
            _optionsGroup.SetActive(!_optionsGroup.activeSelf);
            if (pauseGroup == null)
            {
                mainMenuManager.optionsHere = _optionsGroup.activeSelf;
            }
            else 
            {
                pauseGroup.SetActive(!_optionsGroup.activeSelf);
            }
        }

        public void ChangeVolumeOptions()
        {
            float[] arraySound = new float[3];
            float volPrincValue = sliderVolumePrincipal.value;
            float volMusiqueValue = sliderVolumeMusique.value;
            float volSoundEffectValue = sliderVolumeSoundEffect.value;
            arraySound[0] = volPrincValue;
            arraySound[1] = volMusiqueValue;
            arraySound[2] = volSoundEffectValue;
            
            WriteData(1, arraySound);
        }

        public void ChangeKey(bool actionOrPauseKey)
        {
            Debug.Log("Pls press key");
            waitForInputGroup.SetActive(true);
            StartCoroutine(TestWaitTime(actionOrPauseKey));
            _waitForInput = true;
        }
        IEnumerator TestWaitTime(bool actionOrPauseKey)
        {
            yield return new WaitUntil(() => _newKeyPressed);
            Debug.Log("KeyPushed");
            _newKeyPressed = false;
            waitForInputGroup.SetActive(false);

            string[] arrayCommands = new string[2];
            
            if (actionOrPauseKey)
            {
                myInputAction["Interact"].AddBinding(_theNewKeyCode.ToString());
                arrayCommands[0] = _theNewKeyCode.ToString();
                arrayCommands[1] = LoadValues(2)[1];
                
            }
            else
            {
                myInputAction["Pause"].AddBinding(_theNewKeyCode.ToString());
                arrayCommands[0] = LoadValues(2)[0];
                arrayCommands[1] = _theNewKeyCode.ToString();
            }
            
            WriteData(2, null, arrayCommands);

            _theNewKeyCode = KeyCode.None;
        }
    }
}
