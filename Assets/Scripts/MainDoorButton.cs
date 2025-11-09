using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public enum ButtonType {Orange, Green, Purple, Reset}
public class MainDoorButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ButtonType buttonType;
    private float maxDistance = 7.0f;
    private Vector3 startPos;
    void Start()
    {
        startPos = transform.localPosition;
    }

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
        StartCoroutine(ButtonPush());
        if (buttonType == ButtonType.Reset)
        {
            MainDoorKeypad.code.Clear();
        }
        else
        {
            MainDoorKeypad.code.Add(buttonType);
        }
    }
    IEnumerator ButtonPush()
    {
        float t = 0.0f;
        float duration = 0.2f;
        Vector3 endPos = startPos + new Vector3(0f, 0f, 0.15f);
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, progress);
            yield return null;
        }
        t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            transform.localPosition = Vector3.Lerp(endPos, startPos, progress);
            yield return null;
        }
    }
}
