using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public class HUDEvents : MonoBehaviour
{
    const string ClosedClass = "noteClosed";
    const string GameOverClass = "GameOver";
    const string GameWinClass = "GameWon";

    [SerializeField] string mainMenuSceneName = "MainMenu";

    UIDocument _document;
    VisualElement _notepad;
    Button _dropdownButton;
    Button _mainMenuButton;
    Button _restartButton;
    VisualElement _gameOverMenu;
    VisualElement _gameWinMenu;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        _notepad = root.Q<VisualElement>("Notepad");
        _dropdownButton = root.Q<Button>("Dropdown");
        _mainMenuButton = root.Q<Button>("MainMenuBtn");
        _restartButton = root.Q<Button>("RestartBtn");
        _gameOverMenu = root.Q<VisualElement>("GameOverContainer");
        _gameWinMenu = root.Q<VisualElement>("GameWinContainer");

        _dropdownButton.clicked += ToggleNotepad;
        _mainMenuButton.clicked += MainMenuClicked;
        _restartButton.clicked += RestartClicked;

        GameEvents.OnGameOver += ShowGameOver;
        GameEvents.OnGameWin += ShowGameWin;
    }

    void ToggleNotepad()
    {
        // First press removes noteClosed (opens), second press adds it back (closes), etc.
        _notepad.ToggleInClassList(ClosedClass);
    }

    void ShowGameOver()
    {
        _gameOverMenu.AddToClassList(GameOverClass);
    }

    void ShowGameWin()
    {
        _gameWinMenu.AddToClassList(GameWinClass);
    }

    void OnDisable()
    {
        if (_dropdownButton != null)
            _dropdownButton.clicked -= ToggleNotepad;
        if (_mainMenuButton != null)
            _mainMenuButton.clicked -= MainMenuClicked;
        if (_restartButton != null)
            _restartButton.clicked -= RestartClicked;

        GameEvents.OnGameOver -= ShowGameOver;
        GameEvents.OnGameWin -= ShowGameWin;
    }

    void MainMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void RestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
