using System.Runtime.CompilerServices;
using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{
   public CharacterController characterController;

    private Rigidbody[] boneRigidbodies;

    private Animator animator;


    //this script can get all the ragdolls and set them up to be active or not.

    private void Awake()
    {
        boneRigidbodies = GetComponentsInChildren<Rigidbody>();

        animator = GetComponent<Animator>();

        //SetRagdollActive(false);
    }

    public void Activate()
    {
        SetRagdollActive(true);
    }

    private void SetRagdollActive(bool isActive)
    {
        if (animator != null) animator.enabled = isActive;

        if (characterController != null) characterController.enabled = isActive;

        foreach (Rigidbody rb in boneRigidbodies)
        {
            rb.isKinematic = isActive;
        }
    }
}
