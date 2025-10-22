using System.Diagnostics.Tracing;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    private CharacterController mController;
    private Vector3 mVelocity;
    private Transform mCamera;
    private float cameraPitch = 0f;
    // private bool mLanded = true;
    // private bool mJump = false;

    private InputAction mMoveAction, mLookAction, mJumpAction;

    // Feel free to change these values
    public float WalkSpeed = 7.0f;
    public float Acceleration = 75.0f;
    public float JumpSpeed = 9.0f;
    public float MouseSensitivity = .2f;
    public float MaxLookAngle = 89f;
    public float Gravity = -35f;

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

    private void Jump()
    {
        if (mController.isGrounded)
        {
            mVelocity.y = JumpSpeed;
        }
    }

    void Update()
    {
        HandleLook();
        HandleMovement();

        if (mJumpAction.WasPerformedThisFrame()) Jump();
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

    public void AbsorbPower(string colour)
    {
        switch (colour)
        {
            case "Red":
                Debug.Log("Absorb Red");
                break;
            case "Yellow":
                Debug.Log("Absorb Yellow");
                break;
            case "Blue":
                Debug.Log("Absorb Blue");
                break;
        }
    }
}
