using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float followSpeed = 8f;
    public float lookAtHeightOffset = 1f;

    [Header("Collision Avoidance")]
    public LayerMask collisionMask;
    public float collisionRadius = 0.3f;
    public float collisionBuffer = 0.15f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pivot = target.position + Vector3.up * lookAtHeightOffset;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);

        Vector3 direction = desiredPosition - pivot;
        float desiredDistance = direction.magnitude;
        direction.Normalize();

        float finalDistance = desiredDistance;

        if (Physics.SphereCast(pivot, collisionRadius, direction, out RaycastHit hit, desiredDistance, collisionMask))
        {
            finalDistance = Mathf.Max(hit.distance - collisionBuffer, 0f);
        }

        Vector3 adjustedPosition = pivot + direction * finalDistance;

        transform.position = Vector3.Lerp(transform.position, adjustedPosition, followSpeed * Time.deltaTime);
        transform.LookAt(pivot);
    }
}