using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("SceneMusic: No AudioManager exists.");
            return;
        }

        AudioManager.Instance.PlayMusic(music);
    }
}
