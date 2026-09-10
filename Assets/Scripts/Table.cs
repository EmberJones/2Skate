using UnityEngine;

public class Table : MonoBehaviour
{
    public Transform foodPlacementPoint;
    void OnTriggerEnter(Collider other)
    {
        PlayerCarrier carrier = other.GetComponent<PlayerCarrier>();
        if (carrier == null || carrier.CarriedFood == null) return;

        if (carrier.CarriedFood.targetTable == this)
            carrier.DeliverFood(this);
    }
}