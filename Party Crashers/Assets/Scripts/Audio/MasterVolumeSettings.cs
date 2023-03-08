/*****************************************************************************
 * Master Audio Mixer by AIAdev: 
 * Tutorial Video: https://youtube.com/shorts/_m6nTQOMFl0?feature=share
 * 
 ****************************************************************************/
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterVolumeSettings : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixer masterMixer;

    // Start is called before the first frame update
    void Start()
    {
        SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume", 100));
    }

    public void SetVolume(float _value)
    {
        if (_value < 1)
        {
            _value = .001f;
        }

        RefreshSlider(_value);
        PlayerPrefs.SetFloat("SavedMasterVolume", _value);
        masterMixer.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(volumeSlider.value);
    }

    public void RefreshSlider(float _value)
    {
        volumeSlider.value = _value;
    }
}
