using UnityEngine;

public class CursorController : MonoBehaviour
{
    private GameObject absorbPrompt, defaultCursor;

    void Start()
    {
        absorbPrompt = transform.Find("AbsorbPrompt").gameObject;
        defaultCursor = transform.Find("Crosshair").gameObject;

        foreach (PowerObject obj in FindObjectsByType<PowerObject>(FindObjectsSortMode.None))
        {
            obj.OnPlayerNearby += ShowAbsorbPrompt;
            obj.OnPlayerLeft += HideAbsorbPrompt;
        }

        absorbPrompt.SetActive(false);
        defaultCursor.SetActive(true);
    }

    void Update()
    {
        defaultCursor.SetActive(!absorbPrompt.activeSelf);
    }

    private void ShowAbsorbPrompt() => absorbPrompt.SetActive(true);
    private void HideAbsorbPrompt() => absorbPrompt.SetActive(false);
}