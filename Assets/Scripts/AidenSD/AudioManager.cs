using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;
    
    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;
    
    
    [Header("Volume")]
    
    [Range(0, 1)]
    public float masterVolume = 1;
    
    [Range(0, 1)]
    public float musicVolume = 1;
    
    [Range(0, 1)]
    public float ambienceVolume = 1;
    
    [Range(0, 1)]
    public float SFXVolume = 1;


    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;
    
    // "Clément pourquoi il y a des bus dans le code ?" -Aiden
    // "Feur" -Clément

    
    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("On a plusieurs audio managers wtf !!!!");
        }
        instance = this;

        _eventInstances = new List<EventInstance>();
        _eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        // MASTER BUS LE MAITRE DES BUS
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        // LA GIGA AMBIENCE DANS LE BUS ??? BUS DISCOTHEQUE ??
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }
    
    
    private void Start()
    {
        //InitializeAmbience(FMODEvents.instance.ambience);
        //InitializeMusic(FMODEvents.instance.music);
    }


    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);
    }


    
    // ONE-SHOT SFX
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    
    
    // LOOP SFX
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    
    
    // CLEAN UP
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }
    
    
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in _eventEmitters)
        {
            emitter.Stop();
        }
    }
    
    
    private void OnDestroy()
    {
        CleanUp();
    }
    
    
    
    // AMBIANCE
    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    
    public void SetAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }
    
    
    
    // MUSIC
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void SetMusicArea(MusicArea area)
    {
        musicEventInstance.setParameterByName("area", (float)area);
    }
}
