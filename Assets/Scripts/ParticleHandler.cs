using UnityEngine;

public static class ParticleHandler
{
    private static GameObject blueEmitter, redEmitter, yellowEmitter;
    private static GameObject activeEmitter;
    private static ParticleSystem ps;

    public static void Initialize()
    {
        blueEmitter = GameObject.Find("ParticleEmitterBlue");
        redEmitter = GameObject.Find("ParticleEmitterRed");
        yellowEmitter = GameObject.Find("ParticleEmitterYellow");

        activeEmitter = null;
        ps = null;
    }

    private static GameObject SelectEmitter(PowerType powerType)
    {
        return powerType switch
        {
            PowerType.Blue => blueEmitter,
            PowerType.Red => redEmitter,
            PowerType.Yellow => yellowEmitter,
            _ => blueEmitter,
        };
    }

    public static void StartAbsorbParticles()
    {
        if (!activeEmitter || !ps) return;

        activeEmitter.SetActive(true);
        ps.Play();
    }

    public static void StopAbsorbParticles()
    {
        if (!activeEmitter || !ps) return;

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        activeEmitter.SetActive(false);
    }

    public static void UpdateParticleEmitter(Transform player, Transform absorbingObject)
{
    if (!player || !absorbingObject) return;

    PowerType powerType = absorbingObject.GetComponentInChildren<ColourChanger>().powerType;

    // Pick emitter based on colour
    GameObject emitter = SelectEmitter(powerType);
    
    if (emitter != activeEmitter)
    {
        if (activeEmitter != null && ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            activeEmitter.SetActive(false);
        }

        activeEmitter = emitter;
        ps = activeEmitter?.GetComponent<ParticleSystem>();
    }

    if (!activeEmitter || !ps) return;

    // Position at absorbing object
    activeEmitter.transform.position = absorbingObject.position;

    // Rotate toward player
    Vector3 dir = (player.position - absorbingObject.position).normalized;
    if (dir != Vector3.zero)
        activeEmitter.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

    // Adjust particle shape length
    var shape = ps.shape;
    float distance = Vector3.Distance(player.position, absorbingObject.position);
    shape.scale = new Vector3(shape.scale.x, shape.scale.y, distance);

    if (!activeEmitter.activeSelf)
    {
        activeEmitter.SetActive(true);
        ps.Play();
    }
}
}
