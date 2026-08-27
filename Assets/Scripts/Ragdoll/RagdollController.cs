using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RagdollController : MonoBehaviour
{
    private enum AI_State
    {
        Standing,
        Skating,
        GetUp,
        Ragdoll
    }

    private AI_State currentState = AI_State.Standing;

  
    //threshold
    //[SerializeField] private float minimumForce; 
    //[SerializeField] private float maximumForce;

    // Drag the CharacterController here in the Inspector (it's on the character root)
    public NavMeshAgent NPCcontroller;

    public CapsuleCollider Capcollider;

    // We'll fill this with every bone Rigidbody on the character
    private Rigidbody[] boneRigidbodies;

    // Reference to the Animator that's driving the walking/idle animations
    private Animator animator;

    private void Awake()
    {
        // Find all Rigidbodies on the character and its children
        boneRigidbodies = GetComponentsInChildren<Rigidbody>();

        Capcollider = GetComponent<CapsuleCollider>();

        // Grab the Animator
        animator = GetComponentInChildren<Animator>();

        // Start with ragdoll OFF
        SetRagdollActive(false);
    }

    private void Update()
    {
        switch (currentState) 
        { 
            case AI_State.Standing:
                StandingBehaviour();
                break;

            case AI_State.Skating:
                SkatingBehaviour();
                break;

            case AI_State.Ragdoll:
                RagdollBehaviour();
                break;

            case AI_State.GetUp:
                GetUpBehaviour();
                break;
        }
    }

    public void Activate() //activates ragdoll
    {
        SetRagdollActive(true);
    }

    public void Deactivate()
    {
        SetRagdollActive(false);
    }

    private void SetRagdollActive(bool isActive)
    {
        // Turn off the animator when ragdolling — animation and physics fight otherwise
        if (animator != null) animator.enabled = !isActive;

        // Turn off the CharacterController when ragdolling — its capsule shoves the bones around otherwise
        if (NPCcontroller != null) NPCcontroller.enabled = !isActive;

        if(Capcollider != null) Capcollider.enabled = !isActive;

        // Flip every bone's kinematic state
        foreach (Rigidbody rb in boneRigidbodies)
        {
            rb.isKinematic = !isActive;
        }
    }

    

    private void SkatingBehaviour()
    {

    }

    private void RagdollBehaviour()
    {

    }

    private void StandingBehaviour()
    {

        if (Input.GetKeyDown(KeyCode.F)) 
        { 
            Activate();
        }

    }

    private void TriggerRagdoll(Vector3 force, Vector3 hitpoinnt)
    {

    }

    private void GetUpBehaviour()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    

    

}
