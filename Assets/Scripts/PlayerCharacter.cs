using System;
using System.Collections;
using System.Collections.Generic;
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

    private readonly HashSet<KeycardType> heldKeycards = new();

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
        if (mController.isGrounded) mVelocity.y = JumpSpeed;
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

    public void AbsorbPower(PowerType type)
    {
        if (Powers.IsActive) return;

        switch (type)
        {
            case PowerType.Red:
                StartCoroutine(Powers.Red(this));
                break;
            case PowerType.Yellow:
                StartCoroutine(Powers.Yellow(this));
                break;
            case PowerType.Blue:
                StartCoroutine(Powers.Blue(this));
                break;
        }
    }
    
    public void PickupKeycard(Keycard keycard) => heldKeycards.Add(keycard.type);
    public void UseKeycard(Keycard keycard) => heldKeycards.Remove(keycard.type);
    public bool HasKeycard(Keycard keycard) => heldKeycards.Contains(keycard.type);
}