using UnityEngine;
using UnityEngine.InputSystem;

public class PowerObject : MonoBehaviour
{
    private InputAction mAbsorbAction;
    ColourChanger colourChanger;

    void Start()
    {
        mAbsorbAction = InputSystem.actions.FindAction("Absorb");
        colourChanger = GetComponentInChildren<ColourChanger>();
    }
    
    void Update()
    {
        if (mAbsorbAction.WasPerformedThisFrame())
        {
            Debug.Log("Absorb Pressed");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // If Player collides with a Power Orb
        if (collider.CompareTag("Player") && colourChanger.absorbable)
        {
            collider.GetComponent<PlayerCharacter>().AbsorbPower(colourChanger.powerColour);
            StartCoroutine(colourChanger.FadeToColour(colourChanger.fadedColour));
        }
    }
    
}
