using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class MainMenuScene : MonoBehaviour
{
    public Canvas mainCanvas;
    public GameObject mainCamera;
    public float cameraRotationSpeed;
    public Vector2 angles; //angles.x is min, angles.y is max
    public Dropdown cardDeckDropdown, levelSelectorDropdown;
    public Toggle fullScreenToggle;

    //public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    //public Dropdown textureDropdown;
    //public Dropdown aaDropdown;
    public Slider volumeSlider;
    public Slider musicVolumeSlider;
    public Slider mouseSensitivitySlider;

    Resolution[] resolutions;

    public GameObject playerCharacter;

    public Text mouseSensitivityText;
    public Text volumeText;
    public Text musicVolumeText;
    public GameObject KeybindsGroup;

    private bool isMenu = true;
    private int currentResolutionIndex = 0;

    void Start()
    {
        if (mainCanvas == null)
        {
            Debug.Log("Missing mainCanvas object");
        }

        makeResolutionOptions();
        LoadSettings();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if ((!isMenu && this.mainCamera.transform.eulerAngles.y <= angles.y) || 
            (isMenu && this.mainCamera.transform.eulerAngles.y >= angles.x))
        {
            mainCamera.transform.Rotate(Vector3.up * this.cameraRotationSpeed * (isMenu ? -1 : 1) * Time.deltaTime, Space.World);
        }
            
    }

    private void Awake()
    {
        if (!KeybindsGroup.activeSelf)
        {
            KeybindsGroup.SetActive(true);
            KeybindsGroup.SetActive(false);
        }
    }

    public void loadLevel()
    {
        Debug.Log(cardDeckDropdown.value + " - " + cardDeckDropdown.options[cardDeckDropdown.value].text);
        //PlayerStateScript.playerCardDeckId = cardDeckDropdown.value;
        SceneManager.LoadScene(levelSelectorDropdown.options[levelSelectorDropdown.value].text);
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
    public void setMouseSensitivityValue(float value) { mouseSensitivityText.text = value.ToString("#.##"); }
    public void setVolume(float volume) {
        //audioMixer.SetFloat("Volume", volume);
        volumeText.text = (volume*100).ToString("#");
    }

    public void setMusicVolume(float volume){
        musicVolumeText.text = (volume*100).ToString("#");
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", System.Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", volumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolumePreference", musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MouseSensitivityPreference", mouseSensitivitySlider.value);
    }
    public void LoadSettings()
    {
        fullScreenToggle.isOn = PlayerPrefs.HasKey("FullscreenPreference") ? System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference")) : Screen.fullScreen;
        resolutionDropdown.value = PlayerPrefs.HasKey("ResolutionPreference") ? PlayerPrefs.GetInt("ResolutionPreference") : currentResolutionIndex;
        Screen.fullScreen = PlayerPrefs.HasKey("FullscreenPreference") ? System.Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference")) : true;
        volumeSlider.value = PlayerPrefs.HasKey("VolumePreference") ? PlayerPrefs.GetFloat("VolumePreference") : 0.5f;
        musicVolumeSlider.value = PlayerPrefs.HasKey("MusicVolumePreference") ? PlayerPrefs.GetFloat("MusicVolumePreference") : 0.5f;
        mouseSensitivitySlider.value = PlayerPrefs.HasKey("MouseSensitivityPreference") ? PlayerPrefs.GetFloat("MouseSensitivityPreference") : 10.0f;
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteKey("ResolutionPreference");
        PlayerPrefs.DeleteKey("FullscreenPreference");
        PlayerPrefs.DeleteKey("VolumePreference");
        PlayerPrefs.DeleteKey("MusicVolumePreference");
        PlayerPrefs.DeleteKey("MouseSensitivityPreference");
        LoadSettings();
    }
}
