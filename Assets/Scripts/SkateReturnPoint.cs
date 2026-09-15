using UnityEngine;

public class SkateReturnPoint : MonoBehaviour
{
    public static SkateReturnPoint Instance { get; private set; }

    public ParticleSystem indicatorParticles;
    //public Transform skateStoragePoint;
    public Outline outline;

    void Awake() => Instance = this;

    void OnTriggerEnter(Collider other)
    {
        PlayerCarrier carrier = other.GetComponentInParent<PlayerCarrier>();
        if (carrier == null) return;

        SkateItem skate = carrier.CarriedItem as SkateItem;
        if (skate != null)
            carrier.ReturnSkate(this);
    }

    public void SetHighlight(bool active)
    {
        if (indicatorParticles != null)
        {
            if (active) indicatorParticles.Play();
            else indicatorParticles.Stop();
        }
        if (outline != null) outline.enabled = active;
    }
}
