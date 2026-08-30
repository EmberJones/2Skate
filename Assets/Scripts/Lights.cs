using UnityEngine;

public class Lights : MonoBehaviour
{
    [Header("Color Settings")]
    [Tooltip("How often (in seconds) the light picks a new target color")]
    public float changeInterval = 0.5f;

    [Tooltip("How quickly the light transitions toward the new color")]
    public float transitionSpeed = 5f;

    [Range(0f, 1f)]
    [Tooltip("1 = fully vivid colors, lower = more pastel/washed out")]
    public float saturation = 1f;

    [Range(0f, 1f)]
    [Tooltip("1 = full brightness colors, lower = dimmer colors")]
    public float brightness = 1f;

    [Header("Optional Intensity Flicker")]
    public bool randomizeIntensity = false;
    public float minIntensity = 1f;
    public float maxIntensity = 3f;

    private Light _light;
    private Color _targetColor;
    private float _timer;

    void Awake()
    {
        _light = GetComponent<Light>();
        PickNewColor();
        _light.color = _targetColor; // start on a color instead of fading in from default
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= changeInterval)
        {
            _timer = 0f;
            PickNewColor();
        }

        // Smoothly lerp toward the target color/intensity each frame
        _light.color = Color.Lerp(_light.color, _targetColor, Time.deltaTime * transitionSpeed);

        if (randomizeIntensity)
        {
            _light.intensity = Mathf.Lerp(
                _light.intensity,
                Random.Range(minIntensity, maxIntensity),
                Time.deltaTime * transitionSpeed
            );
        }
    }

    private void PickNewColor()
    {
        float hue = Random.value; // random point around the color wheel
        _targetColor = Color.HSVToRGB(hue, saturation, brightness);
    }
}

