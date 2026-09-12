using UnityEngine;

public class DialogueTest : MonoBehaviour // This is a test class for a button to start the dialogue
{
    public DialogueLoader database;
    public DialogueManager manager;

    public string dialogueID; // which dialogue to load

    public void StartTestDialogue()
    {
        BaseDialogueData dialogue = database.dialogues.Find(d => d.id == dialogueID);

        if (dialogue != null)
        {
            Debug.Log("YOU?");
            manager.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogWarning("Dialogue not found: " + dialogueID);
        }
    }
}
