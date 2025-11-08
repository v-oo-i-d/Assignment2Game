using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlay : MonoBehaviour
{
    private GameObject blackFaderMenu, blackFaderPanel, mainMenu, blurMenu;
    private const float interim_delay = 1f;
    private const float fadeOut_duration = 1f;
    private const float fadeIn_duration = .2f;

    void Start()
    {
        mainMenu = gameObject;
        ToggleMenu(mainMenu, true);

        blackFaderMenu = GameObject.Find("GUIs").transform.Find("Fader").gameObject;
        blackFaderPanel = blackFaderMenu.transform.Find("BlackFader").gameObject;
        blackFaderPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        blurMenu = GameObject.Find("GUIs").transform.Find("Blur").gameObject;
        
        ToggleMenu(blackFaderMenu, false);
    }

    public void StartFade()
    {
        StartCoroutine(FadeSequence());
        StartCoroutine(SoundManager.FilterBackgroundMusic(250f, 0.15f, fadeOut_duration + interim_delay + fadeIn_duration));
    }

    private IEnumerator FadeSequence()
    {
        ToggleMenu(blackFaderMenu, true);
        Image faderImage = blackFaderPanel.GetComponent<Image>();

        // Fade in
        yield return StartCoroutine(Fade(faderImage, 0f, 1f, fadeIn_duration));

        // Wait
        ToggleMenu(mainMenu, false);
        ToggleMenu(blurMenu, false, true);
        SwitchCameras();
        GameObject.Find("Player").GetComponent<PlayerCharacter>().LockCursor();
        yield return new WaitForSeconds(interim_delay);

        // Fade out
        yield return StartCoroutine(Fade(faderImage, 1f, 0f, fadeOut_duration));

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

    private void SwitchCameras()
    {
        Camera mainMenuCamera = GameObject.Find("SpinningCamera").GetComponent<Camera>();
        Camera playerCamera = GameObject.Find("Player").transform.Find("Player Camera").GetComponent<Camera>();

        mainMenuCamera.enabled = false;
        mainMenuCamera.tag = "Untagged";

        playerCamera.enabled = true;
        playerCamera.tag = "MainCamera";
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
