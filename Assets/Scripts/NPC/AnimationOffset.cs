using UnityEngine;

public class AnimationOffset : MonoBehaviour
{
    public string stateName = "Skate";

    void Start()
    {
        var animator = GetComponent<Animator>();
        animator.Play(stateName, 0, Random.value);   
    }
}
