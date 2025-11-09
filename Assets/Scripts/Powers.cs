using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PowerType
{
    Red, Yellow, Blue, Null
}

public static class Powers
{
    public static bool IsActive { get; private set; } = false;
    private static Color DefaultPlayerColour = new(238 / 255f, 194 / 255f, 129 / 255f);
    [HideInInspector] public static int restartTimer = 0;
    [HideInInspector] public static bool changedAbility = false;
    private static float originalJump;
    [HideInInspector] public static float originalSpeed, originalFOV;


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

        // Speed up gradually
        float t = 0f;
        if (restartTimer == 0)
        {
            while (t < 1f)
            {
                t += Time.deltaTime;

                player.WalkSpeed = Mathf.Lerp(originalSpeed, originalSpeed * speedUpMultiplier, t);
                cam.fieldOfView = Mathf.Lerp(originalFOV, originalFOV * speedUpFOVMultiplier, t);

                yield return null;
            }
        }

        // Wait

        yield return new WaitForSeconds(speedUpDuration);

        if (restartTimer > 0)
        {
            restartTimer -= 1;
            yield break;
        }
        if (changedAbility)
        {
            changedAbility = false;
            yield break;
        }
        /*float wait = 0f;
        while (wait < speedUpDuration)
        {
            if (restartTimer)
            {
                restartTimer = false;
                yield break;
            }
            wait += Time.deltaTime;

            yield return null;
        }*/

        // Slow back down
        IsActive = false;
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
    }

    public static IEnumerator Red(PlayerCharacter player)
    {
        IsActive = true;

        // Fetch variables
        float strengthDuration = player.strengthDuration;
        SkinnedMeshRenderer renderer = player.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();

        // Change player colour
        renderer.material.color = Color.red;
        player.strengthened = true;

        // Wait
        yield return new WaitForSeconds(strengthDuration);
        if (restartTimer > 0)
        {
            restartTimer -= 1;
            yield break;
        }
        if (changedAbility) {
            changedAbility = false;
            yield break;
        }
        // Reset colour
        renderer.material.color = DefaultPlayerColour;
        player.strengthened = false;
        IsActive = false;
    }

    // Jump Boost Power
    public static IEnumerator Blue(PlayerCharacter player)
    {
        IsActive = true;

        // Fetch/set variables
        player.bigJumpUsesLeft = 3;
        SkinnedMeshRenderer renderer = player.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();

        // Change player colour
        renderer.material.color = Color.blue;

        // Store original values
        Powers.originalJump = player.JumpSpeed;

        // Set new jump speed
        player.JumpSpeed = originalJump * player.jumpMultiplier;

        // Wait until player runs out of jumps
        while (player.bigJumpUsesLeft != 0)
        {
            yield return null;
        }
        if (changedAbility)
        {
            changedAbility = false;
            yield break;
        }
        if (restartTimer > 0)
        {
            restartTimer--;
            yield break;
        }

        // Reset player
        renderer.material.color = DefaultPlayerColour;
        player.JumpSpeed = originalJump;
        IsActive = false;
    }
    public static void ClearPowers(PlayerCharacter player)
    {
        SkinnedMeshRenderer playerRenderer = player.transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        player.JumpSpeed = originalJump;
        playerRenderer.material.color = DefaultPlayerColour;
        player.strengthened = false;
        player.bigJumpUsesLeft = 0;

        if (player.WalkSpeed > originalSpeed)
        {
            CoroutineRunner.instance.StartCoroutine(ClearFOV(player));
        }
    }
    private static IEnumerator ClearFOV(PlayerCharacter player)
    {

        Camera cam = Camera.main;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;

            player.WalkSpeed = Mathf.Lerp(originalSpeed * player.speedUpMultiplier, originalSpeed, t);
            cam.fieldOfView = Mathf.Lerp(originalFOV * player.speedUpFOVMultiplier, originalFOV, t);
            yield return null;
        }
    }
}

