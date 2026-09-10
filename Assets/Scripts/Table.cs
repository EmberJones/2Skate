using UnityEngine;

public class Table : MonoBehaviour
{

    public ParticleSystem indicatorParticles;

    public Transform foodPlacementPoint;
    void OnTriggerEnter(Collider other)
    {
        PlayerCarrier carrier = other.GetComponent<PlayerCarrier>();
        if (carrier == null || carrier.CarriedFood == null) return;

        if (carrier.CarriedFood.targetTable == this)
        {
            carrier.DeliverFood(this);
            //indicatorParticles.Stop();
        }
            
    }
}