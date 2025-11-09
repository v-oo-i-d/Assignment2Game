using UnityEngine;
public enum ButtonType {Orange, Green, Purple, Reset}
public class MainDoorButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ButtonType buttonType;
    private float maxDistance = 7.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (LookingAtButton()) UseButton();
        }
    }

    private bool LookingAtButton()
    {
        Camera cam = Camera.main;
        Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.transform == this.transform)
            {
                return true;
            }
        }

        return false;
    }
    private void UseButton()
    {
        if (buttonType == ButtonType.Reset)
        {
            MainDoorKeypad.code.Clear();
        }else
        {
            MainDoorKeypad.code.Add(buttonType);
        }
    }
}
