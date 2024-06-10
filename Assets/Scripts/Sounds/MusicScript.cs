using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Sounds
{
    public class MusicScript : MonoBehaviour
    {
        [SerializeField] private EventReference musicRef;
        private EventInstance _instanceMusic;
        [SerializeField] private Transform thePlayer;
        
        private void Start()
        {
            PlayMusic();
        }

        public void PlayMusic()
        {
            _instanceMusic = RuntimeManager.CreateInstance(musicRef);
            _instanceMusic.set3DAttributes(RuntimeUtils.To3DAttributes(thePlayer.position));
            _instanceMusic.start();
            StartCoroutine(RestartMusic());
        }

        IEnumerator RestartMusic()
        {
            _instanceMusic.getDescription(out var description);
            description.getLength(out int timeOfEvent);
            yield return new WaitForSecondsRealtime(timeOfEvent);
            PlayMusic();
        }

        public void StopMusic()
        {
            _instanceMusic.stop(STOP_MODE.ALLOWFADEOUT);
            StopCoroutine(RestartMusic());
        }
    }
}
