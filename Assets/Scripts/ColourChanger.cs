using System.Collections;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public string powerColour;
    public Color startingColour;
    public Color fadedColour;
    private Color currentColour;
    public bool faded;
    private float fadeTime; //How long before the colour changes fully
    Renderer objectRenderer;
    
    void Start()
    {
        faded = false;
        fadedColour = Color.gray;
        fadeTime = 2.0f;
        currentColour = startingColour;
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = currentColour;
    }

    void Update()
    {

    }
    
    public IEnumerator FadeToColour(Color colourChange)
    {
        float currentTime = 0f;
        while (currentTime < fadeTime)
        {
            objectRenderer.material.color = Color.Lerp(currentColour, colourChange, currentTime/fadeTime);
            currentTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        objectRenderer.material.color = colourChange; // Ensure the final color is set
    }
}
