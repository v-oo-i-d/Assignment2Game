using System.Collections;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public string powerColour;
    public Color originalColour, fadedColour, changeStartingColor;
    private Color currentColour;
    public bool faded, absorbable;
    Renderer objectRenderer;
    
    void Start()
    {
        absorbable = true;
        fadedColour = Color.gray;
        currentColour = originalColour;
        changeStartingColor = originalColour;
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = currentColour;
    }

    void Update()
    {

    }

    public void SetChangeStartingColor()
    {
        changeStartingColor = currentColour;
    }
    
    public void FadeToColour(Color colourChange, float currentTime, float changeTime)
    {
        absorbable = false;
        
        objectRenderer.material.color = Color.Lerp(changeStartingColor, colourChange, currentTime / changeTime);
        
        if (currentTime + Time.deltaTime >= changeTime)
        {
            objectRenderer.material.color = colourChange; // Ensure the final color is set
            SetChangeStartingColor();
        }
        
        currentColour = objectRenderer.material.color;
        faded = (currentColour != originalColour);
        absorbable = (currentColour == originalColour);
        
    }
    
    // public IEnumerator FadeToColour(Color colourChange)
    // {
    //     absorbable = false;
    //     float currentTime = 0f;
    //     while (currentTime < fadeTime)
    //     {
    //         objectRenderer.material.color = Color.Lerp(currentColour, colourChange, currentTime/fadeTime);
    //         currentTime += Time.deltaTime;
    //         yield return null; // Wait for the next frame
    //     }
    //     objectRenderer.material.color = colourChange; // Ensure the final color is set
    //     currentColour = colourChange;

    //     faded = (currentColour != startingColour);
    //     if (faded)
    //     {
    //         StartCoroutine(FadeToColour(startingColour));
    //     }

    //     if (currentColour == startingColour)
    //     {
    //         absorbable = true;
    //     }
    // }
}
