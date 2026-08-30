using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ragdolllimbhitreceiver : MonoBehaviour
{
    public AIRagdollController controller;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (controller == null)
            controller = GetComponentInParent<AIRagdollController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check the root object's tag rather than the exact collider hit,
        // since the player's ragdoll can make contact via any limb.


        Rigidbody otherBody = collision.collider.attachedRigidbody;
        if (otherBody == null) return;

        if (otherBody.isKinematic) return;

        AIRagdollController otherController = otherBody.GetComponentInParent<AIRagdollController>();
        if (otherController != null && otherController == controller) return;

        controller.ReceiveHit(rb, collision);
    }
}
