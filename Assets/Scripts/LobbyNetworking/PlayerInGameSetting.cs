using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInGameSetting : MonoBehaviour
{
    public Slider mouseSensitivitySlider;
    public Slider musicVolumeSlider;

    public Text mouseSensitivityText;
    public Text musicVolumeText;

    public void Start()
    {
        this.loadSettings();
    }

    public void setMouseSensitivity(float value)
    {
        mouseSensitivityText.text = value.ToString("#.##");
    }

    public void setMusicVolume(float value)
    {
        musicVolumeText.text = (value * 100).ToString("#");
    }

    public void saveSetting()
    {
        PlayerPrefs.SetFloat("MouseSensitivityPreference", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("VolumePreference", musicVolumeSlider.value);
    }

    public void loadSettings()
    {
        float temp = PlayerPrefs.GetFloat("MouseSensitivityPreference");
        mouseSensitivitySlider.value = temp;
        this.setMouseSensitivity(temp);

        temp = PlayerPrefs.GetFloat("VolumePreference");
        musicVolumeSlider.value = temp;
        this.setMusicVolume(temp);
    }
}
