using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class SettingsEvents : MonoBehaviour
{
    private UIDocument document;

    private Button cancelButton;
    private Button applyButton;
    private Button backButton;

    private DropdownField displayResolution;
    private DropdownField quality;

    private Toggle fullscreenToggle;

    private List<Button> settingsButtons = new List<Button>();

    [SerializeField] private AudioMixer audioMixer;
    private Slider volumeSlider;

    private AudioSource audioSource;


    private void Start()
    {
        volumeSlider = document.rootVisualElement.Q<Slider>("VolumeSlider");
        if (volumeSlider == null)
        {
            Debug.LogError("SettingsEvents: 'VolumeSlider' not found in UXML.");
        }
        else
        {
            volumeSlider.RegisterCallback<ChangeEvent<float>>(OnVolumeChanged);
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f); // load saved value
            SetMixerVolume(volumeSlider.value); // apply it immediately on open
        }

        fullscreenToggle = document.rootVisualElement.Q<Toggle>("FullScreen");
        if (fullscreenToggle == null)
        {
            Debug.LogError("SettingsEvents: 'FullscreenToggle' not found in UXML.");
        }
        else
        {
            fullscreenToggle.value = Screen.fullScreen; // reflect current state on open
        }
    }

    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        backButton = document.rootVisualElement.Q<Button>("SettingsBackButton");
        backButton.RegisterCallback<ClickEvent>(OnBackClick);

        //cancelButton = document.rootVisualElement.Q<Button>("CancelButton");
        //cancelButton.RegisterCallback<ClickEvent>(OnCancelClick);

        applyButton = document.rootVisualElement.Q<Button>("ApplyButton");
        applyButton.RegisterCallback<ClickEvent>(OnApplyClick);

       

        settingsButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < settingsButtons.Count; i++)
        {
            settingsButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

        InitDisplayResolutions();
        InitQualitySettings();
    }


    private void OnDisable()
    {
        backButton.UnregisterCallback<ClickEvent>(OnBackClick);
        //cancelButton.UnregisterCallback<ClickEvent>(OnCancelClick);
        applyButton.UnregisterCallback<ClickEvent>(OnApplyClick);
        if (volumeSlider != null) volumeSlider.UnregisterCallback<ChangeEvent<float>>(OnVolumeChanged);
    }

    private void OnBackClick(ClickEvent evt)
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    private void OnApplyClick(ClickEvent evt)
    {
        var resolution = Screen.resolutions[displayResolution.index];
        Screen.fullScreenMode = fullscreenToggle.value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(resolution.width, resolution.height, true);
        QualitySettings.SetQualityLevel(quality.index, true);
    }


    private void InitDisplayResolutions()
    {
        displayResolution = document.rootVisualElement.Q<DropdownField>("ResolutionDropdown");
        displayResolution.choices = Screen.resolutions.Select(resolution => $"{resolution.width}x{resolution.height}").ToList();
        displayResolution.index = Screen.resolutions
            .Select((resolution, index) => (resolution, index))
            .First((value) => value.resolution.width == Screen.currentResolution.width && value.resolution.height == Screen.currentResolution.height).index;
    }

    private void InitQualitySettings()
    {
        quality = document.rootVisualElement.Q<DropdownField>("QualityDropdown");
        quality.choices = QualitySettings.names.ToList();
        quality.index = QualitySettings.GetQualityLevel();
    }

    private void OnVolumeChanged(ChangeEvent<float> evt)
    {
        SetMixerVolume(evt.newValue);
        audioSource.Play();
    }

    private void SetMixerVolume(float linearValue)
    {
        // Sliders are linear (0-1), but the mixer works in decibels (logarithmic),
        // so a straight 0-1 -> 0dB-(-80dB) mapping sounds wrong this conversion fixes that.

        float dB = linearValue <= 0.0001f ? -80f : Mathf.Log10(linearValue) * 20f;
        audioMixer.SetFloat("MasterVolume", dB);
        PlayerPrefs.SetFloat("MasterVolume", linearValue);
        PlayerPrefs.Save();
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync(gameObject.scene);
        }
    }
}
