using UnityEngine;

public class MainDoorKeypad : MonoBehaviour
{
    public GameObject door;
    private bool correctCode = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (correctCode) OpenDoor();
        }
    }

    public void inputCode()
    {
        
    }

    public void OpenDoor()
    {
        if (!correctCode) return;

        door.GetComponent<Door>().Unlock();
        Destroy(gameObject);
    }
}
