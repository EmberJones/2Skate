using UnityEngine;

public class BathroomCleaningArea : MonoBehaviour
{
    [Tooltip("Seconds the player must stay in the area, mop equipped, to finish cleaning.")]
    public float cleanDuration = 4f;

    [Tooltip("Particle effect (grime/flies/steam) shown while this bathroom still needs cleaning.")]
    public ParticleSystem indicatorParticles;

    public string playerTag = "Player";

    bool _cleaned = false;
    float _timer;
    PlayerCarrier _carrierInArea;

    public ProgressBar progressBar;

    private void Start()
    {
        if (indicatorParticles != null) indicatorParticles.Stop();
    }
   

    void OnTriggerEnter(Collider other)
    {
        if (_cleaned) return;

        PlayerCarrier carrier = other.GetComponentInParent<PlayerCarrier>();

        if (carrier != null)
        {
            _carrierInArea = carrier;

            if (carrier.CarriedItem is MopItem && indicatorParticles != null)
                indicatorParticles.Play();
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        _carrierInArea = null;
        _timer = 0f; // leaving resets progress, must clean in one continuous visit
    }

    void Update()
    {
        if (_cleaned || _carrierInArea == null) return;

        if (!(_carrierInArea.CarriedItem is MopItem))
        {
            Debug.Log("Carrier found and used");
            _timer = 0f; // wandered in without the mop, or dropped it = no progress
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= cleanDuration)
            FinishCleaning();

        progressBar.SetProgress(_timer, cleanDuration);
    }

    void FinishCleaning()
    {
        _cleaned = true;

        if (ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.ReportProgress(ObjectiveType.CleanBathroom);

        if (indicatorParticles != null) indicatorParticles.Stop();

        // Send the mop back to its spawn point so it's free for the next bathroom.
        //if (_carrierInArea != null)
            //_carrierInArea.DropCarriedItem();

        _carrierInArea = null;

        progressBar.gameObject.SetActive(false);
    }


}
