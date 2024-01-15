using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;
    public AudioSource _hurtSource;

    public float MusicVolume = 1f;
    public float SoundVolume = 1f;

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1)
    {
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1)
    {
        _soundsSource?.PlayOneShot(clip, vol);
    }

    public void StopAllAudio()
    {
        _musicSource.Pause();
        _soundsSource.Pause();
        _hurtSource.Pause();
    }

    public void ChangeMasterVolume(float value, SoundType soundType)
    {
        switch(soundType)
        {
            case SoundType.Music:
            _musicSource.volume = value;
            MusicVolume = value;
            break;
            case SoundType.Sound:
            _soundsSource.volume = value;
            _hurtSource.volume = value;
            SoundVolume = value;
            break;
        }
    }
}
[Serializable] public enum SoundType
{
    Music = 0,
    Sound = 1
}