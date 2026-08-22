using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MainMenuEvents : MonoBehaviour
{
    [Header("Scene Names (must match Build Settings exactly)")]
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string settingsSceneName = "SettingsMenu";

    private UIDocument document;

    private VisualElement visualElement;

    private Button startButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button creditsBackButton;
    private Button quitButton;

    private VisualElement creditsPanel;

    private List<Button> menuButtons = new List<Button>();

    private AudioSource audioSource;

    

    private void Awake()
    {
        document = GetComponent<UIDocument>();

        audioSource = GetComponent<AudioSource>();

        var root = document.rootVisualElement;

        startButton = root.Q<Button>("StartButton");
        settingsButton = root.Q<Button>("SettingsButton");
        creditsButton = root.Q<Button>("CreditsButton");
        creditsBackButton = root.Q<Button>("CreditsBackButton");
        quitButton = root.Q<Button>("QuitButton");

        creditsPanel = root.Q<VisualElement>("CreditsPanel");

        startButton.RegisterCallback<ClickEvent>(OnStartClick);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);
        creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);
        creditsBackButton.RegisterCallback<ClickEvent>(OnCreditsCloseClick);
        quitButton.RegisterCallback<ClickEvent>(OnQuitClick);


        menuButtons = document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void Start()
    {
        var settingsBack = document.rootVisualElement.Q("SettingsCloseButton");
        settingsBack.style.display = DisplayStyle.None;
    }

    private void OnDisable()
    {
        startButton.UnregisterCallback<ClickEvent>(OnStartClick);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsClick);
        creditsButton.UnregisterCallback<ClickEvent>(OnCreditsClick);
        creditsBackButton.UnregisterCallback<ClickEvent>(OnCreditsCloseClick);
        quitButton.UnregisterCallback<ClickEvent>(OnQuitClick);

        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("Start Game Button Pressed");
        //SceneManager.LoadScene("Level 1");
    }

    private void OnSettingsClick(ClickEvent evt)
    {
        if (!SceneManager.GetSceneByName(settingsSceneName).isLoaded)
        {
            SceneManager.LoadScene(settingsSceneName, LoadSceneMode.Additive);
        }
        //var settingsBack = document.rootVisualElement.Q("SettingsCloseButton");
        //settingsBack.style.display = DisplayStyle.Flex;
    }


    private void OnCreditsClick(ClickEvent evt)
    {
        creditsPanel.style.display = DisplayStyle.Flex;
    }

    private void OnCreditsCloseClick(ClickEvent evt)
    {
        creditsPanel.style.display = DisplayStyle.None;
    }
    private void OnQuitClick(ClickEvent evt)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    private void OnAllButtonsClick(ClickEvent evt)
    {
        audioSource.Play();
    }
}
