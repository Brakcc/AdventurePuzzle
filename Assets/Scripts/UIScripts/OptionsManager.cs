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

        public InputActionAsset myInputAction;
        
        private void Start()
        {
            _optionsGroup = transform.GetChild(0).gameObject;
            _optionsGroup.SetActive(false);
            
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
    }
}
