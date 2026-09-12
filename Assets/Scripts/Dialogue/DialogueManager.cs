using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
    private DialogueQueue<string> sentenceQueue;
    private BaseDialogueData currentDialogue;
    public DialogueUI ui;
    public GameObject dialogueUI;
    public Action onDialogueEnd;
    //public GameObject newUI;

    void Awake()
    {
        //newUI.SetActive(false);
        sentenceQueue = new DialogueQueue<string>();

        if (dialogueUI != null) dialogueUI.SetActive(false);
    }

    public void StartDialogue(BaseDialogueData dialogue)
    {
        

        if (dialogueUI != null) dialogueUI.SetActive(true);

        currentDialogue = dialogue;

        sentenceQueue.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentenceQueue.Enqueue(sentence);
        }

        DisplayNext();
    }

    public void DisplayNext()
    {
        if (sentenceQueue.IsEmpty())
        {
            EndDialogue();
            return;
        }

        string sentence = sentenceQueue.Dequeue();
        // Debug.Log(sentence); 
        ui.UpdateUI(currentDialogue, sentence);
    }

    void EndDialogue()
    {

        if (dialogueUI != null) dialogueUI.SetActive(false);

        //Debug.Log("End of dialogue");
        //onDialogueEnd?.Invoke();
        //onDialogueEnd = null;

        Action snapshot = onDialogueEnd;
        onDialogueEnd = null;
        snapshot?.Invoke();

        //newUI.SetActive(true);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }


}
