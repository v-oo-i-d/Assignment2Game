using UnityEngine;

public class PowerOrb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnTriggerEnter(Collider collider) {
        // If Player collides with a Power Orb
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerCharacter>().AbsorbPower();
        }
    }
}
