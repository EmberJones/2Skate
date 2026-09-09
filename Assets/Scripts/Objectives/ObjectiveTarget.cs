using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjectiveTarget : MonoBehaviour
{
    public ObjectiveType objectiveType;

    [Tooltip("Progress this single interaction contributes")]
    public int progressAmount = 1;

    [Tooltip("Particle effect that plays in the world while this target is still active.")]
    public ParticleSystem indicatorParticles;

    [Tooltip("Deactivate this object once used")]
    public bool deactivateOnComplete = true;

    public string playerTag = "Player";

    private void Start()
    {
        if (indicatorParticles != null) indicatorParticles.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        Complete();
    }

    public void Complete()
    {
        
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.ReportProgress(objectiveType, progressAmount);
            
            //Debug.Log("Stop Particles");
            //indicatorParticles.loop = false;
        }

        if (indicatorParticles != null) 
        {
            //Debug.Log("Stop Particles");
            indicatorParticles.Stop(); 
        }

        if (deactivateOnComplete) indicatorParticles.gameObject.SetActive(false);

        //indicatorParticles.Stop();
    }
}
