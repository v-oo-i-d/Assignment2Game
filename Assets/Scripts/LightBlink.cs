using UnityEngine;

public class LightBlink : MonoBehaviour
{
    private Light lightSource;
    private float initialIntensity;

    private Material glass;
    private float initialEmissionStrength;

    private readonly float flickerSpeed = 5f;
    private readonly float flickerIntensity = 1.5f;
    private readonly float randomness = 0.3f;

    void Start()
    {
        lightSource = transform.Find("Point Light").GetComponent<Light>();
        glass = transform.Find("Glass").GetComponent<Renderer>().material;

        if (lightSource) initialIntensity = lightSource.intensity;
        if (glass) initialEmissionStrength = glass.GetFloat("_EmissionStrength");
    }

    void Update()
    {
        if (!lightSource || !glass) return;

        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerIntensity;
        flicker += Random.Range(-randomness, randomness);

        flicker = Mathf.Max(0, flicker);

        lightSource.intensity = initialIntensity + flicker;
        glass.SetFloat("_EmissionStrength", initialEmissionStrength + flicker);
    }
}
