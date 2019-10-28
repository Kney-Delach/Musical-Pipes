using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using LevelManagement;
using AudioDataSystem;
using TimeSystem;
using PipeSystem; 
using GameControllers;
// manages all the sound components of the game
public class AudioManager : MonoBehaviour
{
    // reference to master volume mixer 
    [SerializeField]
    private AudioMixer _masterVolumeMixer;

    // reference to array of game sounds  
    [SerializeField]
    private Sound[] _gameSounds;

    private string _activeSongName = "Tutorial"; //"Escape - Jaroslav Beck";
    public string ActivateSongName { get { return _activeSongName ; } }

    // reference to dictionary storing sounds with their respective names
    private Dictionary<string, Sound> _soundDictionary;

    private void Awake()
    {
        _soundDictionary = new Dictionary<string, Sound>();
    }

    public void SelectSong(string songName)
    {
        _activeSongName = songName;
    }

    public void StopSelectedSong()
    {
        StopSound(_activeSongName);
        AudioData.Instance.SetAudioSource(null);
        TimeController.Instance.SetAudioSource(null);
    }
    public void PlaySelectedSong()
    {
        AudioData.Instance.SetAudioSource(GetActiveSongSource());
        TimeController.Instance.SetAudioSource(GetActiveSongSource());
        PlaySound(_activeSongName);
    }

    public AudioSource GetActiveSongSource()
    {
        Sound sound; 
        if (_soundDictionary.TryGetValue(_activeSongName, out sound))
        {
            if(ActivateSongName == "Tutorial")
            {
                AudioBandItemGenerator.Instance.IsTutorial = true; 
                AudioBandItemGenerator.Instance.TutorialIndex = 0;
                AudioBandItemGenerator.Instance.spawnLifeBoost = false; 
                AudioBandItemGenerator.Instance.spawnSpeedBoost = false;
                AudioBandItemGenerator.Instance.spawnCollidableObjects = false; 
                AudioBandItemGenerator.Instance.endTutorial = false; 
                PlayerController.Instance.LivesLeft = 1;
            }
            else
                AudioBandItemGenerator.Instance.IsTutorial = false; 
           return sound._audioSource;
        }
        else 
        {
            Debug.LogError("AudioManager Error: Couldn't get active song sound: " + _activeSongName);
            return null;
        }
    }

    // plays an audioclip attached to a sound, referenced through sound name
    public void PlaySound(string soundName)
    {
        Sound sound; 
        if (_soundDictionary.TryGetValue(soundName, out sound))
        {
            if(!sound._audioSource.isPlaying)
                sound._audioSource.Play();
        }
    }

    public void StopSound(string soundName)
    {
        Sound sound;
        //Debug.Log("Stopping sound A: " + soundName + " - " + (_soundDictionary.TryGetValue(soundName, out sound).ToString()));
        if (_soundDictionary.TryGetValue(soundName, out sound))
        {
            sound._audioSource.Stop();
            sound.IsPaused = false;
        }
            
        
    }

    // pause all sfx
    public void PauseAudio()
    {
        foreach (KeyValuePair<string, Sound> sound in _soundDictionary)
        {
            if(sound.Value._audioSource.isPlaying)
            {
                sound.Value._audioSource.Pause();
                sound.Value.IsPaused = true;
            }

        }
    }

    // TODO: Bug when sfx played at the exact same time as being paused
    // unpauses sfx
    public void UnPauseAudio()
    {
        foreach (KeyValuePair<string, Sound> sound in _soundDictionary)
        {
            if (sound.Value.IsPaused)
            {
                sound.Value._audioSource.Play();
                sound.Value.IsPaused = false;
            }

        }
    }

    // subscribe to the settings menu 
    public void SubscribeToSettings(SettingsMenu settingsMenu)
    {
        SettingsMenu.Instance.notifyAudioObservers += OnAudioChange; // observation handler registration
    }

    // initialise sounds into dictionary and initialise looping music 
    public void InitializeSounds()
    {
        foreach (Sound sound in _gameSounds)
        {
            _soundDictionary.Add(sound._soundName, sound);

            sound._audioSource = gameObject.AddComponent<AudioSource>();
            sound._audioSource.clip = sound._audioClip;
            if (sound._volume != 0)
                sound._audioSource.volume = sound._volume;

            switch (sound._audioType)
            {
                case AudioTypes.Sfx:
                    sound._audioSource.outputAudioMixerGroup = _masterVolumeMixer.FindMatchingGroups("Sfx")[0];
                    sound._audioSource.loop = false;
                    break;
                case AudioTypes.Music:
                    sound._audioSource.outputAudioMixerGroup = _masterVolumeMixer.FindMatchingGroups("Music")[0];
                    sound._audioSource.loop = true;
                    Debug.Log("Song Added: " + sound._soundName);
                    Debug.Log("Active Sound: " + _activeSongName);
                    break;
                case AudioTypes.Master:
                    print("AudioManager UpdateAudioSettings: MASTER CASE REACHED, SOUNDS SHOULD NOT BE OF TYPE 'Master'"); //TODO: Remove this case?
                    break;
                default:
                    Debug.Log("AudioManager UpdateAudioSettings: DEFAULT CASE REACHED, BUG FOUND");
                    break;
            }
        }
    }

    // observer function called when audio settings values change
    private void OnAudioChange(AudioTypes audioType, float value)
    {
        SetVolume(audioType, value);
    }
    
    // sets the volume of a mixer
    private void SetVolume(AudioTypes audioType, float value)
    {
        if(_masterVolumeMixer == null)
        {
            Debug.Log("AudioManager SetVolume ERROR: invalid Master volume mixer");
            return;
        }

        switch(audioType)
        {
            case (AudioTypes.Music):
                _masterVolumeMixer.SetFloat("MusicVolume", value);
                break;
            case (AudioTypes.Sfx):
                _masterVolumeMixer.SetFloat("SfxVolume", value);
                break;
            case (AudioTypes.Master):
                _masterVolumeMixer.SetFloat("MasterVolume", value);
                break;
            default:
                Debug.Log("AudioManager SetVolume: Default case reached, please contact a developer with this message.");
                break;
        }
    }
}
