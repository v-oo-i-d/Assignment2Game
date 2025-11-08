using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PowerObject : MonoBehaviour
{
    private InputAction mAbsorbAction;
    private ColourChanger colourChanger;
    private PlayerCharacter player;
    private bool insideAbsorbZone, absorbing = false, resetColour;
    private float currentTime = 0f, fadeTime = 2f;
    public event System.Action OnPlayerLeft;
    public event System.Action OnPlayerNearby;

    void Start()
    {
        ResetFadeTime();
        mAbsorbAction = InputSystem.actions.FindAction("Absorb");
        colourChanger = GetComponentInChildren<ColourChanger>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();

        ParticleHandler.Initialize();
    }

    void Update()
    {
        AbsorbingProcess();

        if (absorbing) ParticleHandler.UpdateParticleEmitter(player.transform, transform);
        if (resetColour) StartCoroutine(RestoreColour(fadeTime));
    }

    void AbsorbingProcess()
    {
        if (!insideAbsorbZone) return;

        if (mAbsorbAction.WasPressedThisFrame() && colourChanger.absorbable)
        {
            absorbing = true;
            currentTime = 0f;
            ResetFadeTime();

            ParticleHandler.StartAbsorbParticles();
        }

        if (!absorbing) return;

        if (mAbsorbAction.IsPressed() && currentTime < fadeTime)
        {
            ParticleHandler.UpdateParticleEmitter(player.transform, transform);

            colourChanger.FadeToColour(colourChanger.fadedColour, currentTime, fadeTime);
            currentTime += Time.deltaTime;
            return;
        }

        if (mAbsorbAction.WasReleasedThisFrame())
        {
            InterruptAbsorbing();
            resetColour = true;
            absorbing = false;

            ParticleHandler.StopAbsorbParticles();
            return;
        }

        if (currentTime >= fadeTime)
        {
            currentTime = 0f;
            resetColour = true;
            player.AbsorbPower(colourChanger.powerType);
            absorbing = false;

            ParticleHandler.StopAbsorbParticles();
        }
    }

    public IEnumerator RestoreColour(float fadeTime)
    {
        resetColour = false;
        colourChanger.SetChangeStartingColor();
        while (currentTime < fadeTime)
        {
            colourChanger.FadeToColour(colourChanger.originalColour, currentTime, fadeTime);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    void ResetFadeTime()
    {
        fadeTime = 2.0f;
    }
    
    void InterruptAbsorbing()
    {
        fadeTime = currentTime;
        currentTime = 0f;
    }

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Vector3 direction = (transform.position - player.GetComponentInChildren<Camera>().transform.position).normalized;
            if (Vector3.Dot(direction, player.GetComponentInChildren<Camera>().transform.forward) < 0.8)
            {
                insideAbsorbZone = false;
                OnPlayerLeft.Invoke();
            }else
            {
                insideAbsorbZone = true;
                OnPlayerNearby.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideAbsorbZone = false;
            if (absorbing) { InterruptAbsorbing(); }
            resetColour = absorbing;
            absorbing = false;
            ParticleHandler.StopAbsorbParticles();
            OnPlayerLeft.Invoke();
        }
    }
}
