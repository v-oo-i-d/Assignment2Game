using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator mAnimator;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        float playerMoveSpeed = GetComponent<PlayerCharacter>().WalkSpeed;

        // Set animation speed multipliers based on player movement speed
        mAnimator.SetFloat("MoonwalkMultiplier", playerMoveSpeed * 0.243f);
        mAnimator.SetFloat("WalkMultiplier", playerMoveSpeed * 0.243f);
        mAnimator.SetFloat("LeftStrafeMultiplier", playerMoveSpeed * 0.5f);
        mAnimator.SetFloat("RightStrafeMultiplier", playerMoveSpeed * 0.2f);
    }

    void Update()
    {
        // Vector3 playerVelocity = GetComponent<PlayerCharacter>().GetVelocity();
        
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 playerVelocity = GetComponent<PlayerCharacter>().GetVelocity();
        float speed = new Vector3(playerVelocity.x, 0, playerVelocity.z).magnitude;

        mAnimator.SetFloat("ForwardVelocity", inputZ * speed);
        mAnimator.SetFloat("SidewaysVelocity", inputX * speed);
    }
}
