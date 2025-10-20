using UnityEngine;

public class PowerObject : MonoBehaviour
{
    void Start()
    {

    }
    
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        // If Player collides with a Power Orb
        if (collider.CompareTag("Player") && !GetComponentInChildren<ColourChanger>().faded)
        {
            collider.GetComponent<PlayerCharacter>().AbsorbPower(GetComponentInChildren<ColourChanger>().powerColour);
            StartCoroutine(GetComponentInChildren<ColourChanger>().FadeToColour(GetComponentInChildren<ColourChanger>().fadedColour));
        }
    }
    
}
