using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PowerObject : MonoBehaviour
{
    private InputAction mAbsorbAction;
    ColourChanger colourChanger;
    PlayerCharacter player;
    private bool insideAbsorbZone, absorbing, resetColour;
    private float currentTime, fadeTime = 2f;
    public event System.Action OnPlayerLeft;
    public event System.Action OnPlayerNearby;


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
        if (!insideAbsorbZone)
        {
            return;
        }

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
            player.AbsorbPower(colourChanger.powerType);
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

    /*void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideAbsorbZone = true;
        }
    }*/

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
            if (absorbing) { interruptedAbsorbing(); }
            resetColour = absorbing;
            absorbing = false;
            OnPlayerLeft.Invoke();
        }
    }
}
