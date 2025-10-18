using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private CharacterController mController;
    public Vector3 mVelocity;
    private Transform mCamera;
    public bool mLanded = true;
    public bool mJump = false;

    private InputAction mMoveAction, mLookAction, mJumpAction;

    //Feel free to change these values
    public float WalkSpeed = 1.894965f;
    public float Acceleration = 5.0f;
    public float JumpSpeed = 5.0f;

    void Start()
    {
        mController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Set Velocity to (0,0,0)
        mVelocity = Vector3.zero;

        mCamera = transform.Find("Player Camera");

        mMoveAction = InputSystem.actions.FindAction("Move");
        mLookAction = InputSystem.actions.FindAction("Look");
        mJumpAction = InputSystem.actions.FindAction("Jump");
    }

    private void Jump()
    {
        // Make character jump up
        mVelocity.y = JumpSpeed;

        // Player is no longer landed
        mLanded = false;
    }

    void Update()
    {
        Vector2 look = mLookAction.ReadValue<Vector2>();

        // Look left/right
        mCamera.Rotate(Vector3.up, look.x, Space.World);
        mCamera.Rotate(Vector3.right, -look.y, Space.Self);

        // Modify movement speed - Walking, Running
        float moveModifier = 0;
        
        // Player Walking
        moveModifier = WalkSpeed;

        Vector2 move = mMoveAction.ReadValue<Vector2>();

        // Calculate target velocity on xz plane
        Vector3 targetVelocity = Vector3.zero;
        targetVelocity = (transform.forward * move.y +
                          transform.right * move.x) *
                          moveModifier;

        // Update Velocity
        mVelocity.z = Mathf.MoveTowards(mVelocity.z, targetVelocity.z, Acceleration * Time.deltaTime);
        mVelocity.x = Mathf.MoveTowards(mVelocity.x, targetVelocity.x, Acceleration * Time.deltaTime);

        if (mController.isGrounded)
        {
            // Set Velocity -2 (keep player on the ground)
            mVelocity.y = -2f;

            // User Pressed Space
            if (mJumpAction.WasPerformedThisFrame())
            {
                // Make Character Jump
                Jump();
            }

            // If Player has just Landed
            if (mLanded == false)
            {
                // Player has landed
                mLanded = true;
            }
        }
        else
        {
            // Accelerate Character due to Gravity
            mVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
        
        // Move Character
        mController.Move(mVelocity * Time.deltaTime);
    }
}
