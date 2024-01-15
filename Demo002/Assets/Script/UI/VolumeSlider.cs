using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SoundType _soundType;

    private void Start() 
    {
        switch(_soundType)
        {
            case SoundType.Music:
            _slider.value = AudioManager.Instance.MusicVolume;
            break;
            case SoundType.Sound:
            _slider.value = AudioManager.Instance.SoundVolume;
            break;
        }
        
        AudioManager.Instance.ChangeMasterVolume(_slider.value, _soundType);
        _slider?.onValueChanged.AddListener(val => AudioManager.Instance.ChangeMasterVolume(val, _soundType));
    }
}
