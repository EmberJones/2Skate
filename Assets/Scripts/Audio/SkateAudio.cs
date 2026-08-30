using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SkateAudio : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkateController skateController;
    [SerializeField] private AudioSource skateSource;

    [Header("Skating Sound")]
    [SerializeField] private AudioClip skatingLoop;

    [Header("Speed Settings")]
    [SerializeField] private float minimumSpeed = 0.5f;
    [SerializeField] private float maximumSpeed = 12f;

    [Header("Audio Settings")]
    [SerializeField] private float minimumVolume = 0.0f;
    [SerializeField] private float maximumVolume = 1.0f;
    [SerializeField] private float minimumPitch = 0.85f;
    [SerializeField] private float maximumPitch = 1.2f;

    private Rigidbody rb;

    private void Awake()
    {
        if (skateController == null)
            skateController = GetComponent<SkateController>();

        if (skateSource == null)
            skateSource = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody>();

        skateSource.playOnAwake = false;
        skateSource.loop = true;
        skateSource.clip = skatingLoop;
    }

    private void Update()
    {
        if (skatingLoop == null)
            return;

        float speed = rb.linearVelocity.magnitude;

        bool shouldPlay =
            skateController.grounded &&
            speed > minimumSpeed;

        if (shouldPlay)
        {
            if (!skateSource.isPlaying)
                skateSource.Play();

            UpdateSkatingSound(speed);
        }
        else
        {
            if (skateSource.isPlaying)
                skateSource.Stop();
        }
    }

    private void UpdateSkatingSound(float speed)
    {
        float speedPercent = Mathf.InverseLerp(
            minimumSpeed,
            maximumSpeed,
            speed
        );

        // This controls the skating sound's volume relative to itself.
        // The AudioMixer still controls the final overall volume.
        skateSource.volume = Mathf.Lerp(
            minimumVolume,
            maximumVolume,
            speedPercent
        );

        skateSource.pitch = Mathf.Lerp(
            minimumPitch,
            maximumPitch,
            speedPercent
        );
    }
}
