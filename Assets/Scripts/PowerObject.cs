using UnityEngine;

public class PowerOrb : MonoBehaviour
{
    public string colour;
    
    void Start()
    {
        
    }

    void Update()
    {

    }
    
    void OnTriggerEnter(Collider collider) {
        // If Player collides with a Power Orb
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerCharacter>().AbsorbPower(colour);
        }
    }
    
}
