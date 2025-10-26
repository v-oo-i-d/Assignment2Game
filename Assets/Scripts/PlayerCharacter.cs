using System;
using System.Collections;
using System.Diagnostics.Tracing;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    private CharacterController mController;
    private Vector3 mVelocity;
    private InputAction mMoveAction, mLookAction, mJumpAction;
    private Transform mCamera;
    private float cameraPitch = 0f;

    // Feel free to change these values
    public float WalkSpeed = 5.0f;
    public float Acceleration = 75.0f;
    public float JumpSpeed = 9.0f;
    public float MouseSensitivity = .2f;
    public float MaxLookAngle = 89f;
    public float Gravity = -35f;

    [Header("Strength Power Settings")]
    public float x;

    [Header("Speed Power Settings")]
    public float speedUpDuration = 10f;
    public float speedUpMultiplier = 3f;
    public float speedUpFOVMultiplier = 1.5f;

    [Header("Jump Power Settings")]
    public float y;

    [Header("Player Colours")]
    public Material BlueMaterial;
    public Material RedMaterial;
    public Material YellowMaterial; 
    public Material DefaultMaterial;

    void Start()
    {
        mController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        mVelocity = Vector3.zero;

        mCamera = transform.Find("Player Camera");

        mMoveAction = InputSystem.actions.FindAction("Move");
        mLookAction = InputSystem.actions.FindAction("Look");
        mJumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        HandleLook();
        HandleMovement();

        if (mJumpAction.WasPerformedThisFrame()) Jump();
    }

    private void Jump()
    {
        if (mController.isGrounded)
        {
            mVelocity.y = JumpSpeed;
        }
    }

    private void HandleLook()
    {
        Vector2 look = mLookAction.ReadValue<Vector2>() * MouseSensitivity;

        cameraPitch -= look.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -MaxLookAngle, MaxLookAngle);
        mCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

        transform.Rotate(Vector3.up * look.x);
    }

    private void HandleMovement()
    {
        Vector2 move = mMoveAction.ReadValue<Vector2>();
        Vector3 targetVelocity = (transform.forward * move.y + transform.right * move.x) * WalkSpeed;

        // Accelerate
        Vector3 currentHorizontal = new(mVelocity.x, 0f, mVelocity.z);
        Vector3 targetHorizontal = new(targetVelocity.x, 0f, targetVelocity.z);
        currentHorizontal = Vector3.MoveTowards(currentHorizontal, targetHorizontal, Acceleration * Time.deltaTime);

        mVelocity.x = currentHorizontal.x;
        mVelocity.z = currentHorizontal.z;

        // Apply gravity
        if (mController.isGrounded && mVelocity.y < 0) mVelocity.y = -2f;
        mVelocity.y += Gravity * Time.deltaTime;

        mController.Move(mVelocity * Time.deltaTime);
    }
    
    public Vector3 GetVelocity() => mVelocity;

    public void AbsorbPower(string colour)
    {
        switch (colour)
        {
            case "Red": StartCoroutine(AbsorbRed()); break;
            case "Yellow": StartCoroutine(AbsorbYellow()); break;
            case "Blue": StartCoroutine(AbsorbBlue()); break;
        }
    }

    private IEnumerator AbsorbRed()
    {
        yield return null;
    }

    private IEnumerator AbsorbYellow()
    {
        Camera cam = mCamera.GetComponent<Camera>();
        SkinnedMeshRenderer renderer = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();

        // Set character model colour
        if (YellowMaterial != null)
            renderer.material = YellowMaterial;

        // Store original values
        float originalSpeed = WalkSpeed;
        float originalFOV = cam.fieldOfView;

        // Increase speed & FOV
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            WalkSpeed = Mathf.Lerp(originalSpeed, originalSpeed * speedUpMultiplier, t);
            cam.fieldOfView = Mathf.Lerp(originalFOV, originalFOV * speedUpFOVMultiplier, t);
            yield return null;
        }

        // Wait
        yield return new WaitForSeconds(speedUpDuration);

        // Decrease speed & FOV
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            WalkSpeed = Mathf.Lerp(originalSpeed * speedUpMultiplier, originalSpeed, t);
            cam.fieldOfView = Mathf.Lerp(originalFOV * speedUpFOVMultiplier, originalFOV, t);
            yield return null;
        }

        // Revert character model colour
        if (DefaultMaterial != null)
            renderer.material = DefaultMaterial;
    }
    
    private IEnumerator AbsorbBlue()
    {
        yield return null;
    }
}