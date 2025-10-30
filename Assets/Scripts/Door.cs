using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
	private bool isMoving = false;
	public bool isLocked = false; // future use

    public float smooth = 5f;
	public float openAngle = 90f;
	public float maxDistance = 5f;

    private Quaternion closedRotation, openRotation;

    private void Start()
    {
        closedRotation = transform.localRotation;
		openRotation = Quaternion.Euler(0, openAngle, 0) * closedRotation;
		
		if (isLocked)
		{
			// Gray out the door to indicate it's locked
			MeshRenderer renderer = transform.GetComponent<MeshRenderer>();
			Color original = renderer.material.color;
			float gray = original.grayscale;
			renderer.material.color = new Color(gray, gray, gray, original.a);        
		}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
			if (CanOpenDoor() && !isLocked) ToggleDoor();
			else if (isLocked) StartCoroutine(LockedAnimation());
        }

        if (isMoving) OpenAnimation();
    }

	private bool CanOpenDoor()
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
		if (isMoving) yield break; // Prevent interrupting another animation
		isMoving = true;

		float wiggleDegrees = 5f;
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

	private void ToggleDoor()
	{
		isOpen = !isOpen;
		isMoving = true;
	}
	
	public void Lock(bool locked) => isLocked = locked;
}
