using UnityEngine;

public class PowerObject : MonoBehaviour
{
    ColourChanger colourChanger;
    void Start()
    {
        colourChanger = GetComponentInChildren<ColourChanger>();
    }
    
    void Update()
    {
        
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
