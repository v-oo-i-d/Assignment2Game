using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TitleColour : MonoBehaviour
{
    public float speed = 1f;

    private TextMeshProUGUI titleText; 
    private Image underline;

    void Awake()
    {
        titleText = GetComponent<TextMeshProUGUI>();
        underline = transform.Find("Underline").transform.GetComponent<Image>();
    }

    void Update()
    {
        float h = Mathf.Repeat(Time.time * speed, 1f);
        Color c = Color.HSVToRGB(h, 0.5f, 1f);

        titleText.color = c;
        underline.color = c;
    }
}
