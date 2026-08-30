using UnityEngine;
using System.Collections;

public class AIRagdollController : MonoBehaviour
{
    [Header("Ragdoll Setup")]
    [Tooltip("All rigidbodies on the ragdoll's limbs (assign in Inspector or auto-collect in Awake).")]
    public Rigidbody[] ragdollBodies;

    [Tooltip("Animator driving the AI while it's not ragdolled.")]
    public Animator animator;

    [Header("Hit Reaction")]
    [Tooltip("Minimum impact force (impulse / fixedDeltaTime) required to trigger the ragdoll.")]
    public float activationForceThreshold = 8f;

    [Tooltip("How much of the impact force gets applied to the limb that was actually hit.")]
    public float hitLimbForceMultiplier = 1.5f;

    [Tooltip("How much force spreads to the rest of the body for a more readable knockback.")]
    public float wholeBodyForceMultiplier = 0.3f;

    [Header("Get Up")]
    [Tooltip("Minimum time to stay ragdolled before attempting to stand back up.")]
    public float ragdollDuration = 3f;

    [Tooltip("Below this speed (on every limb) the ragdoll is considered 'settled' and safe to stand up.")]
    public float settleVelocityThreshold = 0.15f;

    [Tooltip("Main hip/pelvis bone, used to reposition the root before switching back to animation.")]
    public Transform hipsBone;

    [Tooltip("Exact state name of the stand-up animation in the Animator Controller (not a parameter name).")]
    public string getUpStateName = "Stand";

    [Tooltip("Layers considered 'ground' for the post-getup ground snap raycast.")]
    public LayerMask groundLayerMask = ~0;

    [Tooltip("Vertical offset from the raycast hit point to the root's resting position (usually 0).")]
    public float groundOffset = 0f;

    Coroutine getUpRoutine;

    public bool IsRagdolled { get; private set; }

    void Awake()
    {
        if (ragdollBodies == null || ragdollBodies.Length == 0)
            ragdollBodies = GetComponentsInChildren<Rigidbody>();

        SetKinematic(true);
    }

    void SetKinematic(bool kinematic)
    {
        foreach (var rb in ragdollBodies)
        {
            if (rb == null) continue;
            rb.isKinematic = kinematic;
        }
    }

   
    public void ReceiveHit(Rigidbody hitBody, Collision collision)
    {
        if (IsRagdolled) return;

        float appliedForce = collision.impulse.magnitude / Time.fixedDeltaTime;
        if (appliedForce < activationForceThreshold) return;

        ActivateRagdoll(collision, hitBody, appliedForce);
    }

    void ActivateRagdoll(Collision collision, Rigidbody hitBody, float forceMagnitude)
    {
        IsRagdolled = true;

        if (getUpRoutine != null)
        {
            StopCoroutine(getUpRoutine);
            getUpRoutine = null;
        }

        if (animator != null)
            animator.enabled = false;

        SetKinematic(false);

        // Direction the player was moving at impact - swap for -contact.normal
        // if you'd rather push "away from the hit point" than "along player momentum".
        Vector3 forceDir = collision.relativeVelocity.normalized;
        ContactPoint contact = collision.GetContact(0);

        // Concentrate most of the force on the limb that was actually struck...
        hitBody.AddForceAtPosition(
            forceDir * forceMagnitude * hitLimbForceMultiplier,
            contact.point,
            ForceMode.Impulse);

        
        foreach (var rb in ragdollBodies)
        {
            if (rb == hitBody) continue;
            rb.AddForce(forceDir * forceMagnitude * wholeBodyForceMultiplier, ForceMode.Impulse);
        }

        getUpRoutine = StartCoroutine(GetUpAfterDelay());
    }

    IEnumerator GetUpAfterDelay()
    {
        yield return new WaitForSeconds(ragdollDuration);

        // Wait until the ragdoll has mostly stopped moving so it doesn't
        // pop up mid-tumble.
        yield return new WaitUntil(IsSettled);

        StandUp();
    }

    bool IsSettled()
    {
        float sqrThreshold = settleVelocityThreshold * settleVelocityThreshold;
        foreach (var rb in ragdollBodies)
        {
            if (rb == null) continue;
            if (rb.linearVelocity.sqrMagnitude > sqrThreshold)
                return false;
        }
        return true;
    }

    void StandUp()
    {
        AlignRootToRagdoll();
        SnapToGround();

        SetKinematic(true);

        if (animator != null)
        {
            animator.enabled = true;

            animator.Play(getUpStateName, 0, 0f);
            animator.Update(0f);
        }

        

        IsRagdolled = false;
        getUpRoutine = null;
    }


    void AlignRootToRagdoll()
    {
        if (hipsBone == null) return;

        Vector3 hipPosition = hipsBone.position;
        transform.position = new Vector3(hipPosition.x, transform.position.y, hipPosition.z);

        Vector3 hipForward = hipsBone.forward;
        hipForward.y = 0f;
        if (hipForward.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(hipForward.normalized);
    }

    void SnapToGround()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 2f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f, groundLayerMask, QueryTriggerInteraction.Ignore))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + groundOffset, transform.position.z);
        }
    }
}
