using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : Singleton<SettingsMenu>, IButtonListener
{
    // Variables
    [SerializeField] private EventSystem eventSystem;

    [Header("Settings UI")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button closeSettingsMenu;

    #region Volume Vars
    [Header("Volume")]
    [SerializeField] private TextMeshProUGUI masterVolumeText;
    [SerializeField] private Slider masterVolumeSlider;

    [SerializeField] private TextMeshProUGUI musicVolumeText;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private Slider sfxVolumeSlider;

    private float _currentMasterVolume;
    private float _currentMusicVolume;
    private float _currentSFXVolume;
    #endregion


    // Functions
    protected override void Awake()
    {
        base.Awake();

        closeSettingsMenu.onClick.AddListener(CloseMenu);
        closeSettingsMenu.onClick.AddListener(SaveSettings);

        masterVolumeSlider.onValueChanged.AddListener((value) => UpdateVolume(value, AudioMixerKeys.MasterVolumeKey, ref _currentMasterVolume, ref masterVolumeText));
        musicVolumeSlider.onValueChanged.AddListener((value) => UpdateVolume(value, AudioMixerKeys.MusicVolumeKey, ref _currentMusicVolume, ref musicVolumeText));
        sfxVolumeSlider.onValueChanged.AddListener((value) => UpdateVolume(value, AudioMixerKeys.SFXVolumeKey, ref _currentSFXVolume, ref sfxVolumeText));
    }

    private void Start()
    {
        var inputObject = FindFirstObjectByType<PlayerInputs>();
        inputObject.RegisterListener(this);
        LoadSettings();
    }

    #region Save/Load
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(AudioMixerKeys.MasterVolumeKey, _currentMasterVolume);
        PlayerPrefs.SetFloat(AudioMixerKeys.MusicVolumeKey, _currentMusicVolume);
        PlayerPrefs.SetFloat(AudioMixerKeys.SFXVolumeKey, _currentSFXVolume);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        _currentMasterVolume = PlayerPrefs.GetFloat(AudioMixerKeys.MasterVolumeKey);
        _currentMusicVolume = PlayerPrefs.GetFloat(AudioMixerKeys.MusicVolumeKey);
        _currentSFXVolume = PlayerPrefs.GetFloat(AudioMixerKeys.SFXVolumeKey);

        masterVolumeSlider.value = _currentMasterVolume;
        musicVolumeSlider.value = _currentMusicVolume;
        sfxVolumeSlider.value = _currentSFXVolume;

        UpdateVolume(_currentMasterVolume, AudioMixerKeys.MasterVolumeKey, ref _currentMasterVolume, ref masterVolumeText);
        UpdateVolume(_currentMusicVolume, AudioMixerKeys.MusicVolumeKey, ref _currentMusicVolume, ref musicVolumeText);
        UpdateVolume(_currentSFXVolume, AudioMixerKeys.SFXVolumeKey, ref _currentSFXVolume, ref sfxVolumeText);
    }
    #endregion

    public void OpenMenu()
    {
        settingsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(masterVolumeSlider.gameObject);
    }

    public void CloseMenu()
    {
        settingsMenu.SetActive(false);
        MainMenu.Instance.ShowMainMenu();
    }

    private void UpdateVolume(float value, string keyName, ref float currentValue, ref TextMeshProUGUI text)
    {
        currentValue = value;
        text.text = keyName + ": " + (int)(currentValue * 100.0f);

        AudioManager.Instance.SetMixerVolume(keyName, currentValue);
    }

    public void ButtonHeld(ButtonInfo heldInfo)
    {
    }

    public void ButtonPressed(ButtonInfo pressedInfo)
    {
    }

    public void ButtonReleased(ButtonInfo releasedInfo)
    {
        if (eventSystem.currentSelectedGameObject.TryGetComponent(out Slider slider))
        {
            if (releasedInfo.TimeHeld() <= 0.25f)
            {
                float currentValue = slider.value;

                currentValue += 0.1f;

                if (currentValue > 1.05f)
                    currentValue = 0.0f;

                slider.value = currentValue;
            }
        }
    }
}