using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerObject : MonoBehaviour
{
    private InputAction mAbsorbAction;
    ColourChanger colourChanger;
    PlayerCharacter player;
    private bool insideAbsorbZone, absorbing;

    void Start()
    {
        absorbing = false;
        mAbsorbAction = InputSystem.actions.FindAction("Absorb");
        colourChanger = GetComponentInChildren<ColourChanger>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        if (insideAbsorbZone)
        {
            AbsorbingProcess();
        }
    }

    void AbsorbingProcess() 
    {
        if (mAbsorbAction.WasPressedThisFrame() && colourChanger.absorbable)
        {
            absorbing = true;
            Debug.Log("Pressed");
            StartCoroutine(colourChanger.FadeToColour(colourChanger.fadedColour));
        }

        if (!absorbing) { return; }

        if (mAbsorbAction.WasReleasedThisFrame()) //Stopped absorbing
        {
            absorbing = false;
            return;
        }

        if (mAbsorbAction.WasPerformedThisFrame()) //Player has drained colour and now can absorb power
        {
            player.AbsorbPower(colourChanger.powerColour);
            absorbing = false;
        }
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
            absorbing = false;
        }
    }
}
