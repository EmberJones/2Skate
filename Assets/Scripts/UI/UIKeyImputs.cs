using UnityEngine;

public class UIKeyImputs : MonoBehaviour
{
    public HUDEvents hud;
    public DialogueManager dManager;
    public bool active;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (active)
            {
                dManager.DisplayNext();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            hud.ToggleNotepad();
        }
    }
}
