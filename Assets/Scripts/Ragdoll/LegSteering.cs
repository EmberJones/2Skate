using UnityEngine;
using UnityEngine.InputSystem;

public class LegSteering : MonoBehaviour
{
    [Header("Target Skeleton")]
    [Tooltip("This leg's thigh/upper-leg bone on the TARGET skeleton, not the physical ragdoll.")]
    [SerializeField] private Transform targetThigh;

    [Header("Input")]
    public Key turnLeftKey = Key.A;
    public Key turnRightKey = Key.D;
    public float turnSpeed = 90f; // degrees per second

    private void FixedUpdate()
    {
        if (Keyboard.current == null) return;

        float turnInput = 0f;
        if (Keyboard.current[turnLeftKey].isPressed) turnInput += 1f;
        if (Keyboard.current[turnRightKey].isPressed) turnInput -= 1f;
        if (turnInput == 0f) return;

        float deltaYaw = turnInput * turnSpeed * Time.fixedDeltaTime;
        targetThigh.rotation = Quaternion.AngleAxis(deltaYaw, Vector3.up) * targetThigh.rotation;
    }
}
