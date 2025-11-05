using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class Door : MonoBehaviour
{
    private bool isOpen = false;
	private bool isMoving = false;
	public bool isLocked = false;

    public float smooth = 5f;
	public float openAngle = 90f;
	public float maxDistance = 5f;

	private Quaternion closedRotation, openRotation;

	private GameObject carpet;
	private Color originalColour;
	private Color grayedColour = Color.gray;

    private void Start()
    {
        closedRotation = transform.localRotation;
		openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;

		carpet = transform.parent.Find("Carpet").transform.gameObject;
		
		originalColour = carpet.GetComponent<Renderer>().material.color;

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

        if (isMoving) OpenAnimation();
    }

	private bool LookingAtDoor()
	{
		Camera cam = Camera.main;
		Ray ray = new(cam.transform.position, cam.transform.forward);

		if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
		{
			// Check if we are looking at this door
			return hit.transform == transform;
		}

		return false;
	}

	private void OpenDoor()
	{
		isOpen = !isOpen;
		isMoving = true;
	}

	private void OpenAnimation()
	{
		Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);

        // Stop moving when close enough
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
			// Rotate one way
			Quaternion targetA = startRot * Quaternion.Euler(0, wiggleDegrees, 0);
			while (Quaternion.Angle(transform.localRotation, targetA) > 0.1f)
			{
				transform.localRotation = Quaternion.RotateTowards(
					transform.localRotation, targetA, wiggleSpeed * Time.deltaTime);
				yield return null;
			}

			// Rotate the other way
			Quaternion targetB = startRot * Quaternion.Euler(0, -wiggleDegrees, 0);
			while (Quaternion.Angle(transform.localRotation, targetB) > 0.1f)
			{
				transform.localRotation = Quaternion.RotateTowards(
					transform.localRotation, targetB, wiggleSpeed * Time.deltaTime);
				yield return null;
			}
		}

		// Return to start rotation
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
		Renderer carpetRenderer = carpet.GetComponent<Renderer>();

		Color startColor = doorRenderer.material.color;
		Color endColor = locked ? grayedColour : originalColour;
		float duration = 0.5f;
		float t = 0f;

		while (t < 1f)
		{
			t += Time.deltaTime / duration;
			Color lerpedColor = Color.Lerp(startColor, endColor, t);
			doorRenderer.material.color = lerpedColor;
			carpetRenderer.material.color = lerpedColor;
			yield return null;
		}

		doorRenderer.material.color = endColor;
		carpetRenderer.material.color = endColor;
	}
	
	public void Lock() => StartCoroutine(LerpLockColor(true));
	public void Unlock() => StartCoroutine(LerpLockColor(false));
}