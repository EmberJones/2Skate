using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.Rendering.DebugUI.Table;

//https://learn.unity.com/tutorial/waypoints
//https://medium.com/@be.content23/creating-a-simple-waypoint-system-in-unity-a-beginners-guide-ed3aa1f11353


public class LevelIntroSequencer : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueLoader database;
    public DialogueManager manager;
    public string introDialogueID;

    [Header("Cameras")]
    public GameObject cutsceneCamera;
    public GameObject playerCamera;

    [Header("Player")]
    public GameObject player;
    //public MonoBehaviour playerController;

    [Header("Cutscene Path")]
    public Transform[] cameraWaypoints;
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;

    //[Header("UI")]
    //public GameObject dialogueUI;

    [Header("Dialogue")]
    public SceneDialogueTrigger sceneDialogueTrigger;

    private bool hasPlayed = false;

    //public bool[] snapAtWaypoint;

    //[Header("Intro Platform")]
   // public GameObject introPlatform;

    [Header("UI")]
    public GameObject hud;

    public Timer timer;

    void Start()
    {
        hud.SetActive(false);
        if (hasPlayed) return;
        hasPlayed = true;

        playerCamera.SetActive(false);
        cutsceneCamera.SetActive(true);

        //if (playerController != null) playerController.enabled = false;

        // Fire dialogue immediately, camera moves at the same time
        if (sceneDialogueTrigger != null)
            sceneDialogueTrigger.TriggerDialogue();

        StartCoroutine(RunIntroSequence());
    }

    IEnumerator RunIntroSequence()
    {
        foreach (Transform waypoint in cameraWaypoints)
        {
            while (Vector3.Distance(cutsceneCamera.transform.position, waypoint.position) > 0.1f)
            {
                cutsceneCamera.transform.position = Vector3.MoveTowards(
                    cutsceneCamera.transform.position,
                    waypoint.position,
                    moveSpeed * Time.deltaTime
                );

                if (player != null)
                {
                    Vector3 direction = player.transform.position - cutsceneCamera.transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    cutsceneCamera.transform.rotation = Quaternion.Slerp(
                        cutsceneCamera.transform.rotation,
                        targetRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }

                yield return null;
            }

            // Snap to player , didnt like to lazt to remove
            //if (player != null)
            //{
            //   Vector3 finalDirection = player.transform.position - cutsceneCamera.transform.position;
            // cutsceneCamera.transform.rotation = Quaternion.LookRotation(finalDirection);
            //}
        }


    }

    public void EndIntro()
    {
        cutsceneCamera.SetActive(false);
        playerCamera.SetActive(true);
        //hud.SetActive(true);

        hud.SetActive(true);

        //if (playerController != null) playerController.enabled = true;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        timer.StartTimer();

        Debug.Log("Intro done, player has control");


        //if (platformPrefab != null && player != null)
        //{
        //    Vector3 spawnPosition = player.transform.position + platformOffset;
        //   spawnedPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
        //}

        //if (introPlatform != null) introPlatform.SetActive(true);
    }

}
