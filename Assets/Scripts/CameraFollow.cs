using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0f, 3f, -6f);
    public float followSpeed = 8f;
    public float lookAtHeightOffset = 1f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        Vector3 lookTarget = target.position + Vector3.up * lookAtHeightOffset;
        transform.LookAt(lookTarget);
    }
}