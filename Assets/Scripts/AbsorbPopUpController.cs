using System.IO.Compression;
using UnityEngine;

public class AbsorbPopUpController : MonoBehaviour
{

    void Start()
    {
        foreach (PowerObject obj in FindObjectsByType<PowerObject>(FindObjectsSortMode.None))
        {
            obj.OnPlayerNearby += ShowPrompt;
            obj.OnPlayerLeft += HidePrompt;
        }
        gameObject.SetActive(false);
    }
    public void ShowPrompt()
    {
        gameObject.SetActive(true);
    }
    public void HidePrompt()
    {
        gameObject.SetActive(false);
    }
}
