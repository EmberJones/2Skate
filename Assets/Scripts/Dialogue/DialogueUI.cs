using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image iconImage;

    public void UpdateUI(BaseDialogueData dialogue, string sentence)
    {
        nameText.text = dialogue.name;
        dialogueText.text = sentence;

        if (string.IsNullOrEmpty(dialogue.icon))
        {
            Debug.LogWarning("Icon field is empty or null for dialogue: " + dialogue.id);
            return;
        }

        string path = "DialogueIcons/" + dialogue.icon;
        Debug.Log("Attempting to load icon at path: " + path);


        //Sprite icon = Resources.Load<Sprite>(dialogue.icon);
        //iconImage.sprite = icon;


        // Try loading as Sprite first
        Sprite icon = Resources.Load<Sprite>(path);

        if (icon == null)
        {
            Texture2D tex = Resources.Load<Texture2D>(path);
            if (tex == null)
            {
                Debug.LogError("File does not exist at path or is not imported correctly: " + path);
            }
            else
            {
                Debug.LogError("File EXISTS but is not a Sprite — go to the file in the Project window, " +
                               "set Texture Type to Sprite (2D and UI) and hit Apply");
            }
        }
        else
        {
            Debug.Log("Icon loaded successfully: " + dialogue.icon);

            if (iconImage == null)
                Debug.LogError("iconImage is not assigned on DialogueUI in the Inspector");
            else
            {
                iconImage.sprite = icon;
                Debug.Log("Icon applied to image successfully");
            }
        }

    }
}
