using System;
using System.Collections;
using UnityEngine;

public enum PowerType
{
    Red, Yellow, Blue
}

public static class Powers
{
    public static bool IsActive { get; private set; } = false;
    private static Color DefaultPlayerColour = new(238 / 255f, 194 / 255f, 129 / 255f);

    public static IEnumerator Yellow(PlayerCharacter player)
    {
        IsActive = true;

        // Fetch variables
        Camera cam = player.GetComponentInChildren<Camera>();
        SkinnedMeshRenderer renderer = player.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        float speedUpDuration = player.speedUpDuration;
        float speedUpMultiplier = player.speedUpMultiplier;
        float speedUpFOVMultiplier = player.speedUpFOVMultiplier;

        // Change player colour
        renderer.material.color = Color.yellow;

        // Store original values
        float originalSpeed = player.WalkSpeed;
        float originalFOV = cam.fieldOfView;

        // Speed up gradually
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            player.WalkSpeed = Mathf.Lerp(originalSpeed, originalSpeed * speedUpMultiplier, t);
            cam.fieldOfView = Mathf.Lerp(originalFOV, originalFOV * speedUpFOVMultiplier, t);
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(speedUpDuration);

        // Slow back down
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            player.WalkSpeed = Mathf.Lerp(originalSpeed * speedUpMultiplier, originalSpeed, t);
            cam.fieldOfView = Mathf.Lerp(originalFOV * speedUpFOVMultiplier, originalFOV, t);
            yield return null;
        }

        // Reset colour
        renderer.material.color = DefaultPlayerColour;

        IsActive = false;
    }


    public static IEnumerator Red(PlayerCharacter player)
    {
        yield return null;
    }

    public static IEnumerator Blue(PlayerCharacter player)
    {
        yield return null;
    }
}
