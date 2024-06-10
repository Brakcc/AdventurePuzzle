using FMODUnity;
using UnityEngine;

namespace Sounds
{
    public class AmbianceChanger : MonoBehaviour
    {
        private BackgroundMusic _myBackgroundMusic;
        private bool _musicChanged;
        [SerializeField] private EventReference newAmbiance;
        void Start()
        {
            _musicChanged = false;
            _myBackgroundMusic = transform.parent.parent.GetComponent<BackgroundMusic>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_musicChanged && other.CompareTag("Player"))
            {
                Debug.Log("Ambiance Changed");
                _musicChanged = true;
                _myBackgroundMusic.ChangeBackgroundMusic(newAmbiance);
            }
        }
    }
}
