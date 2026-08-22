using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SkateController : MonoBehaviour
{
    [Header("Input Keys")]
    public Key forwardKey = Key.W;
    public Key backKey = Key.S;
    public Key turnLeftKey = Key.A;
    public Key turnRightKey = Key.D;

    [Header("Propulsion")]
    public float pushForce = 8f;
    public float rollFriction = 0.5f;

    [Header("Turning (Torque)")]
    public float turnTorque = 12f;
    public float angularDamping = 6f;
    public float maxAngularSpeed = 4f;

    [Header("Grip")]
    public float lateralGrip = 10f;

    [Header("Ground Alignment")]
    public LayerMask groundMask;
    public float rayLength = 0.5f;
    public float groundAlignTorque = 20f;
    public float groundAlignDamping = 4f;
    public Vector3 centerOfMassOffset = new Vector3(0f, -0.4f, 0f);

    Rigidbody rb;
    Vector3 groundNormal = Vector3.up;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = maxAngularSpeed;
        rb.centerOfMass = centerOfMassOffset;
    }

    void FixedUpdate()
    {
        CheckGround();
        ApplyGroundAlignmentTorque();
        ApplyTurnTorque();
        ApplyPropulsion();
        ApplySkateFriction();
    }

    void CheckGround()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, rayLength + 0.1f, groundMask);
        groundNormal = grounded ? hit.normal : Vector3.up;
    }

    void ApplyGroundAlignmentTorque()
    {
        Vector3 currentUp = transform.up;
        Vector3 torqueAxis = Vector3.Cross(currentUp, groundNormal);
        float angle = Vector3.Angle(currentUp, groundNormal);
        rb.AddTorque(torqueAxis * angle * groundAlignTorque * Mathf.Deg2Rad, ForceMode.Force);

        Vector3 angVel = rb.angularVelocity;
        Vector3 tiltAngVel = angVel - Vector3.Project(angVel, groundNormal);
        rb.AddTorque(-tiltAngVel * groundAlignDamping, ForceMode.Force);
    }

    void ApplyTurnTorque()
    {
        if (Keyboard.current == null) return;

        float turnInput = 0f;
        if (Keyboard.current[turnLeftKey].isPressed) turnInput -= 1f;
        if (Keyboard.current[turnRightKey].isPressed) turnInput += 1f;

        rb.AddTorque(groundNormal * turnInput * turnTorque, ForceMode.Force);

        Vector3 angVel = rb.angularVelocity;
        Vector3 upComponent = Vector3.Project(angVel, groundNormal);
        rb.angularVelocity -= upComponent * Mathf.Clamp01(angularDamping * Time.fixedDeltaTime);
    }

    void ApplyPropulsion()
    {
        if (!grounded || Keyboard.current == null) return;

        float moveInput = 0f;
        if (Keyboard.current[forwardKey].isPressed) moveInput += 1f;
        if (Keyboard.current[backKey].isPressed) moveInput -= 1f;

        rb.AddForce(transform.forward * moveInput * pushForce, ForceMode.Force);
    }

    void ApplySkateFriction()
    {
        Vector3 vel = rb.linearVelocity;
        Vector3 rollDir = transform.forward;
        Vector3 lateralDir = transform.right;

        float rollSpeed = Vector3.Dot(vel, rollDir);
        float lateralSpeed = Vector3.Dot(vel, lateralDir);
        float verticalSpeed = Vector3.Dot(vel, Vector3.up);

        lateralSpeed *= 1f - Mathf.Clamp01(lateralGrip * Time.fixedDeltaTime);
        rollSpeed *= 1f - Mathf.Clamp01(rollFriction * Time.fixedDeltaTime);

        rb.linearVelocity = rollDir * rollSpeed + lateralDir * lateralSpeed + Vector3.up * verticalSpeed;
    }
}