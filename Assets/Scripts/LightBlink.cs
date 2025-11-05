using UnityEngine;

public class LightBlink : MonoBehaviour
{
    private Light lightSource;
    private float initialIntensity;

    private readonly float flickerSpeed = 5f;
    private readonly float flickerIntensity = 1.5f;
    private readonly float randomness = 0.3f;

    void Start()
    {
        lightSource = transform.Find("Point Light").GetComponent<Light>();
        if (lightSource) initialIntensity = lightSource.intensity;
    }

    void Update()
    {
        if (!lightSource) return;

        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerIntensity;
        flicker += Random.Range(-randomness, randomness);
        flicker = Mathf.Max(0, flicker);

        lightSource.intensity = initialIntensity + flicker;
    }
}
