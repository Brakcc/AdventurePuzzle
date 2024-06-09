using System;
using FMODUnity;
using UnityEngine;

namespace GameContent.AudioScripts
{
    public class AudioManagerTest : MonoBehaviour
    {
        [SerializeField] private new EventReference audio;
        
        public static AudioManagerTest Instance { get; set; }

        private void Awake()
        {
            if (Instance is not null)
                throw new Exception("tu fais de la merde connard");

            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                PlayAtPos(audio, transform.position);
        }

        public void PlayAtPos(EventReference sound, Vector3 pos)
        {
            RuntimeManager.PlayOneShot(sound, pos);
        }
    }
}