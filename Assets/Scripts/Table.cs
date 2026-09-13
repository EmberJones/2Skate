using UnityEngine;

public class Table : MonoBehaviour
{

    public ParticleSystem indicatorParticles;

    public Transform foodPlacementPoint;

    public Outline outline;
    void OnTriggerEnter(Collider other)
    {
        PlayerCarrier carrier = other.GetComponentInParent<PlayerCarrier>();
        if (carrier == null) return;

        FoodItem food = carrier.CarriedItem as FoodItem;
        if (food != null && TableManager.Instance.IsTableActive(this))
        {
            carrier.DeliverFood(this);
            TableManager.Instance.DeactivateTable(this);
           
        }

    }

    public void SetActive(bool active)
    {
        if (indicatorParticles != null)
        {
            if (active)
            {
                indicatorParticles.Play();
                outline.enabled = true;
            }
            else 
            { 
                indicatorParticles.Stop(); 
                outline.enabled = false;
            }
        }
    }
}