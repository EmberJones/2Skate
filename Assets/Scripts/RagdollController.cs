using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    // Drag the CharacterController here in the Inspector (it's on the character root)
    public CharacterController characterController;

    // We'll fill this with every bone Rigidbody on the character
    private Rigidbody[] boneRigidbodies;

    // Reference to the Animator that's driving the walking/idle animations
    private Animator animator;

    private void Awake()
    {
        // Find all Rigidbodies on the character and its children
        //boneRigidbodies = GetComponentsInChildren<Rigidbody>();

        // Grab the Animator
        animator = GetComponentInChildren<Animator>();

        // Start with ragdoll OFF
        SetRagdollActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Activate();
        }
    }

    public void Activate() //activates ragdoll
    {
        SetRagdollActive(true);
    }

    private void SetRagdollActive(bool isActive)
    {
        // Turn off the animator when ragdolling — animation and physics fight otherwise
        if (animator != null) animator.enabled = !isActive;

        // Turn off the CharacterController when ragdolling — its capsule shoves the bones around otherwise
        if (characterController != null) characterController.enabled = !isActive;

        // Flip every bone's kinematic state
        foreach (Rigidbody rb in boneRigidbodies)
        {
            rb.isKinematic = !isActive;
        }
    }
}
