using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PowerUI : MonoBehaviour
{
    private static Image progressBar;

    public static void InitUI()
    {
        progressBar = GameObject.Find("GUIs").transform.Find("PowerSlider").transform.Find("Slider").GetComponent<Image>();
    }

    public static void StartDurationSlider(float duration, Color color)
    {
        if (progressBar == null) InitUI();

        progressBar.color = color;
        progressBar.gameObject.SetActive(true);
        progressBar.transform.localScale = new Vector3(1f, 1f, 1f);

        CoroutineRunner.instance.StartCoroutine(DurationDrain(duration));
    }

    private static IEnumerator DurationDrain(float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scaleX = Mathf.Clamp01(1f - t / duration);
            progressBar.transform.localScale = new Vector3(scaleX, 1f, 1f);
            yield return null;
        }
        progressBar.gameObject.SetActive(false);
    }

    public static void StartUsesSlider(int totalUses, Color color)
    {
        if (progressBar == null) InitUI();

        progressBar.color = color;
        progressBar.gameObject.SetActive(true);
        progressBar.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public static void UpdateUsesSlider(int usesLeft, int maxUses)
    {
        float scaleX = Mathf.Clamp01((float)usesLeft / maxUses);
        progressBar.transform.localScale = new Vector3(scaleX, 1f, 1f);

        if (usesLeft <= 0) progressBar.gameObject.SetActive(false);
    }

    public static void HideSlider()
    {
        progressBar.gameObject.SetActive(false);
    }
}
