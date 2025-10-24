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
        mAnimator.SetFloat("StrafeMultiplier", playerMoveSpeed * 0.3f);
    }

    void Update()
    {
        Vector3 playerVelocity = GetComponent<PlayerCharacter>().GetVelocity();
        
        // Horizontal movement only
        float velMag = new Vector3(playerVelocity.x, 0, playerVelocity.z).magnitude;
        float forwardVelocity = 0f; // forwards and backwards
        float sidewaysVelocity = 0f; // side to side

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            forwardVelocity = velMag;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            forwardVelocity = -velMag;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            sidewaysVelocity = velMag;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            sidewaysVelocity = -velMag;

        mAnimator.SetFloat("ForwardVelocity", forwardVelocity);
        mAnimator.SetFloat("SidewaysVelocity", sidewaysVelocity);
    }
}
