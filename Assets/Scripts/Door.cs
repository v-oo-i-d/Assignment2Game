using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Renderer))]
public class Door : MonoBehaviour
{
    private bool isOpen = false;
    private bool isMoving = false;
    public bool isLocked = false;

    [HideInInspector] public float smooth = 5f;
    [HideInInspector] public float openAngle = 90f;
    [HideInInspector] public float maxDistance = 5f;

    private Quaternion closedRotation, openRotation;

    private GameObject carpet;
    private Color originalColour;
    private readonly Color grayedColour = Color.gray;

    private void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = Quaternion.Euler(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + openAngle,
            transform.localEulerAngles.z
        );

        Transform carpetTransform = transform.parent ? transform.parent.Find("Carpet") : null;
        if (carpetTransform != null)
            carpet = carpetTransform.gameObject;

        originalColour = GetComponent<Renderer>().material.color;

        if (isLocked) Lock();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (LookingAtDoor())
            {
                if (!isLocked)
                    OpenDoor();
                else
                    StartCoroutine(LockedAnimation());
            }
        }

        if (isMoving)
            OpenAnimation();
    }

    private bool LookingAtDoor()
    {
        Camera cam = Camera.main;
        if (cam == null) return false;

        Ray ray = new(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            return hit.transform == transform;

        return false;
    }

    private void OpenDoor()
    {
        isOpen = !isOpen;
        isMoving = true;
        SoundManager.PlaySound(SoundType.DoorOpen);
    }

    private void OpenAnimation()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * smooth
        );

        if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.1f)
        {
            transform.localRotation = targetRotation;
            isMoving = false;
        }
    }

    private IEnumerator LockedAnimation()
    {
        if (isMoving) yield break;
        isMoving = true;

        float wiggleDegrees = 3f;
        float wiggleSpeed = 100f;
        int wiggles = 2;

        Quaternion startRot = transform.localRotation;

        for (int i = 0; i < wiggles; i++)
        {
            Quaternion targetA = startRot * Quaternion.Euler(0, wiggleDegrees, 0);
            while (Quaternion.Angle(transform.localRotation, targetA) > 0.1f)
            {
                transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation, targetA, wiggleSpeed * Time.deltaTime);
                yield return null;
            }

            Quaternion targetB = startRot * Quaternion.Euler(0, -wiggleDegrees, 0);
            while (Quaternion.Angle(transform.localRotation, targetB) > 0.1f)
            {
                transform.localRotation = Quaternion.RotateTowards(
                    transform.localRotation, targetB, wiggleSpeed * Time.deltaTime);
                yield return null;
            }
            SoundManager.PlaySound(SoundType.DoorLock);
        }

        while (Quaternion.Angle(transform.localRotation, startRot) > 0.1f)
        {
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation, startRot, wiggleSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }

    private IEnumerator LerpLockColor(bool locked)
    {
        if (!gameObject.activeInHierarchy) yield break;

        isLocked = locked;

        MeshRenderer doorRenderer = GetComponent<MeshRenderer>();
        Renderer carpetRenderer = carpet ? carpet.GetComponent<Renderer>() : null;

        Color startColor = doorRenderer.material.color;
        Color endColor = locked ? grayedColour : originalColour;
        float duration = 0.5f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Color lerpedColor = Color.Lerp(startColor, endColor, t);
            doorRenderer.material.color = lerpedColor;
            if (carpetRenderer != null)
                carpetRenderer.material.color = lerpedColor;
            yield return null;
        }

        doorRenderer.material.color = endColor;
        if (carpetRenderer != null)
            carpetRenderer.material.color = endColor;
    }

    public void Lock() => StartCoroutine(LerpLockColor(true));
    public void Unlock() => StartCoroutine(LerpLockColor(false));
}