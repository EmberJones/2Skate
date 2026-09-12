using UnityEngine;

public class HitboxDialogueTrigger : MonoBehaviour
{
    public DialogueLoader database;
    public DialogueManager manager;
    public string dialogueID;
    public string playerTag = "Player";
    public bool triggerOnce = true;

    private bool hasTriggered = false;
    //[Header("UI")]
    //public GameObject dialogueUI;

   // void Start()
    //{
        // Make sure UI is hidden at the start
       // if (dialogueUI != null) dialogueUI.SetActive(false);

       // manager.onDialogueEnd += OnDialogueEnd;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (triggerOnce && hasTriggered) return;

            BaseDialogueData dialogue = database.dialogues.Find(d => d.id == dialogueID);
            if (dialogue != null)
            {

                hasTriggered = true;
                manager.StartDialogue(dialogue);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Debug.LogWarning("Hitbox dialogue not found: " + dialogueID);
            }
        }
    }

   // void OnDialogueEnd()
    //{
        // Hide UI when dialogue finishes
        //if (dialogueUI != null) dialogueUI.SetActive(false);
   //
    //void OnDestroy()
   // {
        // Clean up subscription when object is destroyed
      //  manager.onDialogueEnd -= OnDialogueEnd;
  //  }
}
