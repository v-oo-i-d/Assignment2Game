using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColourChanger : MonoBehaviour
{
    public PowerType powerType;
    public Color originalColour, fadedColour, changeStartingColor;
    private Color currentColour;
    public bool faded, absorbable;
    private Renderer objectRenderer;
    
    void Start()
    {
        absorbable = true;
        fadedColour = Color.gray;
        currentColour = originalColour;
        changeStartingColor = originalColour;
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material.color = currentColour;
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
        faded = currentColour != originalColour;
        absorbable = currentColour == originalColour;
        
    }
}
