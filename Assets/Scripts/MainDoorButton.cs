using UnityEngine;

public enum ButtonType {Orange, Green, Purple, Reset}
public class MainDoorButton : MonoBehaviour
{
    public ButtonType buttonType;
    private float maxDistance = 7.0f;
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
            if (hit.transform == transform)
            {
                Debug.Log(buttonType);
                return true;
            }
        }

        return false;
    }
    private void UseButton()
    {
        SoundManager.PlaySound(SoundType.Beep);
        if (buttonType == ButtonType.Reset)
        {
            MainDoorKeypad.code.Clear();
        }
        else
        {
            MainDoorKeypad.code.Add(buttonType);
        }
    }
}
