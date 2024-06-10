using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class AmbianceChanger : MonoBehaviour
    {
        private BackgroundMusic _myBackgroundAmbience;
        private MusicScript _myMusicScript;
        private bool _musicChanged;
        [SerializeField] private EventReference newAmbiance;
        [SerializeField] private EventReference newMusic;
        void Start()
        {
            _musicChanged = false;
            _myBackgroundAmbience = transform.parent.parent.GetComponent<BackgroundMusic>();
            _myMusicScript = transform.parent.parent.GetComponent<MusicScript>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_musicChanged && other.CompareTag("Player"))
            {
                Debug.Log("Ambiance Changed");
                _musicChanged = true;
                _myBackgroundAmbience.ChangeBackgroundMusic(newAmbiance);
                _myMusicScript.StopMusic();
                _myMusicScript.musicRef = newMusic;
                _myMusicScript.PlayMusic();
            }
        }
    }
}
