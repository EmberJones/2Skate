using UnityEngine;
using System.Collections.Generic;
using System.IO;

//https://www.youtube.com/watch?v=W85lx7QO6_8
//https://docs.unity3d.com/ScriptReference/JsonUtility.html

public class DialogueLoader : MonoBehaviour
{
    /*public List<BaseDialogueData> dialogues;

    void Awake()
    {
        LoadDialogue();
    }

    void LoadDialogue()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "dialogue.json");   // Finding the json file
        string json = File.ReadAllText(path);                                           // getiing the data / reading the text technically 

        dialogues = new List<BaseDialogueData>(JsonUtility.FromJson<DialogueListWrapper>(json).dialogues);  // Using the wrapper and the built in -
                                                                                                            // - JsonUtility to create the list containing the class (BaseDialogueData)
    }*/

    public List<BaseDialogueData> dialogues;

    void Awake()
    {
        LoadDialogue();
    }

    void LoadDialogue()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("dialogue"); // no extension, no path

        if (jsonFile == null)
        {
            Debug.LogError("DialogueLoader: dialogue.json not found in Resources folder.");
            return;
        }

        dialogues = new List<BaseDialogueData>(JsonUtility.FromJson<DialogueListWrapper>(jsonFile.text).dialogues);
    }
}

