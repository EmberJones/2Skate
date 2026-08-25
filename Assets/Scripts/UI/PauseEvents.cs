using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class PauseEvents : MonoBehaviour
{

    [Header("Scene Names (must match Build Settings exactly)")]
    [SerializeField] private string gameplaySceneName = "MainLevel";
    [SerializeField] private string settingsSceneName = "Settings";
    [SerializeField] private string mainmenuSceneName = "MainMenu";

    private UIDocument document;

    [SerializeField] private AudioMixer audioMixer;

    private AudioSource audioSource;

    private Button resumeButton;
    private Button settingsButton;
    private Button mainmenuButton;

    private VisualElement pauseContainer;

    private List<Button> pauseButtons = new List<Button>();

    private void Start()
    {
        document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        var root = document.rootVisualElement;

        pauseContainer = root.Q<VisualElement>("PauseContainer");
        //pauseContainer.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        audioSource = GetComponent<AudioSource>();

        var root = document.rootVisualElement;

        resumeButton = root.Q<Button>("ResumeButton");
        resumeButton.RegisterCallback<ClickEvent>(OnResumeClick);

        settingsButton = root.Q<Button>("SettingsButton");
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);

        mainmenuButton = root.Q<Button>("MainMenuButton");
        mainmenuButton.RegisterCallback<ClickEvent>(OnMainMenuClick);

        pauseButtons = root.Query<Button>().ToList();

        for (int i = 0; i < pauseButtons.Count; i++)
        {
            pauseButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnDisable()
    {
        resumeButton.UnregisterCallback<ClickEvent>(OnResumeClick);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        mainmenuButton.UnregisterCallback<ClickEvent>(OnMainMenuClick);

        for (int i = 0; i < pauseButtons.Count; i++)
        {
            pauseButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnResumeClick(ClickEvent evt)
    {
        //Unpause Game
        pauseContainer.style.display = DisplayStyle.None;
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        if (!SceneManager.GetSceneByName(settingsSceneName).isLoaded)
        {
            SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
        }
    }

    private void OnMainMenuClick(ClickEvent evt)
    {
        SceneManager.LoadScene(mainmenuSceneName, LoadSceneMode.Single);
    }
    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
