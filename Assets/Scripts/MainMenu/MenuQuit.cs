using UnityEngine;

public class MenuQuit : MonoBehaviour
{
    public void Quit()
    {
        // Doesn't actually work in unity editor
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
