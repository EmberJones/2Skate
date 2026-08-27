using UnityEngine;

/// <summary>
/// Attach to each physically-simulated ragdoll bone. Every FixedUpdate it reads
/// the matching bone on the animated/target skeleton and steers this joint's
/// ConfigurableJoint.targetRotation toward it.
///
/// Position is NOT handled here - it relies entirely on Linear X/Y/Z Motion
/// being set to Locked on the joint, so the physics solver keeps the bones
/// pinned together and this script only ever has to fight over rotation.
/// </summary>
/// 
[RequireComponent(typeof(ConfigurableJoint))]
public class ActiveRagdollJointDriver : MonoBehaviour
{
    [Tooltip("The matching bone on the kinematic/animated skeleton (same hierarchy, same bind pose).")]
    [SerializeField] private Transform animatedBone;

    private ConfigurableJoint joint;
    private Quaternion startLocalRotation; // this bone's own rest-pose local rotation

    private void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
        startLocalRotation = transform.localRotation;
    }

    private void FixedUpdate()
    {
        joint.targetRotation = GetTargetRotation(animatedBone.localRotation, startLocalRotation);
    }

    private static Quaternion GetTargetRotation(Quaternion currentLocalRotation, Quaternion startLocalRotation)
    {
        Quaternion delta = Quaternion.Inverse(startLocalRotation) * currentLocalRotation;
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        return Quaternion.AngleAxis(angle, axis);
    }
}
