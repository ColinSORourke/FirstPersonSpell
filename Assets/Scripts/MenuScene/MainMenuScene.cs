using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;

public class MainMenuScene : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject mainCamera;
    public float cameraRotationSpeed;
    public Vector2 angles; //angles.x is min, angles.y is max
    public Dropdown cardDeckDropdown;

    //public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    //public Dropdown textureDropdown;
    //public Dropdown aaDropdown;
    public Slider volumeSlider;
    float currentVolume;
    Resolution[] resolutions;

    public GameObject playerCharacter;
    public GameObject[] characterModels;

    private bool isMenu = true;
    void Start()
    {
        if (mainCanvas == null)
        {
            Debug.Log("Missing mainCanvas object");
        }

        makeResolutionOptions();
    }

    void Update()
    {
        if ((!isMenu && this.mainCamera.transform.eulerAngles.y <= angles.y) || 
            (isMenu && this.mainCamera.transform.eulerAngles.y >= angles.x))
        {
            mainCamera.transform.Rotate(Vector3.up * this.cameraRotationSpeed * (isMenu ? -1 : 1) * Time.deltaTime, Space.World);
        }
            
    }

    public void loadLevel(string name)
    {
        Debug.Log(cardDeckDropdown.value + " - " + cardDeckDropdown.options[cardDeckDropdown.value].text);
        PlayerStateScript.playerCardDeckId = cardDeckDropdown.value;
        SceneManager.LoadScene(name);
    }

    public void rotateCamera()
    {
        this.isMenu = !this.isMenu;
    }

    public void quitGame()
    {   
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
                Application.Quit();
        #endif
    }

    private void makeResolutionOptions()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                     resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) QualitySettings.SetQualityLevel(qualityIndex);
        qualityDropdown.value = qualityIndex;
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        //PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        //PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else 
            qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
        /*
        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 1;
        */
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value = 0.5f;
    }

    public void selectCharacter(string newCharacter)
    {
        Material mat = playerCharacter.GetComponent<Renderer>().material;
        Debug.Log("New Color/Model: " + newCharacter);
        switch (newCharacter)
        {
            case "Red":
                mat.SetColor("_Color", Color.red);
                break;
            case "Blue":
                mat.SetColor("_Color", Color.blue);
                break;
            case "Green":
                mat.SetColor("_Color", Color.green);
                break;
            case "Black":
                mat.SetColor("_Color", Color.black);
                break;
            case "White":
                mat.SetColor("_Color", Color.white);
                break;
            case "Gray":
                mat.SetColor("_Color", Color.gray);
                break;
        }
    }

}
