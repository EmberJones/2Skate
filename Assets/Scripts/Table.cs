using UnityEngine;

public class Table : MonoBehaviour
{

    public ParticleSystem indicatorParticles;

    public Transform foodPlacementPoint;
    void OnTriggerEnter(Collider other)
    {
        PlayerCarrier carrier = other.GetComponent<PlayerCarrier>();
        if (carrier == null) return;

        FoodItem food = carrier.CarriedItem as FoodItem;
        if (food != null && food.targetTable == this)
            carrier.DeliverFood(this);
    }
}