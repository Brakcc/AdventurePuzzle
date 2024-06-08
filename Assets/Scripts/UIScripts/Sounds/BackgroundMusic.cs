using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace UIScripts.Sounds
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] PlaySound forestBackgroundMusic;
        
        private void Start()
        {
            StartCoroutine(PlayBackgroundMusic());
        }

        IEnumerator PlayBackgroundMusic()
        {
            yield return new WaitForSeconds(1f);
            forestBackgroundMusic.PlayMySound();
            int time;
            forestBackgroundMusic.GetComponent<StudioEventEmitter>().EventDescription.getLength(out time);
            yield return new WaitForSeconds(time - 1f);
        }
    }
}
