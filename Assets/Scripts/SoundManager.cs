using UnityEngine;
using System.Collections;
using System;

public enum SoundType
{
    MenuClick,
    MenuHover,
    DoorOpen,
    DoorLock,
    Beep
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;

    private static GameObject bgMusicObject;
    private static AudioSource bgSource;
    private static AudioLowPassFilter bgLowPass;

    void Start()
    {
        instance = this;
        bgMusicObject = GameObject.Find("BackgroundMusic");
        bgSource = bgMusicObject.GetComponent<AudioSource>();
        bgLowPass = bgMusicObject.GetComponent<AudioLowPassFilter>();
    }

    public static void PlaySound(SoundType sound, float volume = 1.0f)
    {
        instance.GetComponent<AudioSource>().PlayOneShot(instance.soundList[(int)sound], volume);
    }

    public static IEnumerator FilterBackgroundMusic(float targetCutoff, float targetVolume, float duration)
    {
        float startCutoff = bgLowPass.cutoffFrequency;
        float startVolume = bgSource.volume;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            bgLowPass.cutoffFrequency = Mathf.Lerp(startCutoff, targetCutoff, t);
            bgSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

            yield return null;
        }

        // Ensure final values are exactly set
        bgLowPass.cutoffFrequency = targetCutoff;
        bgSource.volume = targetVolume;
    }
}