using UnityEngine;

public class SceneDialogueTrigger : MonoBehaviour // No longer need this
{
    public DialogueLoader database;
    public DialogueManager manager;
    public string dialogueID;

    [Header("Intro")]
    public LevelIntroSequencer introSequencer; // drag in here if used with intro

    private bool hasPlayed = false;

    void Start()
    {
        // Only auto fire if not being controlled by LevelIntroSequencer
        if (introSequencer != null) return;

        TriggerDialogue();

    }

    public void TriggerDialogue()
    {
        if (hasPlayed) return;
        hasPlayed = true;

        BaseDialogueData dialogue = database.dialogues.Find(d => d.id == dialogueID);
        if (dialogue != null)
        {
            manager.StartDialogue(dialogue);
            manager.onDialogueEnd += OnDialogueEnd;
        }
        else
        {
            Debug.LogWarning("Scene dialogue not found: " + dialogueID);
            OnDialogueEnd();
        }
    }

    void OnDialogueEnd()
    {
        manager.onDialogueEnd -= OnDialogueEnd;

        // Tell the intro sequencer to finish up
        if (introSequencer != null)
            introSequencer.EndIntro();
    }
}
