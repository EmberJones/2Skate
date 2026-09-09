using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform arrow;
    public Transform barLocation;
    public PlayerCarrier carrier;
    Transform target;

    Vector3 GetTargetPosition(Transform target)
    {
        if (target == null) return Vector3.zero;

        Collider col = target.GetComponent<Collider>();
        if (col != null)
            return col.ClosestPoint(arrow.position);

        return target.position;
    }

    void Update()
    {
        //OPTIMISE THIS AFTER JAM
        if (carrier.CarriedFood != null)
        {
            if (carrier.CarriedFood.targetTable != null)
            {
                target = carrier.CarriedFood.targetTable.transform;
                //Debug.Log($"[Arrow] Carrying food -> table target: {target.name} at {target.position}");
            }
            else
            {
                target = barLocation;
                //Debug.LogWarning($"[Arrow] Carrying food '{carrier.CarriedFood.name}' but targetTable is NULL -> falling back to bar");
            }
        }
        else
        {
            FoodItem nearest = null;
            float nearestDist = float.MaxValue;

            //Debug.Log($"[Arrow] Available food count: {FoodItem.Available.Count}");

            foreach (var food in FoodItem.Available)
            {
                if (food == null)
                {
                    //Debug.LogWarning("[Arrow] Null entry found in FoodItem.Available list!");
                    continue;
                }

                if (food.isDelivered)
                {
                    //Debug.Log($"[Arrow] Skipping delivered food: {food.name}");
                    continue;
                }

                float dist = (food.transform.position - arrow.position).sqrMagnitude;
                //Debug.Log($"[Arrow] Candidate: {food.name} (delivered={food.isDelivered}) at {food.transform.position}, distSqr={dist}");

                if (dist < nearestDist) { nearestDist = dist; nearest = food; }
            }

            target = nearest != null ? nearest.transform : barLocation;
            //Debug.Log($"[Arrow] Chosen target: {(nearest != null ? nearest.name : "barLocation")} at {target.position}");
        }

        if (target == null) return;

        Vector3 targetPos = GetTargetPosition(target);
        Vector3 dir = targetPos - arrow.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f) return;

        Debug.DrawLine(arrow.position, targetPos, Color.red);

        float yaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 90f;
        arrow.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}