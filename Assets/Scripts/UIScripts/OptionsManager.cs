using System.Globalization;
using UnityEngine;
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

        public void ChangeVolumeOptions(int line)
        {
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

            ChangeFile(line, newValue);
        }
        
    }
}
