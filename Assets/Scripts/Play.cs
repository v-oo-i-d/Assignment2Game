using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    private GameObject blackFaderMenu, blackFaderPanel, mainMenu;
    private const float interim_delay = 1f;
    private const float fade_duration = 1f;

    void Start()
    {
        mainMenu = gameObject;
        ToggleMenu(mainMenu, true);

        blackFaderMenu = GameObject.Find("GUIs").transform.Find("Fader").gameObject;
        blackFaderPanel = blackFaderMenu.transform.Find("BlackFader").gameObject;
        blackFaderPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        ToggleMenu(blackFaderMenu, false);
    }

    public void StartFade()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        ToggleMenu(blackFaderMenu, true);
        Image faderImage = blackFaderPanel.GetComponent<Image>();

        // Fade in
        yield return StartCoroutine(Fade(faderImage, 0f, 1f, fade_duration));

        // Wait
        ToggleMenu(mainMenu, false);
        yield return new WaitForSeconds(interim_delay);

        // Fade out
        yield return StartCoroutine(Fade(faderImage, 1f, 0f, fade_duration));

        ToggleMenu(blackFaderMenu, false, true);
        ToggleMenu(mainMenu, false, true);
    }

    private IEnumerator Fade(Image image, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }

    private void ToggleMenu(GameObject menu, bool state, bool toggleMenuItself = false)
    {
        if (toggleMenuItself)  menu.SetActive(state);

        foreach (Transform child in menu.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
}
