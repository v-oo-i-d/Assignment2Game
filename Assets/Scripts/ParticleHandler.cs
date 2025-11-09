using UnityEngine;

public static class ParticleHandler
{
    private static GameObject particleEmitter;
    private static ParticleSystem ps;

    public static void Initialize()
    {
        particleEmitter = GameObject.Find("ParticleEmitter");
        ps = particleEmitter?.GetComponent<ParticleSystem>();
    }

    public static void StartAbsorbParticles()
    {
        if (!particleEmitter || !ps) return;

        particleEmitter.SetActive(true);
        ps.Play();
    }

    public static void StopAbsorbParticles()
    {
        if (!particleEmitter || !ps) return;

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleEmitter.SetActive(false);
    }

    public static void UpdateParticleEmitter(Transform player, Transform absorbingObject)
    {
        Debug.Log(absorbingObject);
        if (!particleEmitter || !player || !absorbingObject) return;

        // Change colour
        Color objectColor = absorbingObject.GetComponentInChildren<ColourChanger>()?.originalColour ?? Color.white;
        objectColor.a = 1f;

        var main = ps.main;
        main.startColor = objectColor;

        if (particleEmitter.TryGetComponent<ParticleSystemRenderer>(out var renderer))
        {
            renderer.material.color = objectColor;
        }
        else
        {
            Debug.LogWarning("ParticleSystemRenderer not found on ParticleEmitter.");
        }

        // Position at absorbing object
        particleEmitter.transform.position = absorbingObject.position;

        // Rotate toward player
        Vector3 dir = (player.position - absorbingObject.position).normalized;
        if (dir != Vector3.zero) 
            particleEmitter.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        // Adjust Shape module
        if (ps)
        {
            var shape = ps.shape;
            float distance = Vector3.Distance(player.position, absorbingObject.position);
            shape.scale = new Vector3(shape.scale.x, shape.scale.y, distance);
        }
    }
}
