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
        float velMag = new Vector3(playerVelocity.x, 0, playerVelocity.z).magnitude;
        mAnimator.SetFloat("PlayerVelocity", velMag);
    }
}
