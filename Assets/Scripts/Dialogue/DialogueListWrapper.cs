using UnityEngine;
[System.Serializable]

//https://www.c-sharpcorner.com/blogs/wrapper-class-in-c-sharp1
public class DialogueListWrapper // The reason for a wrapper is that JsonUtility requires the name of the json array ( in this case dialogues)-
                                 // - to succfully load from the json file into the list in the code
{
    
    public BaseDialogueData[] dialogues;
    
}
