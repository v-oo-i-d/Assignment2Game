using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
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
    //public float x;
    public float strengthDuration = 10f;
    public bool strengthened = false;

    [Header("Speed Power Settings")]
    public float speedUpDuration = 10f;
    public float speedUpMultiplier = 3f;
    public float speedUpFOVMultiplier = 1.5f;

    [Header("Jump Power Settings")]
    //public float y;
    public int bigJumpUsesLeft = 0;
    public float jumpMultiplier = 2f;

    private readonly HashSet<KeycardType> heldKeycards = new();

    void Start()
    {
        mController = GetComponent<CharacterController>();

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

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Jump()
    {
        if (bigJumpUsesLeft != 0) { bigJumpUsesLeft--; }
        Debug.Log(JumpSpeed);
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
    
    void OnControllerColliderHit(ControllerColliderHit collidedObject)
    {
        if (!strengthened) { return; } // Doesn't have strength active
        
        Rigidbody body = collidedObject.collider.attachedRigidbody;

        // Return if object is not a rigidbody
        if (body == null || body.isKinematic) { return; }
        
        Vector3 pushDirection = new Vector3(collidedObject.moveDirection.x, 0, collidedObject.moveDirection.z);
        
        float pushPower = 1.0f;         

        // Apply force to the pushable object
        body.AddForce(pushDirection * pushPower, ForceMode.Impulse);
    }

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