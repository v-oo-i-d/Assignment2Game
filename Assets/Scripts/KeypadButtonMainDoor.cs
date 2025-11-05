using UnityEngine;

public enum ButtonType { Orange, Green, Purple, Reset }

public class KeypadButtonMainDoor : MonoBehaviour
{
    public ButtonType buttonType;
    public GameObject keypadObject;
    private MainDoorKeypad keypadScript;
    public float maxDistance = 7f;

    void Start()
    {
        keypadScript = GameObject.Find("Keypad").GetComponent<MainDoorKeypad>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(LookingAtButton()) UseButton();
        }
    }

    private bool LookingAtButton()
    {
        Camera cam = Camera.main;
        Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Debug.Log(buttonType);
            return hit.transform == keypadObject.transform;
        }

        return false;
    }

    private void UseButton()
    {
        //keypadScript.inputCode(buttonType);
        //Debug.Log(buttonType);
    }
}
