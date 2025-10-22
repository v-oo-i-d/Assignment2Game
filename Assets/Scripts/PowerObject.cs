using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PowerObject : MonoBehaviour
{
    private InputAction mAbsorbAction;
    ColourChanger colourChanger;
    PlayerCharacter player;
    private bool insideAbsorbZone, absorbing, resetColour;
    float currentTime, fadeTime; 

    void Start()
    {
        resetFadeTime();
        currentTime = 0f;
        absorbing = false;
        mAbsorbAction = InputSystem.actions.FindAction("Absorb");
        colourChanger = GetComponentInChildren<ColourChanger>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        AbsorbingProcess();
        if (resetColour)
        {
            StartCoroutine(restoreColour(fadeTime));
        }
    }

    void AbsorbingProcess()
    {
        if (!insideAbsorbZone) { return; }

        if (mAbsorbAction.WasPressedThisFrame() && colourChanger.absorbable)
        {
            absorbing = true;
            currentTime = 0f;
            resetFadeTime();
        }

        if (!absorbing) { return; }

        if (mAbsorbAction.IsPressed() && currentTime < fadeTime)
        {
            colourChanger.FadeToColour(colourChanger.fadedColour, currentTime, fadeTime);
            currentTime += Time.deltaTime;
            return;
        }

        if (mAbsorbAction.WasReleasedThisFrame()) //Stopped absorbing
        {
            interruptedAbsorbing();
            resetColour = true;
            absorbing = false;
            return;
        }

        if (currentTime >= fadeTime) //Player has drained colour and now can absorb power
        {
            currentTime = 0f;
            resetColour = true;
            player.AbsorbPower(colourChanger.powerColour);
            absorbing = false;
        }
    }

    public IEnumerator restoreColour(float fadeTime)
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

    void resetFadeTime()
    {
        fadeTime = 2.0f;
    }
    
    void interruptedAbsorbing()
    {
        fadeTime = currentTime;
        currentTime = 0f;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideAbsorbZone = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideAbsorbZone = false;
            interruptedAbsorbing();
            resetColour = absorbing;
            absorbing = false;
        }
    }
}
