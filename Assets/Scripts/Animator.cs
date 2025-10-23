using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator mAnimator;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 playerVelocity = GetComponent<PlayerCharacter>().GetVelocity();
        
        // Horizontal movement only
        float velMag = new Vector3(playerVelocity.x, 0, playerVelocity.z).magnitude;

        float directionalVelocity = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            directionalVelocity = velMag;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            directionalVelocity = -velMag;
        }

        mAnimator.SetFloat("Speed", directionalVelocity);
    }
}
