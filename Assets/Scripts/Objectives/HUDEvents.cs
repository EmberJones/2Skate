using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class HUDEvents : MonoBehaviour
{
    const string ClosedClass = "noteClosed";

    UIDocument _document;
    VisualElement _notepad;
    Button _dropdownButton;

    void Awake()
    {
        _document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        VisualElement root = _document.rootVisualElement;

        _notepad = root.Q<VisualElement>("Notepad");
        _dropdownButton = root.Q<Button>("Dropdown");

        _dropdownButton.clicked += ToggleNotepad;
    }

    void ToggleNotepad()
    {
        // First press removes noteClosed (opens), second press adds it back (closes), etc.
        _notepad.ToggleInClassList(ClosedClass);
    }

    void OnDisable()
    {
        if (_dropdownButton != null)
            _dropdownButton.clicked -= ToggleNotepad;
    }
}
